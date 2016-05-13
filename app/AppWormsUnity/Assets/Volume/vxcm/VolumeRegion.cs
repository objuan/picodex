using System;
using System.Collections.Generic;

using System.Text;

//using picodex.worms.math;
using UnityEngine;

namespace Picodex.Vxcm
{
    [System.Serializable]
    public class VolumeRegion 
    {
        public Vector3i min ;
        public Vector3i max;

        public Vector3i size
        {
            get { return max - min; }
        }

        /// Constructs a Region from corners specified as seperate integer parameters.
        public VolumeRegion(int lowerX, int lowerY, int lowerZ, int upperX, int upperY, int upperZ)
        {
            min = new Vector3i(lowerX, lowerY, lowerZ);
            max = new Vector3i(upperX, upperY, upperZ);
        }

        public VolumeRegion(Vector3i min, Vector3i max)
        {
            this.min = min;
            this.max = max;
        }


        /// Returns a System.String that represents the current Cubiquity.Region.
        public override string ToString()
        {
            return string.Format("VolumeRegion({0}, {1}, {2}, {3}, {4}, {5})",
                min.x, min.y, min.z,
                max.x, max.y, max.z);
        }
    }
}
