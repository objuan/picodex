
using System;
using UnityEngine;

using Picodex.Math;
using Picodex.Unity;


namespace Picodex
{
    // MESH MANUAL
    [AddComponentMenu("Vxcm/VXCMObject_v01")]
    [ExecuteInEditMode]
    public class VXCMObject_v01 : MonoBehaviour
    {
        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

        private new Renderer renderer;
        private Material material;
        private MeshFilter meshFilter;

        public Texture3D texture;
        // public int size = 16;

        private Matrix4x4 objectToVolumeTrx;

        void Awake()
        {
        }

        void Start()
        {
            //  VXCMContext.Instance.useContext();

            renderer = GetComponent<Renderer>();
            material = GetComponent<Renderer>().sharedMaterial;
            meshFilter = GetComponent<MeshFilter>();

          //  material.Set
            //Shader.SetGlobalFloat("DF_MIN", -2);
            //Shader.SetGlobalFloat("DF_MAX_MINUS_MIN", 2 - (-2));

            material.SetFloat("DF_MIN", -2);
            material.SetFloat("DF_MAX_MINUS_MIN", 2 - (-2));

         
            // material.EnableKeyword("DISTANCE_FIELD_MIN=-2");
            //  material.EnableKeyword("DISTANCE_FIELD_MAX=2");

            // ----

            objectToVolumeTrx = new Matrix4x4();

            Bounds bounds = meshFilter.sharedMesh.bounds;

            Vector3 scale = new Vector3(1.0f / bounds.size.x, 1.0f / bounds.size.y, 1.0f / bounds.size.z);
            objectToVolumeTrx.SetTRS(-bounds.min, Quaternion.identity, scale);

            //volume = DFUtil.getVolume();
            texture = DFUtil.getTexture();

            // volume = VXCMContext.Instance.CreateVolumeFromTexture(texture);
        }

        public void OnWillRenderObject()
        {
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;
            
            Camera cam = Camera.current;
            if (!cam) return;

            UpdateMat();
            /*
            // renderizzo sul buffer della camera

            RenderTexture renderTexture = cam.gameObject.GetOrAddComponent<VXCMCamera>().renderTexture;
            if (renderTexture == null) return;

            RenderTexture currentActiveRT = RenderTexture.active; // store current active rt
            Graphics.SetRenderTarget(renderTexture); // set to target rt

            GL.PushMatrix();
            GL.LoadProjectionMatrix(cam.projectionMatrix);
            GL.modelview = cam.worldToCameraMatrix;

            if (material.SetPass(0))
            {
                Graphics.DrawMeshNow(meshFilter.sharedMesh, meshFilter.transform.localToWorldMatrix);

                if (currentActiveRT != null) RenderTexture.active = currentActiveRT;
            }
            GL.PopMatrix();
            */
        }

        void UpdateMat()
        {
            if (Camera.current == null) return;

            material.SetTexture("_Volume", texture);

            material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);
            material.SetMatrix("u_objectToVolumeInvTrx", objectToVolumeTrx.inverse);

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
            
            material.SetFloat("u_textureRes", 1.0f / 64);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
  
        

        }



    }

}