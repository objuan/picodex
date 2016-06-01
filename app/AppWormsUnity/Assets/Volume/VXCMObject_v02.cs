
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

     //   public Vector3i dimensions = new Vector3i(64, 64, 64);

        private new Renderer renderer;
        private DFVolumeRenderer volumeRenderer;
        private Material material;
        private MeshFilter meshFilter;

        public Texture3D texture = null;
        public VXCMVolume volume;
      ///  DFVolumeEditor editor;

        private Matrix4x4 objectToVolumeTrx;

        private bool mustInitialize=true;

        protected void OnValidate()
        {
            if (!volume) return;
            //if (dimensions != volume.region.size)
            //{
            //    this.volume.Resize(dimensions);
            //}
        }

        void Awake()
        {
        }

        //public void OnEnable()
        //{
        //   // register the callback when enabling object
        //    Camera.onPreCull += PreCullAdjustFOV;
        //    Camera.onPreRender += PreRenderAdjustFOV;
        //}
        //public void OnDisable()
        //{
        // //   remove the callback when disabling object
        //    Camera.onPreCull -= PreCullAdjustFOV;
        //    Camera.onPreRender -= PreRenderAdjustFOV;
        //}

        void Start()
        {
          //  Debug.Log("Start");
            if (volume == null)
            {
                DFVolumeFilter volumeFilter = GetComponent<DFVolumeFilter>();
                if (volumeFilter != null && volumeFilter.volume != null)
                    volume = volumeFilter.volume;
            }

            mustInitialize = true;
            //  VXCMContext.Instance.useContext();

            meshFilter = GetComponent<MeshFilter>();
            renderer = GetComponent<Renderer>();
            volumeRenderer = transform.parent.gameObject.GetComponent<DFVolumeRenderer>();

            material = volumeRenderer.material;
            // material = GetComponent<Renderer>().sharedMaterial;
            // material = new Material(Shader.Find("Vxcm/Object/ray_v07"));
            //material = new Material(Shader.Find("Vxcm/Transparent"));

            //editor = this.GetComponentInParent<DFVolumeEditor>();

            renderer.material = material;

        }

        //public void PreRenderAdjustFOV(Camera cam)
        //{

        //   // if (cam == thisCamera)
        //    {
        //        Debug.Log("MyPreRender: " + cam.gameObject.name);
        //        cam.fieldOfView = 60;
        //    }
        //}

        //// callback to be called before any culling
        //public void PreCullAdjustFOV(Camera cam)
        //{
        //  //  if (cam == thisCamera)
        //    {
        //        Debug.Log("PreCull: " + cam.gameObject.name);
        //        cam.fieldOfView = 70;

        //        //These are needed for the FOV change to take effect.
        //        cam.ResetWorldToCameraMatrix();
        //        cam.ResetProjectionMatrix();
        //    }

        //}

        public void OnWillRenderObject()
        {
          //  Debug.Log("OnWillRenderObject");
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;
            
            Camera cam = Camera.current;
            if (!cam) return;

            UpdateMat();

            // notify editor

         //   DFVolumeEditor editor = this.GetComponentInParent<DFVolumeEditor>();

            //if (editor)
            //    editor.OnWillRenderObject(); // manual call
        }

//#if UNITY_EDITOR
//        void UpdateFixed()
//        {
//            if (editor)
//                editor.Update(); // manual call
//        }
//#endif
        public void UpdateMat()
        {
            if (!volume) return;
            if (!meshFilter) return;

          
            // Debug.Log("UpdateMat");
            if (texture == null
               // || (volume && volume.lastFrameChanged)
                || (texture != null && (texture.width != volume.resolution.x) || (texture.height != volume.resolution.y) || (texture.depth != volume.resolution.z)))
            {
                // resize Proxy
                volumeRenderer.proxyBuilder.Rebuild();
                //if (volumeRenderer.proxyType == DFVolumeRendererProxyType.Box)
                //    PrimitiveHelper.CreateCube(meshFilter.sharedMesh, volume.resolution.x, volume.resolution.y, volume.resolution.z);
                //else
                //    PrimitiveHelper.CreateSphere(meshFilter.sharedMesh, volume.resolution.x * 0.5f);

                // build texture
                texture = new Texture3D(volume.resolution.x, volume.resolution.y, volume.resolution.z, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Bilinear;
                texture.wrapMode = TextureWrapMode.Clamp;

             //   Debug.Log("Build Txt");
            }
            if (volumeRenderer.proxyBuilder != null)
                volumeRenderer.proxyBuilder.Update(transform, Camera.current);

            if (texture && volume && (volume.lastFrameChanged || mustInitialize))
            {
             //   volume.lastFrameChanged = false;
                mustInitialize = false;
                texture.SetPixels32(volume.DF);
                texture.Apply();


                objectToVolumeTrx = new Matrix4x4();

            //    Bounds bounds = meshFilter.sharedMesh.bounds;

                float m = 1.0f / 2;
                Vector4 scale = new Vector4(1.0f / volume.resolution.x, 1.0f / volume.resolution.y, 1.0f / volume.resolution.z,0);
                objectToVolumeTrx.SetTRS(new Vector3(m, m, m), Quaternion.identity, scale);

                // mat

                material.SetFloat("DF_MIN", volume.distanceFieldRangeMin);
                material.SetFloat("DF_MAX_MINUS_MIN", volume.distanceFieldRangeMax - volume.distanceFieldRangeMin);

                material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);
                material.SetMatrix("u_objectToVolumeInvTrx", objectToVolumeTrx.inverse);

                material.SetVector("u_textureRes", scale);

             //   Debug.Log("Build Mat");
            }


            material.SetTexture("_Volume", texture);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);

           
        }


        void OnRenderObject()
        {
            if (volume)
                volume.lastFrameChanged = false;
        }

        // ============================

    

    }

}