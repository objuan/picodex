using UnityEngine;
using System.Collections.Generic;
using System.Threading;

namespace Picodex
{
  
    public class VolumeNavigator_Incremental : VolumeNavigator
    {
        // runtime, in grid position
        Vector3 currentPos;
        Vector3 lastPos;
        Vector3i i_targetPos;

        static Vector3[] nearOffset = new Vector3[] { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

        public VolumeNavigator_Incremental(DFVolumeCollider collider) : base(collider)
        {
           
        }

       public override void Update(Vector3 pos)
        {
            // to grid coordinate
            Vector3 _currentPos = transform.worldToLocalMatrix.MultiplyPoint(pos);
            currentPos = _currentPos + volume.objectToGridOffsetReal;

            Vector3i i_currentPos = new Vector3i(currentPos);
           
            Vector3 point;
            Vector3i i_point;
            float d;
            float dist = PathFinder.Distance(i_currentPos, i_targetPos);
            Vector3 fwDirection = (currentPos - lastPos).normalized;

            float distanceW = 1;
            float dirW = 2;

            int idx = -1;
            float bestW = 99999;
            Vector3 bestPoint = Vector3.zero;
            if (dist > 0)
            {
                for (int i = 0; i < nearOffset.Length; i++)
                {
                    point = currentPos + nearOffset[i];
                    i_point = new Vector3i(point);

                    if (IsWalkable(i_point))
                    {
                        //    d = PathFinder.Distance(point, targetPos);
                        d = PathFinder.Distance(i_point, i_targetPos);
                        // d = (point - targetPos).magnitude;
                        // dir
                        Vector3 dir = (point - lastPos).normalized;
                        float dot = 1.0f - Vector3.Dot(dir, fwDirection);
                        float w = d * distanceW + dot * dirW;
                        if (w < bestW)
                        {
                            bestW = w;
                            bestPoint = point;
                            idx = i;
                        }
                    }
                }
            }

            // OUT
            volumePath.Clear();
            if (bestW != 99999)
            {
                Vector3 dir = (bestPoint - lastPos).normalized;
                float dot = 1.0f - Vector3.Dot(dir, fwDirection);

                Debug.Log("pos " + pos + " dir " + dir + " dot " + dot + " idx " + idx);

                volumePath.Append(bestPoint);
            }
            lastPos = currentPos;
        }

        public override void MoveTo(Vector3 toPointW)
        {
            targetPos = toPointW;

            // to grid coordinate
            Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPointW);
            i_targetPos = new Vector3i(_toPointObj + volume.objectToGridOffsetReal);


            //    _targetPos.set(_toPointObj + volume.objectToGridOffsetReal, gridToObjectOffset, localToWorldMatrix);
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
    }
    

}