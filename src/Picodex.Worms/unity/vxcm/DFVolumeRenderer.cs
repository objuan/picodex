using UnityEngine;
using System.Collections;

using Picodex.Vxcm;

namespace Picodex.Unity.Vxcm
{	
	[ExecuteInEditMode]
	/// Controls some visual aspects of the colord cubes volume and allows it to be rendered.
	/**
	 * See the base VolumeRenderer class for further details and available properties.
	 */
	public class DFVolumeRenderer : VolumeRenderer
	{
		void Awake()
		{
			if(material == null)
			{
				// This shader should be appropriate in most scenarios, and makes a good default.
				material = Instantiate(Resources.Load("Materials/VolumeDF", typeof(Material))) as Material;
			}
		}

        [Range(0.001f, 1)]
        public float SampleRate = 0.01f;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [Range(0, 3)]
        public int DebugLayer = 2;

        public Texture3D texture;

        private Matrix4x4 objectToVolumeTrx;

        //   private VXCMVolume volume =null;
        private VXCMVolume volume;

        GameObject renderNode;
      //  Material material;

        void Start()
        {
           // if (material == null) return;

            volume = GetComponent<DFVolume>().data.volume;

            MeshRenderer meshRenderer = null;
            MeshFilter meshFilter = null;
            if (transform.childCount == 0)
            {
                renderNode = CreateDummyNode();

                meshFilter = renderNode.GetOrAddComponent<MeshFilter>() as MeshFilter;
                if (meshFilter.sharedMesh == null)
                {
                    meshFilter.sharedMesh = new Mesh();

                    // build the volume proxy

                    Picodex.Box.CreateBox(meshFilter.sharedMesh, 64, 64, 64);

                }
                meshRenderer = renderNode.GetOrAddComponent<MeshRenderer>() as MeshRenderer;
                meshRenderer.enabled = true;
            }
            else
            {
                renderNode = transform.GetChild(0).gameObject;
                meshFilter = renderNode.GetOrAddComponent<MeshFilter>() as MeshFilter;
                meshRenderer = renderNode.GetOrAddComponent<MeshRenderer>() as MeshRenderer;
            }


            Picodex.Box.CreateBox(meshFilter.sharedMesh, 64, 64, 64);
            meshRenderer.material = material;

             // MeshRenderer meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>() as MeshRenderer;


             //  VXCMContext.Instance.useContext();

             //   Renderer re = GetComponent<Renderer>();
             //   re.material = new Material(Shader.Find("Vxcm/Texture3D/slice_v01"));
             //   material = GetComponent<Renderer>().material;
             //   mesh = GetComponent<MeshFilter>().mesh;

             //  texture = GetTextureUVW(size);

             objectToVolumeTrx = new Matrix4x4();

            //TODO
            Bounds bounds = new Bounds(new Vector3(32,32,32),new Vector3(64,64,64));//; ; mesh.bounds;

            Vector3 scale = new Vector3(1.0f / bounds.size.x, 1.0f / bounds.size.y, 1.0f / bounds.size.z);
            objectToVolumeTrx.SetTRS(-bounds.min, Quaternion.identity, scale);

            texture = new Texture3D(64, 64, 64, TextureFormat.RGBA32, false);
            texture.SetPixels32(volume.DF);
            texture.Apply();



            //volume = DFUtil.getVolume();
            // texture = DFUtil.getTexture();

        }


        void Update()
        {
            if (Camera.current == null) return;
            if (material == null) return;

            //volume.LoadTexture(VXCMVolumeLayerType.VXCMVolumeLayerType_DistanceField);

            material.SetTexture("_Volume", texture);

            material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);

            // current camera info
            float fYfovRad = Camera.current.fieldOfView * Mathf.Deg2Rad;
            float tanFov_2 = (float)System.Math.Tan(fYfovRad / 2.0);
            float h2 = Camera.current.nearClipPlane * tanFov_2;
            float w2 = Camera.current.aspect * h2;

            material.SetVector("u_cameraInfo", new Vector3(w2, h2, Camera.current.nearClipPlane));

            // azzero la traslazione della camera, prendo solo la rotazione
            Matrix4x4 camRot = Camera.current.transform.localToWorldMatrix;

            Picodex.Math.MathUtility.SetMatrixTranslation(ref camRot, new Vector3(0, 0, 0));

            material.SetMatrix("u_vxcm_worldViewMatrix", camRot);

            material.SetFloat("u_sample_rate", SampleRate);

            material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
            material.SetInt("u_debug_layer", DebugLayer);

        }

        private GameObject CreateDummyNode()
        {
            // Build a corresponding game object
            System.Text.StringBuilder name = new System.Text.StringBuilder("Render Node");
            GameObject newGameObject = new GameObject(name.ToString());
          //  newGameObject.hideFlags = HideFlags.HideInHierarchy;

            // Use parent properties as appropriate
            newGameObject.transform.parent = gameObject.transform;
            newGameObject.layer = gameObject.layer;

            // It seems that setting the parent does not cause the object to move as Unity adjusts 
            // the child transform to compensate (this can be seen when moving objects between parents 
            // in the hierarchy view). Reset the local transform as shown here: http://goo.gl/k5n7M7
            newGameObject.transform.localRotation = Quaternion.identity;
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.transform.localScale = Vector3.one;

            return newGameObject;
        }

    }
}
