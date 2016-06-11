using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Picodex
{
    public delegate void PathFinderHandler(PathFinder finder);

    public class PathFinder
    {
        DFVolume world;
        Vxcm.VXCMVolumeAccessor accessor;

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

        Vector3i[] nextDir = new Vector3i[26];
        float[] nextDist = new float[26];

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
            accessor = world.Accessor;
            MAX_X = world.resolution.x - 1;
            MAX_Y = world.resolution.y - 1;
            MAX_Z = world.resolution.z - 1;

            int idx = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;

                        nextDir[idx] =  new Vector3i(x,y,z);
                        nextDist[idx] = nextDir[idx].magnitude;
                        idx++;
                    }
                }
            }
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
            // final, target pos
            //path.Add(targetLocation);
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

            Vector3i p;
            for (int i=0;i< nextDir.Length;i++)
            {
                p = pos + nextDir[i];

                if (!closed.ContainsKey(p))
                {
                    byte val = accessor.GetDistanceField(p);
                    if (val > 0 && val <= isoValue)
                    {
                        adjacentPositions.Add(p);
                        distanceFromStart.Add(dist.g + nextDist[i]);
                    }
                }
            }
          
            for (int i = 0; i < adjacentPositions.Count; i++)
            {
              //  if (!closed.ContainsKey(adjacentPositions[i]))
                {
                    var h = new Heuristics(
                                distanceFromStart[i],
                                Distance(targetLocation,
                                adjacentPositions[i]),
                                pos);

                   // if (IsWalkable(world, adjacentPositions[i]))
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

        public bool IsWalkable( Vector3i pos)
        {

            // get value
            byte val = world.Accessor.GetDistanceField(pos);
            return val > 0 && val <= isoValue;//TODO
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