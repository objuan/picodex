using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace Picodex.Vxcm
{
    //[CustomPropertyDrawer(typeof(MyData))]
    //public class DataDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    //    {
    //        SerializedProperty myValue = prop.FindPropertyRelative("myValue");
    //        EditorGUI.Slider(new Rect(pos.x, pos.y, pos.width, pos.height), myValue, 0f, 1f);
    //    }
    //}

    // [System.Serializable]
    public class VXCMVolume : ScriptableObject
    {
        int size;

        [Range(1f, 4f)]
        public int subSampling=1;

        [Range(-2f, 0f)]
        public float distanceFieldRangeMin=-2;
        [Range(2f, 8f)]
        public float distanceFieldRangeMax=2;

        //public byte[] DF;
        [HideInInspector]
        public Color32[] DF;

       // [Range(16f, 128f)]
        public Vector3i resolution = new Vector3i(64,64,64);

        [HideInInspector]
        public Vector3i localToVolumeTrx;

     //   [HideInInspector]
        public VolumeRegion region
        {
            get
            {
                Vector3i s = resolution * 0.5f;
                return new VolumeRegion(-s.x, -s.y, -s.z, s.x, s.y, s.z);
            }
        }
        // [HideInInspector]
        // public Matrix4x4 WorldTrx;

        // [HideInInspector]
        //public Vector3i resolution
        //{
        //    get
        //    {
        //        return size;
        //    }
        //    set
        //    {
        //        if (value != resolution)
        //        {
        //            Resize(value);
        //        }
        //    }
        //}
        [HideInInspector]
        internal bool lastFrameChanged = true;

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

        public void initialize(Vector3i size, int subSampling, float distanceFieldRangeMin, float distanceFieldRangeMax)
        {
            this.subSampling = subSampling;
            this.distanceFieldRangeMin = distanceFieldRangeMin;
            this.distanceFieldRangeMax = distanceFieldRangeMax;
            Resize(size);
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        void OnDestroy()
        {

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

        public void Resize(Vector3i resolution)
        {
        //    Vector3i s = size * 0.5f;
        //    this.region = new VolumeRegion(-s.x, -s.y, -s.z, s.x, s.y, s.z);
            this.resolution = resolution;
            size = resolution.x * resolution.y * resolution.z;

            DF = new Color32[size];

            //  MemoryUtil.MemSetG<byte>(DF, 0);

            localToVolumeTrx = -region.min;

            accessor = null; // invalidate
            lastFrameChanged = true;
        }

        private void ensureInit()
        {
            int size = resolution.x * resolution.y * resolution.z;
            if (DF == null || DF.Length != size) Resize(resolution);
        }

        public void Clear()
        {
            ensureInit();
            MemoryUtil.MemSetG<Color32>(DF, new Color32(0,0,0,0));
            lastFrameChanged = true;
        }

        public void Invalidate()
        {
            lastFrameChanged = true;
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
