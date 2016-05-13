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
        public VolumeAddress min ;
        public VolumeAddress max;

        public VolumeAddress size
        {
            get { return max - min; }
        }

        /// Constructs a Region from corners specified as seperate integer parameters.
        public VolumeRegion(int lowerX, int lowerY, int lowerZ, int upperX, int upperY, int upperZ)
        {
            min = new VolumeAddress(lowerX, lowerY, lowerZ);
            max = new VolumeAddress(upperX, upperY, upperZ);
        }

        public VolumeRegion(VolumeAddress min, VolumeAddress max)
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
