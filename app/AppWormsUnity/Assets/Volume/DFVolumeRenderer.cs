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

    public enum DFVolumeRendererProxyType
    {
        Box = 0,
        Sphere,
        Optimized,
    }

    [AddComponentMenu("Vxcm/DFVolumeRenderer")]
    [ExecuteInEditMode]
    public class DFVolumeRenderer : MonoBehaviour
    {
        [Range(0, 1)]
        public float CutPlaneXZ = 1f;

        public UnityEngine.Rendering.ShadowCastingMode shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        public bool receiveShadows = true;

        public DFVolumeRendererMode renderMode = DFVolumeRendererMode.Raycast;

        public DFVolumeRendererProxyType proxyType = DFVolumeRendererProxyType.Box;

        // runtime
        VXCMObject_v02 vxcmObject;

        [System.NonSerialized]
        public  DFVolume volume;

        [System.NonSerialized]
        public GameObject proxyGameObject=null;

        [RangeAttribute(1,4)]
        public int lod=1;

        public Material material;

        [System.NonSerialized]
        public ProxyBuilder proxyBuilder;

        // tmp vars

        DFVolumeRendererMode old_renderMode = DFVolumeRendererMode.Raycast;
        DFVolumeRendererProxyType old_proxyType = DFVolumeRendererProxyType.Box;
        int old_lod =1;
   
        void Awake()
        {
            rebuild();
        }

        void Start()
        {

            //   material;

            material = new Material(Shader.Find("Vxcm/Object/ray_v07"));

            Volume.AddVolume(this);
        }


        void rebuild()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            volume = GetComponent<DFVolumeFilter>().volume;


            //if (gameObject.transform.childCount  > 0)
            //{
            //    // detach the child
            //    GameObject go = gameObject.transform.GetChild(0).gameObject;
            //    go.transform.parent = null;
            //    DestroyImmediate(go);
            //    vxcmObject = null;
            //}
            if (renderMode == DFVolumeRendererMode.Raycast)
            {
                //if (proxyType == DFVolumeRendererProxyType.Box)
                //    proxyGameObject = PrimitiveHelper.CreatePrimitive(PrimitiveType.Cube);
                //else
                //    proxyGameObject = PrimitiveHelper.CreatePrimitive(PrimitiveType.Sphere);

                //proxyBuilder.Rebuild();
                //proxyGameObject = proxyBuilder.proxyGameObject;

                // link on root

                // ---------------

                if (proxyBuilder == null)
                {
                    proxyBuilder = new ProxyBuilder(this);
                    proxyGameObject = proxyBuilder.proxyGameObject;
                }


                if (proxyGameObject.GetComponent<VXCMObject_v02>() == null)
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

            proxyGameObject.GetComponent<MeshRenderer>().shadowCastingMode = shadowCastingMode;
            proxyGameObject.GetComponent<MeshRenderer>().receiveShadows = receiveShadows;

          //  proxyGameObject.hideFlags = HideFlags.HideAndDontSave;
            proxyGameObject.hideFlags = HideFlags.DontSave;

            // save
            old_renderMode = renderMode;
            old_proxyType = proxyType;
            old_lod = lod;
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
            if (renderMode != old_renderMode  || proxyType  != old_proxyType  || old_lod != lod)
            {
                rebuild();
                if (proxyBuilder!=null)
                    proxyBuilder.Rebuild();
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
