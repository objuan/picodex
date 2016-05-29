using UnityEngine;
using System.Collections;

//using Cubiquity.Impl;

using Picodex.Vxcm;

namespace Picodex
{
    public enum DFVolumeRendererMode
    {
        Raycast = 0,
        Mesh
    }
 
    [AddComponentMenu("Vxcm/DFVolumeRenderer")]
    [ExecuteInEditMode]
    public class DFVolumeRenderer : MonoBehaviour
    {
        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        public bool castShadows = true;

        public bool receiveShadows = true;

        public DFVolumeRendererMode renderMode = DFVolumeRendererMode.Raycast;

        // runtime
        VXCMObject_v02 vxcmObject;

        [System.NonSerialized]
        public  DFVolume volume;

        [System.NonSerialized]
        public GameObject proxyGameObject=null;

        [RangeAttribute(1,4)]
        public int lod=1;

        [System.NonSerialized]
        Material _material;

        /// Material for this volume.
        public Material material
        {
            get
            {
                if (renderMode == DFVolumeRendererMode.Raycast)
                {
                    _material = vxcmObject.GetComponent<Renderer>().material;
                }
                else
                    _material = proxyGameObject.GetComponent<Renderer>().material;
                return _material;
            }
            set
            {
            }
        }

 
 
        [System.NonSerialized]
        public DFVolumeRendererMode old_renderMode = DFVolumeRendererMode.Raycast;

        [System.NonSerialized]
        public int old_lod =1;


        void Awake()
        {

            //  Debug.Log("Awake 1");
            // proxy
            rebuild();

        }

        void rebuild()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            volume = GetComponent<DFVolumeFilter>().volume;


            if (gameObject.transform.childCount  > 0)
            {
                // detach the child
                GameObject go = gameObject.transform.GetChild(0).gameObject;
                go.transform.parent = null;
                DestroyImmediate(go);
                vxcmObject = null;
            }
            if (renderMode == DFVolumeRendererMode.Raycast)
            {
                proxyGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                DestroyImmediate(proxyGameObject.GetComponent<Collider>());

                // link on root

                // ---------------

                vxcmObject = proxyGameObject.AddComponent<VXCMObject_v02>();
                vxcmObject.volume = GetComponent<DFVolumeFilter>().volume;


                // proxyGameObject.hideFlags = HideFlags.DontSave;

            }
            else
            {
                proxyGameObject = VXCMMeshBuilder.CreateMeshTransvoxel(volume, volume.resolution, lod);
            }

            proxyGameObject.name = "VXCM Volume";
            proxyGameObject.transform.parent = this.transform;
            proxyGameObject.transform.setLocalToIdentity();

          //  proxyGameObject.hideFlags = HideFlags.HideAndDontSave;
            proxyGameObject.hideFlags = HideFlags.DontSave;

            // save
            old_renderMode = renderMode;
            old_lod = lod;
        }

        void Start() {

            //   material;

            Volume.AddVolume(this);
        }

        void OnWillRenderObject()
        {
            //if (renderMode != old_renderMode || old_lod != lod)
            //{
            //    rebuild();
            //}
        }

#if UNITY_EDITOR
        void Update()
        {
            // check for UI changes
            if (renderMode != old_renderMode || old_lod != lod)
            {
                rebuild();
            }
        }
#endif

        void OnApplicationQuit()
        {
            DestroyImmediate(proxyGameObject);

            Volume.RemoveVolume(this);
        }

        // It seems that we need to implement this function in order to make the volume pickable in the editor.
        // It's actually the gizmo which get's picked which is often bigger than than the volume (unless all
        // voxels are solid). So somtimes the volume will be selected by clicking on apparently empty space.
        // We shold try and fix this by using raycasting to check if a voxel is under the mouse cursor?
        void OnDrawGizmos()
        {
            DFVolumeUI.OnDrawGizmos(gameObject);
        }

   
    }
}
