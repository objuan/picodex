using UnityEngine;
using System.Collections;

//using Cubiquity.Impl;

using Picodex.Vxcm;

namespace Picodex
{
	/// Controls some visual aspects of the volume and allows it to be rendered.
	/**
	 * The role of the VolumeRenderer component for volumes is conceptually similar to the role of Unity's MeshRenderer class for meshes.
	 * Specifically, it can be attached to a GameObject which also has a Volume component to cause that Volume component to be drawn. It 
	 * also exposes a number of properties such as whether a volume should cast and receive shadows.
	 * 
	 * Remember that Cubiquity acctually draws the volume by creating standard Mesh objects. Internally Cubiquity will copy the properties
	 * of the VolumeRenderer to the MeshRenderers which are generated.
	 * 
	 * \sa VolumeCollider
	 */
  //  [ExecuteInEditMode]
	public class DFVolumeRenderer : MonoBehaviour
    {
        [System.NonSerialized]
        Texture3D texture = null;

        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        [System.NonSerialized]
        Mesh proxyMesh;

        [SerializeField]
        private Material mMaterial;

        [SerializeField]
        private bool mReceiveShadows = true;

        [SerializeField]
        private bool mShowWireframe = false;

        /// \cond
        [System.NonSerialized]
        public bool hasChanged = true;
        /// \endcond

        DFVolume volume;

        Matrix4x4 objectToVolumeTrx;

        DFVolumeData data;

        /// Material for this volume.
        public Material material
        {
            get
            {
                return mMaterial;
            }
        }
      
		/// Controls whether this volume casts shadows.
		public bool castShadows
		{
			get
			{
				return mCastShadows;
			}
			set
			{
				if(mCastShadows != value)
				{
					mCastShadows = value;
                    hasChanged = true;
				}
			}
		}
		[SerializeField]
		private bool mCastShadows = true;
		
		/// Controls whether this volume receives shadows.
		public bool receiveShadows
		{
			get
			{
				return mReceiveShadows;
			}
			set
			{
				if(mReceiveShadows != value)
				{
					mReceiveShadows = value;
                    hasChanged = true;
				}
			}
		}
		
        /// Controls whether the wireframe overlay is displayed when this volume is selected in the editor.
        public bool showWireframe
		{
			get
			{
				return mShowWireframe;
			}
			set
			{
				if(mShowWireframe != value)
				{
					mShowWireframe = value;
                    hasChanged = true;
				}
			}
		}

        // 

        void Start() {
            volume = GetComponent<DFVolume>();
            data = GetComponent<DFVolumeFilter>().volumeData;

           // mMaterial = new Material(Shader.Find("Vxcm/Object/ray_v05"));

            proxyMesh = volume.proxyGameObject.GetComponent<MeshFilter>().sharedMesh;
           // volume.proxyGameObject.GetComponent<MeshFilter>().GetComponent<Renderer>().material = material;
        }

        void OnEnable()
        {
            hasChanged = true;
        }

        void OnDisable()
        {
            hasChanged = true;
        }


        //public void OnWillRenderObject()
        //{
        //    if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
        //        return;

        //    Camera cam = Camera.current;
        //    if (!cam) return;

        //    UpdateTexture();

        //    UpdateMat();
        //}

        //public void UpdateTexture()
        //{
        //    if (texture == null || (texture != null && (texture.width != volume.resolution.x) || (texture.height != volume.resolution.y) || (texture.depth != volume.resolution.z)))
        //    {
        //        if (true)
        //        {
        //            VolumePrimitiveSphere raster = new VolumePrimitiveSphere(data.volume);

        //            GeometrySample sample = new GeometrySample();
        //            sample.debugColor = new Vector3(1, 0, 0);

        //            raster.Raster(new Vector3(-10, 0, 0), 10, sample);
        //            raster.Raster(new Vector3(10, 0, 0), 10, sample);
        //        }

        //        PrimitiveHelper.CreateCube(proxyMesh, volume.resolution.x, volume.resolution.y, volume.resolution.z);

        //        texture = new Texture3D(volume.resolution.x, volume.resolution.y, volume.resolution.z, TextureFormat.RGBA32, false);
        //        texture.SetPixels32(data.DF);
        //        texture.Apply();

        //        objectToVolumeTrx = new Matrix4x4();

        //        Bounds bounds = proxyMesh.bounds;

        //        float m = 1.0f / 2;
        //        Vector3 scale = new Vector3(1.0f / volume.resolution.x, 1.0f / volume.resolution.y, 1.0f / volume.resolution.z);
        //        objectToVolumeTrx.SetTRS(new Vector3(m, m, m), Quaternion.identity, scale);

        //        // mat

        //        material.SetFloat("DF_MIN", volume.data.distanceFieldRangeMin);
        //        material.SetFloat("DF_MAX_MINUS_MIN", volume.data.distanceFieldRangeMax - volume.data.distanceFieldRangeMin);

        //        material.SetMatrix("u_objectToVolumeTrx", objectToVolumeTrx);
        //        material.SetMatrix("u_objectToVolumeInvTrx", objectToVolumeTrx.inverse);

        //        material.SetVector("u_textureRes", scale);


        //    }
        //}

        //public void UpdateMat()
        //{
        //    if (Camera.current == null) return;
        //    if (material == null) return;

        //    material.SetTexture("_Volume", texture);

        //    material.SetFloat("u_cut_plane_xz", CutPlaneXZ);
        //}
    }
}
