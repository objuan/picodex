﻿using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Picodex
{
    public abstract class VolumeNavigator
    {
        protected DFVolume volume;
        protected Vxcm.VXCMVolumeAccessor accessor;

        protected  byte isoValue;
        protected int MAX_X, MAX_Y, MAX_Z;

    //    protected VolumePathPoint _targetPos;
        protected DFVolumeCollider collider;
        protected VolumePath _volumePath;

        protected Transform transform;
        protected Vector3i gridToObjectOffset;
        protected Matrix4x4 localToWorldMatrix;

        public Vector3 targetPos;

        public VolumePath volumePath
        {
            get
            {
                return _volumePath;
            }
        }

        public int Count
        {
            get
            {
                return volumePath.pointList.Count;
            }
        }
        public VolumePathPoint nextPoint
        {
            get
            {
                return volumePath.pointList[0];
            }
        }

        public VolumeNavigator( DFVolumeCollider collider)
        {
            this.collider = collider;
            volume = collider.gameObject.GetComponent<DFVolumeFilter>().volume;
            accessor = volume.Accessor;

            isoValue = volume.Accessor.isoValue;
            MAX_X = volume.resolution.x - 1;
            MAX_Y = volume.resolution.y - 1;
            MAX_Z = volume.resolution.z - 1;

            transform = collider.transform;
            gridToObjectOffset = -volume.objectToGridOffset;
            localToWorldMatrix = transform.localToWorldMatrix;

            _volumePath = new VolumePath(gridToObjectOffset, localToWorldMatrix);
        }

        public abstract void Update(Vector3 pos);

        public abstract void MoveTo(Vector3 toPointW);
    }

    // ======================================================================
    // ======================================================================

    public class VolumeNavigator_AStar : VolumeNavigator
    {
        PathFinder pathFinder;
        //ShortestPathGraphSearch pathFinder;
        Vector3 currentPosW;
        Vector3 lastWaypointW;
        Vector3 lastPosW;

        public VolumeNavigator_AStar(DFVolumeCollider collider) : base(collider)
        {
             pathFinder = new PathFinder(volume);
           // pathFinder = new ShortestPathGraphSearch(volume);
            pathFinder.OnEnd += PathFinder_OnEnd;
        }


        // =================

        public override void Update(Vector3 posW)
        {
            Debug.Log("Update "+ posW);

            lastPosW = currentPosW;
            currentPosW = posW;

            // ho raggiunto il nodo ?? 
            while (Count >0)
            {
                Vector3 wayPoint = nextPoint.worldPosition;
                // Vector3 n = (wayPoint - lastWaypointW).normalized;
                Vector3 n = (currentPosW - lastPosW).normalized;

                Plane p = new Plane(wayPoint, n);
                if (p.PointSide(currentPosW) > 0)
                {
                    lastWaypointW = wayPoint;
                    volumePath.Pop();
                    Debug.Log("POP ");
                }
               // else
                {
                    break;
                }
            }
        }

        // multithread
        private void PathFinder_OnEnd(PathFinder pathFinder)
        {
            VolumePathPoint p;
            volumePath.Clear();
            foreach (Vector3i point in pathFinder.path)
            {
                //p = new VolumePathPoint();
                //p.set(new Vector3(point.x, point.y, point.z), gridToObjectOffset, localToWorldMatrix);
                 p = volumePath.Append(new Vector3(point.x, point.y, point.z));
            }

            if (volumePath.pointList.Count>0)
                lastWaypointW = volumePath.pointList[0].worldPosition;
            volumePath.Pop(); // toilgo il primo
            // metto il target
   
        }

        public override void MoveTo(Vector3 toPoint)
        {
            targetPos = toPoint;
            Vector3 _fromPointObj = transform.worldToLocalMatrix.MultiplyPoint(currentPosW);
            Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);

            Vector3i _fromPoint = new Vector3i(_fromPointObj) + volume.objectToGridOffset;
            Vector3i _toPoint = new Vector3i(_toPointObj) + volume.objectToGridOffset;
            //  Vector3i gridToObjectOffset = -volume.objectToGridOffset;

            pathFinder.Start(new Vector3i(_fromPoint),new Vector3i(_toPoint));
         //   VolumePath path = new VolumePath();
         //List<Vector3i > list = new ShortestPathGraphSearch(volume).Start(new Vector3i(_fromPoint), new Vector3i(_toPoint));
         // volumePath.Clear();
         // foreach (Vector3i point in list)
         // {
         //     volumePath.Append(new Vector3(point.x, point.y, point.z));
         // }
         //VolumePathPoint p;
         //foreach (Vector3i point in pathFinder.path)
         //{
         //    p = new VolumePathPoint();
         //    p.set(point, gridToObjectOffset,transform.localToWorldMatrix);
         //    path.pointList.Add(p);
         //}
         // return path;
        }


    }
}