using UnityEngine;
using System.Collections.Generic;

namespace Picodex
{
    public struct VolumePathPoint
    {
        Matrix4x4 localToWorldTrx;

        Vector3 gridP;
        public Vector3 localPosition;
        //public Vector3 volumePosition;

        public Vector3 worldPosition
        {
            get
            {
                return localToWorldTrx.MultiplyPoint(localPosition);
            }
        }
        public VolumePathPoint set(Vector3 gridP,Vector3i gridToObjectOffset, Matrix4x4 localToWorldTrx)
        {
            this.gridP = gridP;
            this.localToWorldTrx = localToWorldTrx;
            localPosition.x = gridP.x + gridToObjectOffset.x;
            localPosition.y = gridP.y + gridToObjectOffset.y;
            localPosition.z = gridP.z + gridToObjectOffset.z;
            return this;
        }
    }

    public class VolumePath
    {
        public List<VolumePathPoint> pointList = new List<VolumePathPoint>();

        public VolumePath()
        {
        }

        public void Clear()
        {
            pointList.Clear();
        }
    }
}