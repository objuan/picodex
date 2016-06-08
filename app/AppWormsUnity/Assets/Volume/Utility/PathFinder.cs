using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Picodex
{
    public delegate void PathFinderHandler(PathFinder finder);

    public class PathFinder
    {
        DFVolume world;

        Dictionary<Vector3i, Heuristics> open = new Dictionary<Vector3i, Heuristics>();
        Dictionary<Vector3i, Heuristics> closed = new Dictionary<Vector3i, Heuristics>();

        public List<Vector3i> path = new List<Vector3i>();

        Vector3i targetLocation;
        Vector3i startLocation;

     //   int entityHeight;

        public float range = 2;
        float distanceFromStartToTarget = 0;
        float maxDistToTravelAfterDirect = 80;
        float maxDistToTravelMultiplier = 2;
        byte isoValue;
        int MAX_Z;
        int MAX_Y;
        int MAX_X;

        public event PathFinderHandler OnEnd;

        struct Heuristics
        {
            /// Real distance from start
            public float g;
            /// Estimated distance to target
            public float h;

            public Vector3i parent;

            public Heuristics(float g, float h, Vector3i parent)
            {
                this.g = g;
                this.h = h;
                this.parent = parent;
            }
        };

        public PathFinder(DFVolume world)
        {
            isoValue = world.Accessor.isoValue;
            this.world = world;
            MAX_X = world.resolution.x - 1;
            MAX_Y = world.resolution.y - 1;
            MAX_Z = world.resolution.z - 1;

          
        }

        public void Start(Vector3i startLocation, Vector3i targetLocation)
        {
            lock(this)
            {
                path.Clear();
                open.Clear();
                closed.Clear();

                this.startLocation = startLocation;
                this.targetLocation = targetLocation;
                distanceFromStartToTarget = Distance(startLocation, targetLocation);

                //    this.entityHeight = entityHeight;

                open.Add(startLocation, new Heuristics(0, distanceFromStartToTarget, startLocation));
            }

            if (true)//Toggle.UseMultiThreading)
            {
                Thread thread = new Thread(() =>
                {
                   while (path.Count == 0)
                   {
                        lock (this)
                        {
                            ProcessBest();
                        }
                        // update();
                    }
                    // END
                    if (OnEnd!=null)
                        OnEnd(this);
                });
                thread.Start();
            }
            //else
            //{
            //    while (path.Count == 0)
            //    {
            //        update();
            //    }
            //}
        }

        //public void update()
        //{
        //    if (path.Count == 0)
        //    {
        //        ProcessBest();
        //    }
        //}

        void PathComplete(Vector3i lastTile)
        {
            Heuristics pos;
            closed.TryGetValue(lastTile, out pos);
            path.Clear();
            path.Add(lastTile);

            open.TryGetValue(lastTile, out pos);

            while (!pos.parent.Equals(startLocation))
            {
                path.Insert(0, pos.parent);
                if (!closed.TryGetValue(pos.parent, out pos))
                    break;
            }
        }

        void ProcessBest()
        {
            float shortestDist = (distanceFromStartToTarget * maxDistToTravelMultiplier) + maxDistToTravelAfterDirect;
            Vector3i bestPos = new Vector3i(0, 10000, 0);

            foreach (var tile in open)
            {
                if (tile.Value.g + tile.Value.h < shortestDist)
                {
                    bestPos = tile.Key;
                    shortestDist = tile.Value.g + tile.Value.h;
                }
            }

            Heuristics parent;
            open.TryGetValue(bestPos, out parent);

            //  if (Distance(new Vector3i(((Vector3)bestPos) + (Vector3.up * 2)), targetLocation) <= range)
            if (Distance( bestPos , targetLocation) <= range)
            {
                PathComplete(bestPos);
                return;
            }

            if (bestPos.Equals(new Vector3i(0, 10000, 0)))
            {
                Debug.Log("Failed to pf " + targetLocation.x + ", " + targetLocation.y + ", " + targetLocation.z);
                bestPos = new Vector3i(0, 10000, 0);

                foreach (var tile in open)
                {
                    if (tile.Value.g + tile.Value.h < shortestDist)
                    {
                        bestPos = tile.Key;
                        shortestDist = tile.Value.g + tile.Value.h;
                    }
                }
                PathComplete(bestPos);
            }

            ProcessTile(bestPos);
        }

        void ProcessTile(Vector3i pos)
        {
            Heuristics h = new Heuristics();
            bool exists = open.TryGetValue(pos, out h);

            if (!exists)
                return;

            open.Remove(pos);
            closed.Add(pos, h);

            CheckAdjacent(pos, h);
        }

        void CheckAdjacent(Vector3i pos, Heuristics dist)
        {
            List<Vector3i> adjacentPositions = new List<Vector3i>();
            List<float> distanceFromStart = new List<float>();

            //Cardinal directions
            if (pos.z < MAX_Z)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y, pos.z + 1));
                distanceFromStart.Add(dist.g + 1);
            }
            if (pos.x < MAX_X)
            {
                adjacentPositions.Add(new Vector3i(pos.x + 1, pos.y, pos.z));
                distanceFromStart.Add(dist.g + 1);
            }
            if (pos.z >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y, pos.z - 1));
                distanceFromStart.Add(dist.g + 1);
            }
            if (pos.x> 0)
            {
                adjacentPositions.Add(new Vector3i(pos.x - 1, pos.y, pos.z));
                distanceFromStart.Add(dist.g + 1);
            }

            //diagonal directions
            if (pos.x < MAX_X && pos.z < MAX_Z)
            {
                adjacentPositions.Add(new Vector3i(pos.x + 1, pos.y, pos.z + 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.x < MAX_X && pos.z >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x + 1, pos.y, pos.z - 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.x >0 && pos.z >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x - 1, pos.y, pos.z - 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.x >0 && pos.z < MAX_Z)
            {
                adjacentPositions.Add(new Vector3i(pos.x - 1, pos.y, pos.z + 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }

            //climb up directions
            if (pos.y < MAX_Y && pos.z < MAX_Z)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y + 1, pos.z + 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y < MAX_Y && pos.x < MAX_X)
            {
                adjacentPositions.Add(new Vector3i(pos.x + 1, pos.y + 1, pos.z));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y < MAX_Y && pos.z >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y + 1, pos.z - 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y < MAX_Y && pos.x >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x - 1, pos.y + 1, pos.z));
                distanceFromStart.Add(dist.g + 1.414f);
            }

            //climb down directions
            if (pos.y >0  && pos.z < MAX_Z)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y - 1, pos.z + 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y > 0 && pos.x < MAX_X)
            {
                adjacentPositions.Add(new Vector3i(pos.x + 1, pos.y - 1, pos.z));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y > 0 && pos.z >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x, pos.y - 1, pos.z - 1));
                distanceFromStart.Add(dist.g + 1.414f);
            }
            if (pos.y > 0 && pos.x >0)
            {
                adjacentPositions.Add(new Vector3i(pos.x - 1, pos.y - 1, pos.z));
                distanceFromStart.Add(dist.g + 1.414f);
            }

            for (int i = 0; i < adjacentPositions.Count; i++)
            {
                if (!closed.ContainsKey(adjacentPositions[i]))
                {

                    var h = new Heuristics(
                                distanceFromStart[i],
                                Distance(targetLocation,
                                adjacentPositions[i]),
                                pos);

                    if (IsWalkable(world, adjacentPositions[i]))
                    {

                        Heuristics existingTile;
                        if (open.TryGetValue(adjacentPositions[i], out existingTile))
                        {
                            if (existingTile.g > distanceFromStart[i])
                            {
                                open.Remove(adjacentPositions[i]);
                                open.Add(adjacentPositions[i], h);
                            }

                        }
                        else
                        {
                            open.Add(adjacentPositions[i], h);
                        }

                    }
                }

            }

        }

        public bool IsWalkable(DFVolume world, Vector3i pos)
        {
            // fuori bound ?? 
            //int sum = pos.x + pos.y + pos.z;
            //if (sum == 0 || sum == MAX_SUM) return false;

            // get value
            byte val = world.Accessor.GetDistanceField(pos);
            return val > 0 && val <= isoValue;//TODO

            //TODO
            //Block block = world.GetBlock(pos);

            //if (!block.controller.CanBeWalkedOn(block))
            //    return false;

            //for (int y = 1; y < entityHeight + 1; y++)
            //{
            //    block = world.GetBlock(pos.Add(0, y, 0));

            //    if (!block.controller.CanBeWalkedThrough(block))
            //    {
            //        return false;
            //    }
            //}

           // return true;

        }

        public static float Distance(Vector3i a, Vector3i b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;
            var z = a.z - b.z;

            if (x < 0)
                x *= -1;

            if (y < 0)
                y *= -1;

            if (z < 0)
                z *= -1;

            return x + y + z;
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;
            var z = a.z - b.z;

            if (x < 0)
                x *= -1;

            if (y < 0)
                y *= -1;

            if (z < 0)
                z *= -1;

            return x + y + z;
        }
    }
}