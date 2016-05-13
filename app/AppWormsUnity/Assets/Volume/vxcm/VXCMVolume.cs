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

      //  [HideInInspector]
        public Vector3i localToVolumeTrx;
     //   [HideInInspector]
        public VolumeRegion region;
       // [HideInInspector]
        public Matrix4x4 WorldTrx;

       // [HideInInspector]
        public Vector3i resolution
        {
            get
            {
                return region.size;
            }
        }

        private VXCMVolumeAccessor accessor;

        public VXCMVolumeAccessor Accessor
        {
            get
            {
                if (accessor == null)
                {
                    accessor = new VXCMVolumeAccessor(this);
                }
                return accessor;
            }
        }

        public VXCMVolume(Vector3i size, int subSampling, float distanceFieldRangeMin, float distanceFieldRangeMax)
        {
            DistanceFieldRangeMin = distanceFieldRangeMin;
            DistanceFieldRangeMax = distanceFieldRangeMax;
            Resize(size);
        }

        //public VXCMVolume(VolumeRegion region, int subSampling, float distanceFieldRangeMin, float distanceFieldRangeMax)
        //{
        //    DistanceFieldRangeMin = distanceFieldRangeMin;
        //    DistanceFieldRangeMax = distanceFieldRangeMax;
        //    this.region = region;
        //    size = region.size.x * region.size.y * region.size.z;
        //    DF = new Color32[size];

        //  //  MemoryUtil.MemSetG<byte>(DF, 0);

        //    localToVolumeTrx = - region.min;
        //}

        public void Resize(Vector3i size)
        {
            Vector3i s = size * 0.5f;
            this.region = new VolumeRegion(-s.x, -s.y, -s.z, s.x, s.y, s.z);
            this.size = region.size.x * region.size.y * region.size.z;

            DF = new Color32[this.size];

            //  MemoryUtil.MemSetG<byte>(DF, 0);

            localToVolumeTrx = -region.min;

            accessor = null; // invalidate

        }


    

        // ================

        public Texture3D CreateTexture()
        {
            Texture3D texture = new Texture3D(resolution.x, resolution.y, resolution.z, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.SetPixels32(DF);
            texture.Apply();
            return texture;
        }

        public static Vector3i GetTextureInfo(Vector3 volumeSize, float samplingRate)
        {
            int chunkSize = 1;
            Vector3i info = new Vector3i();
            info = new Vector3i(1 << (1 + (int)Math.Log(volumeSize.x * samplingRate - 0.5, 2)),
                1 << (1 + (int)Math.Log(volumeSize.y * samplingRate - 0.5, 2)),
                1 << (1 + (int)Math.Log(volumeSize.z * samplingRate - 0.5, 2)));
            //info.x = Math.Max(info.x, chunkSize * 2);
            //info.y = Math.Max(info.y, chunkSize * 2);
            //info.z = Math.Max(info.z, chunkSize * 2);

            return info;
        }

        public void ImportMesh(Mesh mesh, Transform trx, VXCMVolumeDef objheader)
        {
            Matrix4x4 meshToLocalTrx = Matrix4x4.TRS(-mesh.bounds.center, Quaternion.identity, trx.lossyScale);
            Matrix4x4 localToWorldTrx = trx.localToWorldMatrix * meshToLocalTrx.inverse;

            ImportMesh(mesh, meshToLocalTrx, objheader);

          //  SetLocalToWorldTrx(localToWorldTrx);
        }

        public void ImportMesh(Mesh mesh, Matrix4x4 meshToLocalTrx, VXCMVolumeDef objheader)
        {
            // ass sampling rate
            Matrix4x4 samplingRateTrx = Matrix4x4.Scale(new Vector3(objheader.samplingRate, objheader.samplingRate, objheader.samplingRate));
            meshToLocalTrx = samplingRateTrx * meshToLocalTrx;

            //volumeNative.ImportMesh(mesh, meshToLocalTrx, objheader);
            //UpdateHeader();
        }

    }


}
