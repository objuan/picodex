
using System;
using UnityEngine;


namespace Picodex
{
    // prima versione , usa lo shader 4
    // MESH MANUAL
    [AddComponentMenu("Vxcm/VXCMObject_v01")]
  //  [ExecuteInEditMode]
    public class VXCMObject_v01 : MonoBehaviour
    {
        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

     //   private new Renderer renderer;
        private Material material;
        private MeshFilter meshFilter;

        public Texture3D texture;
        // public int size = 16;

        private Matrix4x4 objectToVolumeTrx;
        private Matrix4x4 worldToVolumeTrx;

        void Awake()
        {
        }

        void Start()
        {
            //  VXCMContext.Instance.useContext();

         //   renderer = GetComponent<Renderer>();
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
          //  scale.Scale(new Vector3(0.5f, 0.5f, 0.5f));
            objectToVolumeTrx.SetTRS(-bounds.min, Quaternion.identity, scale);


            worldToVolumeTrx = transform.worldToLocalMatrix;
        //    worldToVolumeTrx = worldToVolumeTrx * 
            //Vector3 ss = new Vector3(1.0f / 64, 1.0f / 64, 1.0f / 64);
            //worldToVolumeTrx.SetTRS( transform.position, Quaternion.identity, ss);

            //volume = DFUtil.getVolume();
            texture = DFUtil.getTexture(64, 64, 64);

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

            Matrix4x4 _objectToVolumeTrx = objectToVolumeTrx;
          //  _objectToVolumeTrx = _objectToVolumeTrx * Matrix4x4.TRS(Vector3.zero, this.transform.rotation, Vector3.one).inverse;
            material.SetMatrix("u_objectToVolumeTrx", _objectToVolumeTrx);
            material.SetMatrix("u_objectToVolumeInvTrx", _objectToVolumeTrx.inverse);

            worldToVolumeTrx = transform.worldToLocalMatrix;// * objectToVolumeTrx;
            Matrix4x4 trx = new Matrix4x4();
         //   Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
            trx.SetTRS( new Vector3(0.5f,0.5f,0.5f), Quaternion.identity, Vector3.one);

            material.SetMatrix("u_worldToVolumeTrx", trx * worldToVolumeTrx );

            // current camera info
            float fYfovRad = Camera.current.fieldOfView * Mathf.Deg2Rad;
            float tanFov_2 = Mathf.Tan(fYfovRad / 2.0f);
            float h2 = Camera.current.nearClipPlane * tanFov_2;
            float w2 = Camera.current.aspect * h2;

            material.SetVector("u_cameraInfo", new Vector3(w2, h2, Camera.current.nearClipPlane));

            //float fYfovRad = Camera.current.fieldOfView * Mathf.Deg2Rad;
            //float tanFov_2 = Mathf.Tan(fYfovRad / 2.0f);
            //float h2 = Camera.current.nearClipPlane * tanFov_2;
            //float w2 = Camera.current.aspect * h2;

            Light[] lights = FindObjectsOfType(typeof(Light)) as Light[];
            foreach (Light light in lights)
            {
                // Vector3 localDir = Camera.current.transform.worldToLocalMatrix * -light.transform.forward;
                Vector3 localDir =  -light.transform.forward;
                //  localDir = new Vector3(-1, -1, 0).normalized;

                material.SetVector("u_cameraInfoLight", localDir);
            }
          
            Matrix4x4 camToLocal = Camera.current.transform.localToWorldMatrix;
            Vector3 camPosLoc = transform.worldToLocalMatrix * Camera.current.transform.position;

            material.SetMatrix("u_camToLocalTrx", camToLocal * Camera.current.projectionMatrix);
            material.SetVector("u_camPosTrx", camPosLoc);
            material.SetMatrix("u_prjInvTrx", Camera.current.projectionMatrix.inverse);

            // azzero la traslazione della camera, prendo solo la rotazione
            Matrix4x4 camRot = Camera.current.transform.localToWorldMatrix;
            MathUtility.SetMatrixTranslation(ref camRot, new Vector3(0, 0, 0));
            material.SetMatrix("u_vxcm_worldViewMatrix", camRot);
            
            material.SetFloat("u_textureRes", 1.0f / 64);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
        }



    }

}