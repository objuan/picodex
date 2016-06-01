
using System;
using UnityEngine;


namespace Picodex
{
    // SOLO CAMERA
    [AddComponentMenu("Vxcm/VXCMTexture3D_Off_v02")]
    [ExecuteInEditMode]
    public class VXCMTexture3D_Off_v02 : MonoBehaviour
    {
        private bool s_InsideRendering = false;

        // VXCM layer
       // static readonly int VXCMLayer = 1024;

        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

     //   private new Renderer renderer;
        private Material material;
      //  private Mesh mesh;

        public Texture3D texture;
        // public int size = 16;

        private Matrix4x4 objectToVolumeTrx;

        
        public RenderTexture renderTexture = null;
        public GameObject renderCamera = null;

        void Awake()
        {
          //  gameObject.layer = VXCMLayer;
        }

        void Start()
        {
            //  VXCMContext.Instance.useContext();

         //   renderer = GetComponent<Renderer>();
            material = GetComponent<Renderer>().sharedMaterial;
          //  mesh = GetComponent<MeshFilter>().sharedMesh;

            // ----

            objectToVolumeTrx = new Matrix4x4();

            Bounds bounds = GetComponent<MeshFilter>().sharedMesh.bounds;

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
        // Because the script executes in edit mode
        public void OnWillRenderObject()
        {
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;

            Camera cam = Camera.current;
            if (!cam)  return;

            // Safeguard from recursive reflections.        
            if (s_InsideRendering)
                return;
            s_InsideRendering = true;

            Camera camera = null;
            if (true)
            {
                CleanUpTextures();

                renderTexture = RenderTexture.GetTemporary((int)Camera.current.pixelRect.width, (int)Camera.current.pixelRect.height, 16, RenderTextureFormat.ARGB32);
                renderTexture.name = "VXCMBuffer";
                renderTexture.wrapMode = TextureWrapMode.Clamp;
                renderTexture.filterMode = FilterMode.Point;
                // CAMERA SETUP
                if (!renderCamera)
                {
                    renderCamera = new GameObject("RenderCamera");
                    camera = renderCamera.AddComponent<Camera>();
                    camera.enabled = false;
                    renderCamera.hideFlags = HideFlags.HideAndDontSave;
                    //  camera.cullingMask = 1 << LayerMask.NameToLayer("Character1") | 1 << LayerMask.NameToLayer("Character2");
                    // camera.cullingMask = 1024;// 1 << LayerMask.NameToLayer("VXCM");

                }
                else
                    camera = renderCamera.GetComponent<Camera>();

                camera.CopyFrom(cam);
                camera.cullingMask = 1 << LayerMask.NameToLayer("VXCM");
                camera.backgroundColor = new Color(0, 0, 0, 0);
                camera.clearFlags = CameraClearFlags.SolidColor;
                

            }

            // OK
            if (true)
            {
                UpdateMat();

                camera.targetTexture = renderTexture;
                // camera.RenderWithShader(Shader.Find("Hidden/Camera-DepthNormalTexture"), null);
                camera.Render();

                //  camera.depthTextureMode |= DepthTextureMode.DepthNormals;
            }

          
            s_InsideRendering = false;

        }

        void UpdateMat()
        {
            if (Camera.current == null) return;

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

   

        }



    }

}