
using System;
using UnityEngine;


namespace Picodex
{
    // MESH MANUAL
    [AddComponentMenu("Vxcm/VXCMTexture3D_Off_v03")]
    //[ExecuteInEditMode]
    public class VXCMTexture3D_Off_v03 : MonoBehaviour
    {
        private bool s_InsideRendering = false;

  
        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

     //   private new Renderer renderer;
        private Material material;
        private Mesh mesh;

        public Texture3D texture;
        // public int size = 16;

        private Matrix4x4 objectToVolumeTrx;

        public RenderTexture renderTexture = null;
   
        void Awake()
        {
          //  gameObject.layer = VXCMLayer;
        }

        void Start()
        {
            //  VXCMContext.Instance.useContext();

          //  renderer = GetComponent<Renderer>();
            material = GetComponent<Renderer>().material;
            mesh = GetComponent<MeshFilter>().sharedMesh;

            // ----

            objectToVolumeTrx = new Matrix4x4();

            Bounds bounds = mesh.bounds;

            Vector3 scale = new Vector3(1.0f / bounds.size.x, 1.0f / bounds.size.y, 1.0f / bounds.size.z);
            objectToVolumeTrx.SetTRS(-bounds.min, Quaternion.identity, scale);

            //volume = DFUtil.getVolume();
            texture = DFUtil.getTexture(64, 64, 64);

            // volume = VXCMContext.Instance.CreateVolumeFromTexture(texture);
        }

        void CleanUpTextures()
        {
            if (renderTexture)
            {
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = null;
            }
        }

        // This is called when it's known that the object will be rendered by some
        // camera. We render reflections and do other updates here.
        // Because the script executes in edit mode, reflections for the scene view
        // camera will just work!
        public void OnWillRenderObject()
        {
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;

            Camera cam = Camera.current;
            if (!cam) return;

            // Safeguard from recursive reflections.        
            if (s_InsideRendering)
                return;
            s_InsideRendering = true;

            CleanUpTextures();

            if (renderTexture == null)
            {
                renderTexture = RenderTexture.GetTemporary((int)Camera.current.pixelRect.width, (int)Camera.current.pixelRect.height, 16, RenderTextureFormat.ARGB32);
                renderTexture.name = "VXCMBuffer";
                renderTexture.wrapMode = TextureWrapMode.Clamp;
                renderTexture.filterMode = FilterMode.Point;
            }
                


                //    renderTexture.DiscardContents(true, true); // clear depth, keep color buffer contents
            RenderTexture currentActiveRT = RenderTexture.active; // store current active rt
            Graphics.SetRenderTarget(renderTexture); // set to target rt

            GL.PushMatrix();
            GL.LoadProjectionMatrix(cam.projectionMatrix);
                                                    //   GL.LoadPixelMatrix();
                                                   //   GL.Viewport(new Rect(0, 0,1,1));

            Matrix4x4 trx = GetComponent<MeshFilter>().transform.localToWorldMatrix;

            GL.modelview = cam.worldToCameraMatrix;// *  ;


            GL.Clear(true, true, Color.yellow);

            
            // Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);

            UpdateMat();

             if (material.SetPass(0))
            {
                //  GL.LoadPixelMatrix();
                //GL.LoadIdentity();
                //GL.MultMatrix(this.transform.localToWorldMatrix);
                //GL.MultMatrix(cam.worldToCameraMatrix);
                

                Graphics.DrawMeshNow(mesh, trx);

                      //screenTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                      //screenTexture.Apply(false);

                  }

            GL.PopMatrix();
            //Graphics.Blit(src, dest, mat);

            if (currentActiveRT != null) RenderTexture.active = currentActiveRT;

             
                //RenderTexture.ReleaseTemporary(lowresDepthRT);


   
            s_InsideRendering = false;

        }

        void UpdateMat()
        {
            if (Camera.current == null) return;

            //if (renderTexture == null)
            //    renderTexture = new RenderTexture((int)Camera.current.pixelRect.width, (int)Camera.current.pixelRect.height, 0);

            //volume.LoadTexture(VXCMVolumeLayerType.VXCMVolumeLayerType_DistanceField);


            material.SetTexture("_Volume", texture);

            material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);

            // current camera info
            float fYfovRad = Camera.current.fieldOfView * Mathf.Deg2Rad;
            float tanFov_2 = Mathf.Tan(fYfovRad / 2.0f);
            float h2 = Camera.current.nearClipPlane * tanFov_2;
            float w2 = Camera.current.aspect * h2;

            material.SetVector("u_cameraInfo", new Vector3(w2, h2, Camera.current.nearClipPlane));

            // azzero la traslazione della camera, prendo solo la rotazione
            Matrix4x4 camRot = Camera.current.transform.localToWorldMatrix;
            MathUtility.SetMatrixTranslation(ref camRot, new Vector3(0, 0, 0));
            material.SetMatrix("u_vxcm_worldViewMatrix", camRot);

            material.SetFloat("u_sample_rate", SampleRate);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
            material.SetInt("u_debug_layer", DebugLayer);

            //       // DEPTH BUFFER

            //       if (true)
            //       {
            //           renderTexture.DiscardContents(true, true); // clear depth, keep color buffer contents
            //           RenderTexture currentActiveRT = RenderTexture.active; // store current active rt
            //           Graphics.SetRenderTarget(renderTexture); // set to target rt

            //           if (material.SetPass(0))
            //           {
            //               Matrix4x4 mat = Camera.current.projectionMatrix * Camera.current.cameraToWorldMatrix;


            //               Graphics.DrawMeshNow(mesh, mat);

            //               //screenTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            //               //screenTexture.Apply(false);

            //           }

            //           Camera.current.Render();

            //           //Graphics.Blit(src, dest, mat);

            //           if (currentActiveRT != null) RenderTexture.active = currentActiveRT;

            //           //GetComponent<Renderer>().enabled = false;
            //           //RenderTextureFormat formatRF32 = RenderTextureFormat.RFloat;
            //           //int lowresDepthWidth = source.width / 2;
            //           //int lowresDepthHeight = source.height / 2;

            //           //RenderTexture lowresDepthRT = RenderTexture.GetTemporary(lowresDepthWidth, lowresDepthHeight, 0, formatRF32);
            //           // downscale depth buffer to quarter resolution
            ////Graphics.Blit(source, lowresDepthRT, DownscaleDepthMaterial);



            ////RenderTexture.ReleaseTemporary(lowresDepthRT);


            //       }
            if (false)
            {
                Camera.current.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                Camera.current.Render();

                //screenTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                //screenTexture.Apply(false);

                RenderTexture.active = null;
                Camera.current.targetTexture = null;
            }
            //Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
            //renderBuffer.

        }



    }

}