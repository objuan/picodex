using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace Picodex.Vxcm
{
    [System.Serializable]
    public class VXCMVolume
    {
        int size;
        public int subSampling;

        public float DistanceFieldRangeMin;
        public float DistanceFieldRangeMax;

        //public byte[] DF;
        [HideInInspector]
        public Color32[] DF;

        [HideInInspector]
        public VolumeAddress localToVolumeTrx;
        [HideInInspector]
        public VolumeRegion region;
        [HideInInspector]
        public Matrix4x4 WorldTrx;

        private VXCMVolumeAccessor accessor;

        public VXCMVolumeAccessor Accessor
        {
            get { return accessor; }
        }

        public VXCMVolume(VolumeRegion region, int subSampling, float distanceFieldRangeMin, float distanceFieldRangeMax)
        {
            DistanceFieldRangeMin = distanceFieldRangeMin;
            DistanceFieldRangeMax = distanceFieldRangeMax;
            accessor = new VXCMVolumeAccessor(this);

            this.region = region;
            size = region.size.x * region.size.y * region.size.z;
       
            DF = new Color32[size];

          //  MemoryUtil.MemSetG<byte>(DF, 0);

            localToVolumeTrx = - region.min;

        }

        public void Resize(VolumeAddress size)
        {
            VolumeAddress s = size * 0.5f;
            this.region = new VolumeRegion(-s.x, -s.y, -s.z, s.x, s.y, s.z);
            this.size = region.size.x * region.size.y * region.size.z;

            DF = new Color32[this.size];

            //  MemoryUtil.MemSetG<byte>(DF, 0);

            localToVolumeTrx = -region.min;

        }


    }

   
}
