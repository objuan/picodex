using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Picodex
{
  
    public class VolumeNavigator
    {
        DFVolume volume;
        Vxcm.VXCMVolumeAccessor accessor;
        DFVolumeCollider collider;
        byte isoValue;
        int MAX_X, MAX_Y, MAX_Z;

        Transform transform;
        Vector3i gridToObjectOffset;
        Matrix4x4 localToWorldMatrix;
        PathFinder pathFinder;

        public VolumePath volumePath;

        // runtime, in grid position
        Vector3 currentPos;
        Vector3 targetPos;

        Vector3 lastPos;

        public VolumeNavigator(DFVolumeCollider collider)
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

            pathFinder = new PathFinder(volume);
            pathFinder.OnEnd += PathFinder_OnEnd;

            volumePath = new VolumePath();
        }

        static Vector3[] nearOffset = new Vector3[] { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

        public void Update(Vector3 pos)
        {
            // to grid coordinate
            Vector3 _currentPos = transform.worldToLocalMatrix.MultiplyPoint(pos);
            currentPos = _currentPos + volume.objectToGridOffsetReal;

            Vector3i i_currentPos = new Vector3i(currentPos );
            Vector3i i_targetPos = new Vector3i(targetPos);

            Vector3 point;
            Vector3i i_point;
            float d;
            float dist = PathFinder.Distance(i_currentPos, i_targetPos);
            Vector3 fwDirection = (currentPos - lastPos).normalized;

            float distanceW = 1;
            float dirW = 1;

            float bestW = 99999;
            Vector3 bestPoint=Vector3.zero;
            if (dist > 0)
            {
                for(int i=0;i< nearOffset.Length; i++)
                {
                    point = currentPos + nearOffset[i];
                    i_point = new Vector3i(point);
                    if (IsWalkable(i_point))
                    {
                        d = PathFinder.Distance(i_point, i_targetPos);
                        // dir
                        Vector3 dir = (point - lastPos).normalized;
                        float dot = 1.0f - Vector3.Dot(dir, fwDirection);
                        float w = d * distanceW + dot * dirW;
                        if (w < bestW)
                        {
                            bestW = w ;
                            bestPoint = point;
                        }
                    }
                }
            }

            // OUT
            volumePath.Clear();
            if (bestW != 0)
            {
                VolumePathPoint  p = new VolumePathPoint();
                p.set(bestPoint, gridToObjectOffset, localToWorldMatrix);
                volumePath.pointList.Add(p);
            }
            lastPos = currentPos;
        }

        public void MoveTo( Vector3 toPoint)
        {
            // to grid coordinate
            Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);
            targetPos = _toPointObj + volume.objectToGridOffsetReal;

            //Vector3i point;
            //volumePath.Clear();
            //VolumePathPoint p;
            //for (var i = 0; i < nearOffset.Length; i++)
            //{
            //    point = new Vector3i(fromPoint + nearOffset[i]);

            //    byte val = volume.Accessor.GetDistanceField(point);
            //    return val > 0 && val <= isoValue;//TODO

            //    // 
            //    p = new VolumePathPoint();
            //    p.set(point, gridToObjectOffset, localToWorldMatrix);
            //    volumePath.pointList.Add(p);
            //}
            //volume.Accessor.GetDistanceField();
        }

        public bool IsWalkable(Vector3i pos)
        {
            // fuori bound ?? 
            //int sum = pos.x + pos.y + pos.z;
            //if (sum == 0 || sum == MAX_SUM) return false;

            // get value
            byte val = accessor.GetDistanceField(pos);
            return val > 0 && val <= isoValue;//TODO
        }

        // =================


        // multithread
        private void PathFinder_OnEnd(PathFinder pathFinder)
        {
            VolumePathPoint p;
            volumePath.Clear();
            foreach (Vector3i point in pathFinder.path)
            {
                p = new VolumePathPoint();
                p.set(new Vector3(point.x, point.y, point.z), gridToObjectOffset, localToWorldMatrix);
                volumePath.pointList.Add(p);
            }
        }

        public void CreatePathW(Vector3 fromPoint, Vector3 toPoint)
        {
            Vector3 _fromPointObj = transform.worldToLocalMatrix.MultiplyPoint(fromPoint);
            Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);

            Vector3i _fromPoint = new Vector3i(_fromPointObj) + volume.objectToGridOffset;
            Vector3i _toPoint = new Vector3i(_toPointObj) + volume.objectToGridOffset;
            //  Vector3i gridToObjectOffset = -volume.objectToGridOffset;

            VolumePath path = new VolumePath();
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