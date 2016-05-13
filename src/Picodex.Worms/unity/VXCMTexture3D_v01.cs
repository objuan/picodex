//using Assets.VXCM.Scripts.core;
//using Assets.VXCM.Scripts.native;
//using Assets.VXCM.Scripts.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Picodex.Math;

namespace picodex.worms
{
    [AddComponentMenu("Vxcm/VXCMTexture3D_v01")]
    //[ExecuteInEditMode]
    public class VXCMTexture3D_v01 : MonoBehaviour
    {
        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

        private Material material;
        private Mesh mesh;

        public Texture3D texture;
       // public int size = 16;

        private Matrix4x4 objectToVolumeTrx;

     //   private VXCMVolume volume =null;

        void Awake()
        {
#if !UNITY_EDITOR // senza queso trocco non e' possibile usare ExecuteInEditMode e leggere mesh e material
        //Only do this in the editor
        MeshFilter mf = GetComponent<MeshFilter>();   //a better way of getting the meshfilter using Generics
        Mesh meshCopy = Mesh.Instantiate(mf.sharedMesh) as Mesh;  //make a deep copy
        mesh = mf.mesh = meshCopy;

       // Renderer re = GetComponent<Renderer>();   //a better way of getting the meshfilter using Generics
     //   Material matCopy = Material.Instantiate(re.sharedMaterial) as Material;  //make a deep copy
      //  material = re.material = matCopy;

#else

#endif
        }

        void Start()
        {
          //  VXCMContext.Instance.useContext();

            Renderer re = GetComponent<Renderer>();
            re.material = new Material(Shader.Find("Vxcm/Texture3D/slice_v01"));

            material = GetComponent<Renderer>().material;
            mesh = GetComponent<MeshFilter>().mesh;

          //  texture = GetTextureUVW(size);

            objectToVolumeTrx = new Matrix4x4();

            Bounds bounds = mesh.bounds;

            Vector3 scale = new Vector3(1.0f / bounds.size.x, 1.0f / bounds.size.y, 1.0f / bounds.size.z);
            objectToVolumeTrx.SetTRS(-bounds.min, Quaternion.identity, scale);

            //volume = DFUtil.getVolume();
            texture = DFUtil.getTexture();

            // volume = VXCMContext.Instance.CreateVolumeFromTexture(texture);

           // yield return StartCoroutine("CallPluginAtEndOfFrames");
        }

        void OnApplicationQuit()
        {
         //   volume.Destroy(); // free the resource

      //      VXCMContext.Instance.freeContext();
        }

        // chiamata ad ogni frame alla fine della catena
        //private IEnumerator CallPluginAtEndOfFrames()
        //{
        //    while (true)
        //    {
        //        // Wait until all frame rendering is done
        //        yield return new WaitForEndOfFrame();

        //        // Issue a plugin event with arbitrary integer identifier.
        //        // The plugin can distinguish between different
        //        // things it needs to do based on this ID.
        //        // For our simple plugin, it does not matter which ID we pass here.

        //        //volume.LoadTexture(VXCMVolumeLayerType.VXCMVolumeLayerType_DistanceField);

        //   //     volume.RegisterRenderEventFunc();

        //    }
        //}

        void Update()
        {
            if (Camera.current == null) return;

            //volume.LoadTexture(VXCMVolumeLayerType.VXCMVolumeLayerType_DistanceField);


            material.SetTexture("_Volume", texture);

            material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);

            // current camera info
            float fYfovRad = Camera.current.fieldOfView * Mathf.Deg2Rad;
            float tanFov_2 = (float)Math.Tan(fYfovRad / 2.0);
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