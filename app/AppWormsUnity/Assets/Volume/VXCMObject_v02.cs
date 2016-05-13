
using System;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    // seconda versione , usa lo shader 5 e il volume. la scala e' uo
    [AddComponentMenu("Vxcm/VXCMObject_v02")]
    [ExecuteInEditMode]
    public class VXCMObject_v02 : MonoBehaviour
    {
        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        public Vector3i dimensions = new Vector3i(64, 64, 64);

        private new Renderer renderer;
        private Material material;
        private MeshFilter meshFilter;

        public Texture3D texture = null;
        public VXCMVolume volume;
    
        private Matrix4x4 objectToVolumeTrx;

        protected void OnValidate()
        {
            if (dimensions != volume.region.size)
            {
                this.volume.Resize(dimensions);
            }
        }

        void Awake()
        {
        }

        void Start()
        {
            if (volume == null)
            {
                DFVolumeFilter volumeFilter = GetComponent<DFVolumeFilter>();
                if (volumeFilter != null && volumeFilter.volumeData != null)
                    volume = volumeFilter.volumeData.volume;
                else
                    volume = new VXCMVolume(dimensions, 1, -2, 2);
            }

            //  VXCMContext.Instance.useContext();

            meshFilter = GetComponent<MeshFilter>();
            renderer = GetComponent<Renderer>();
            // material = GetComponent<Renderer>().sharedMaterial;
            material = new Material(Shader.Find("Vxcm/Object/ray_v05"));
            renderer.material = material;
            //meshFilter.mesh = new Mesh();
            PrimitiveHelper.CreateCube(meshFilter.sharedMesh, dimensions.x, dimensions.y, dimensions.z);

       
        }

        public void OnWillRenderObject()
        {
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;
            
            Camera cam = Camera.current;
            if (!cam) return;

            UpdateTexture();

            UpdateMat();
        }

        public void UpdateTexture()
        {
            if (texture == null || (texture != null && (texture.width != volume.resolution.x) || (texture.height != volume.resolution.y) || (texture.depth != volume.resolution.z)))
            {
                if (false)
                {
                    VolumePrimitiveSphere raster = new VolumePrimitiveSphere(volume);

                    GeometrySample sample = new GeometrySample();
                    sample.debugColor = new Vector3(1, 0, 0);

                    raster.Raster(new Vector3(-10, 0, 0), 10, sample);
                    raster.Raster(new Vector3(10, 0, 0), 10, sample);
                }

                PrimitiveHelper.CreateCube(meshFilter.sharedMesh, dimensions.x, dimensions.y, dimensions.z);

                texture = new Texture3D(volume.resolution.x, volume.resolution.y, volume.resolution.z, TextureFormat.RGBA32, false);
                texture.SetPixels32(volume.DF);
                texture.Apply();

                objectToVolumeTrx = new Matrix4x4();

                Bounds bounds = meshFilter.sharedMesh.bounds;

                float m = 1.0f / 2;
                Vector3 scale = new Vector3(1.0f / volume.resolution.x, 1.0f / volume.resolution.y, 1.0f / volume.resolution.z);
                objectToVolumeTrx.SetTRS(new Vector3(m,m,m), Quaternion.identity, scale);

                // mat

                material.SetFloat("DF_MIN", volume.DistanceFieldRangeMin);
                material.SetFloat("DF_MAX_MINUS_MIN", volume.DistanceFieldRangeMax - volume.DistanceFieldRangeMin);

                material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);
                material.SetMatrix("u_objectToVolumeInvTrx", objectToVolumeTrx.inverse);

                material.SetVector("u_textureRes", scale);


            }
        }

        void UpdateMat()
        {
            if (Camera.current == null) return;
            if (material == null) return;

            material.SetTexture("_Volume", texture);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
        }



    }

}