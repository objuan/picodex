using UnityEngine;
using System;
using System.Collections.Generic;

namespace Picodex
{
    /// <summary>
    /// Based on uniform-cost-search/A* from the book
    /// Artificial Intelligence: A Modern Approach 3rd Ed by Russell/Norvig 
    /// </summary>
    public class ShortestPathGraphSearch
    {
        #region FLOAT
        /// <summary>
        /// Workaround for http://monotouch.net/Documentation/Limitations Value types as Dictionary Keys (iOS)
        /// </summary>
        class Float : IComparable<Float>
        {
            public readonly float f;

            public Float(float f)
            {
                this.f = f;
            }

            public int CompareTo(Float other)
            {
                return f.CompareTo(other.f);
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Float p = obj as Float;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (f == p.f);
            }

            public bool Equals(Float p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (f == p.f);
            }
            public override int GetHashCode()
            {
                return f.GetHashCode();
            }
        }
        #endregion

        DFVolume volume;

        byte isoValue;
        int MAX_Z;
        int MAX_Y;
        int MAX_X;

        List<Vector3i> adjacentPositions;

        //  private IShortestPath<State,Action> info;
        public ShortestPathGraphSearch(DFVolume volume)
        {
            this.volume = volume;
            isoValue = volume.Accessor.isoValue;
            MAX_X = volume.resolution.x - 1;
            MAX_Y = volume.resolution.y - 1;
            MAX_Z = volume.resolution.z - 1;
            adjacentPositions = new List<Vector3i>();
        }

        public List<Vector3i> Start(Vector3i fromState, Vector3i toState)
        {
#if UNITY_IPHONE
		PriorityQueue<Float,SearchNode<State,Vector3i>> frontier = new PriorityQueue<Float,SearchNode<State,Vector3i>>();
#else
            PriorityQueue<float, SearchNode<Vector3i, Vector3i>> frontier = new PriorityQueue<float, SearchNode<Vector3i, Vector3i>>();
#endif
            HashSet<Vector3i> exploredSet = new HashSet<Vector3i>();
            Dictionary<Vector3i, SearchNode<Vector3i, Vector3i>> frontierMap = new Dictionary<Vector3i, SearchNode<Vector3i, Vector3i>>();

            SearchNode<Vector3i, Vector3i> startNode = new SearchNode<Vector3i, Vector3i>(null, 0, 0, fromState, default(Vector3i));
#if UNITY_IPHONE
		frontier.Enqueue(startNode,new Float(0));
#else
            frontier.Enqueue(startNode, 0);
#endif

            frontierMap.Add(fromState, startNode);

            while (!frontier.IsEmpty)
            {
                SearchNode<Vector3i, Vector3i> node = frontier.Dequeue();
                frontierMap.Remove(node.state);

                if (node.state.Equals(toState)) return BuildSolution(node);
                exploredSet.Add(node.state);
                // expand node and add to frontier
                foreach (Vector3i action in Expand(node.state))
                {
                    Vector3i child = ApplyAction(node.state, action);
                    
                    SearchNode<Vector3i, Vector3i> frontierNode = null;
                    bool isNodeInFrontier = frontierMap.TryGetValue(child, out frontierNode);
                    if (!exploredSet.Contains(child) && !isNodeInFrontier)
                    {
                        SearchNode<Vector3i, Vector3i> searchNode = CreateSearchNode(node, action, child, toState);
#if UNITY_IPHONE
					    frontier.Enqueue(searchNode,new Float(searchNode.f));
#else
                        frontier.Enqueue(searchNode, searchNode.f);
#endif
                        frontierMap.Add(child, searchNode);
                    }
                    else if (isNodeInFrontier)
                    {
                        SearchNode<Vector3i, Vector3i> searchNode = CreateSearchNode(node, action, child, toState);
                        if (frontierNode.f > searchNode.f)
                        {
#if UNITY_IPHONE
						    frontier.Replace(frontierNode,new Float(frontierNode.f), new Float(searchNode.f));
#else
                            frontier.Replace(frontierNode, frontierNode.f, searchNode.f);
#endif
                        }
                    }
                    
                }
            }

            return null;
        }

        private SearchNode<Vector3i, Vector3i> CreateSearchNode(SearchNode<Vector3i, Vector3i> node, Vector3i action, Vector3i child, Vector3i toState)
        {
            float cost = ActualCost(node.state, action);
            float heuristic = Heuristic(child, toState);
            return new SearchNode<Vector3i, Vector3i>(node, node.g + cost, node.g + cost + heuristic, child, action);
        }

        private List<Vector3i> BuildSolution(SearchNode<Vector3i, Vector3i> seachNode)
        {
            List<Vector3i> list = new List<Vector3i>();
            while (seachNode != null)
            {
                if ((seachNode.action != null) && (!seachNode.action.Equals(default(Vector3i))))
                {
                    list.Insert(0, seachNode.action);
                }
                seachNode = seachNode.parent;
            }
            return list;
        }

        // ==============================================================
        public bool IsWalkable(Vector3i pos)
        {
            // fuori bound ?? 
            //int sum = pos.x + pos.y + pos.z;
            //if (sum == 0 || sum == MAX_SUM) return false;

            // get value
            byte val = volume.Accessor.GetDistanceField(pos);
            return val > 0 && val <= isoValue;//TODO
        }

        /**
 * Should return a estimate of shortest distance. The estimate must me admissible (never overestimate)
 */
        public float Heuristic(Vector3i fromLocation, Vector3i toLocation)
        {
            if (false)//moveDiagonal)
            {
                return (fromLocation - toLocation).magnitude; // return straight line distance
            }
            else
            {
                Vector3i res = fromLocation - toLocation;
                return Mathf.Abs(res.x) + Mathf.Abs(res.y) + Mathf.Abs(res.z); // manhatten-distance
            }
        }

        /**
         * Return the legal moves from position
         */
        public List<Vector3i> Expand(Vector3i state)
        {
            List<Vector3i> res = new List<Vector3i>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                    {
                     //   if (x == 0 && y == 0 && z==0) continue;
                        Vector3i action = new Vector3i(x, y,z);
                        Vector3i newState = ApplyAction(state, action);

                        if (IsWalkable(newState))
                            {
                            //   if (newState.magnitude == 0) continue; // location 0,0 is blocked
                            //  if (!moveDiagonal && x * y != 0) continue; // 
                            res.Add(action);
                        }
                    }
            return res;
        }

        /**
         * Return the actual cost between two adjecent locations
         */
        public float ActualCost(Vector3i fromLocation, Vector3i toLocation)
        {
            return (fromLocation - toLocation).magnitude;
        }

        public Vector3i ApplyAction(Vector3i state, Vector3i action)
        {
            return state + action;
        }
    }

    class SearchNode<State, Action> : IComparable<SearchNode<State, Action>>
    {
        public SearchNode<State, Action> parent;

        public State state;
        public Action action;
        public float g; // cost
        public float f; // estimate

        public SearchNode(SearchNode<State, Action> parent, float g, float f, State state, Action action)
        {
            this.parent = parent;
            this.g = g;
            this.f = f;
            this.state = state;
            this.action = action;
        }

        /// <summary>
        /// Reverse sort order (smallest numbers first)
        /// </summary>
        public int CompareTo(SearchNode<State, Action> other)
        {
            return other.f.CompareTo(f);
        }

        public override string ToString()
        {
            return "SN {f:" + f + ", state: " + state + " action: " + action + "}";
        }
    }
}