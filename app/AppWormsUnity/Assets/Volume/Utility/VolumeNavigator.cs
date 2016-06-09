using UnityEngine;
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
        Vector3 currentPosW;
        Vector3 lastPosW;

        public VolumeNavigator_AStar(DFVolumeCollider collider) : base(collider)
        {
            pathFinder = new PathFinder(volume);
            pathFinder.OnEnd += PathFinder_OnEnd;
        }


        // =================

        public override void Update(Vector3 posW)
        {
            lastPosW = currentPosW;
            currentPosW = posW;
            // ho raggiunto il nodo ?? 
            if (Count > 0)
            {
                Vector3 wayPoint = nextPoint.worldPosition;

                Vector3 a = currentPosW - wayPoint;
                Vector3 b = lastPosW - wayPoint;
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
        }

        public override void MoveTo(Vector3 toPoint)
        {
            targetPos = toPoint;
            Vector3 _fromPointObj = transform.worldToLocalMatrix.MultiplyPoint(currentPosW);
            Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);

            Vector3i _fromPoint = new Vector3i(_fromPointObj) + volume.objectToGridOffset;
            Vector3i _toPoint = new Vector3i(_toPointObj) + volume.objectToGridOffset;
            //  Vector3i gridToObjectOffset = -volume.objectToGridOffset;

         //   VolumePath path = new VolumePath();
            pathFinder.Start(new Vector3i(_fromPoint), new Vector3i(_toPoint));

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