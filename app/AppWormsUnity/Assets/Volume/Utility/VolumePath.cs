using UnityEngine;
using System.Collections.Generic;

namespace Picodex
{
    public struct VolumePathPoint
    {
        public Matrix4x4 localToWorldTrx;

        public Vector3 gridP;
        public Vector3 localPosition;
        //public Vector3 volumePosition;

        public Vector3 worldPosition
        {
            get
            {
                return localToWorldTrx.MultiplyPoint(localPosition);
            }
        }

        //public VolumePathPoint set(Vector3 gridP,Vector3i gridToObjectOffset, Matrix4x4 localToWorldTrx)
        //{
        //    this.gridP = gridP;
        //    this.localToWorldTrx = localToWorldTrx;
        //    localPosition.x = gridP.x + gridToObjectOffset.x;
        //    localPosition.y = gridP.y + gridToObjectOffset.y;
        //    localPosition.z = gridP.z + gridToObjectOffset.z;
        //    return this;
        //}
    }

    // =======================================================

    public class VolumePath
    {
        public List<VolumePathPoint> pointList = new List<VolumePathPoint>();
        Vector3i gridToObjectOffset;
        Matrix4x4 localToWorldTrx;

        public VolumePath(Vector3i gridToObjectOffset, Matrix4x4 localToWorldTrx)
        {
            this.gridToObjectOffset = gridToObjectOffset;
            this.localToWorldTrx = localToWorldTrx;
        }

        public void Clear()
        {
            pointList.Clear();
        }

        public VolumePathPoint Append(Vector3 pointGrid)
        {
            VolumePathPoint p = new VolumePathPoint();

            p.gridP = pointGrid;
            p.localToWorldTrx = localToWorldTrx;
            p.localPosition.x = pointGrid.x + gridToObjectOffset.x;
            p.localPosition.y = pointGrid.y + gridToObjectOffset.y;
            p.localPosition.z = pointGrid.z + gridToObjectOffset.z;
            pointList.Add(p);
            return p;
        }

    }
}