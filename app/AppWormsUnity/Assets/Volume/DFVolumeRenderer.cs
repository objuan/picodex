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
        VXCMObject vxcmObject;

      //  [System.NonSerialized]
        public DFVolume volume
        {
            get
            {
                return GetComponent<DFVolumeFilter>().volume;
            }
        }

        //    [System.NonSerialized]
        GameObject _proxyGameObject = null;

        public GameObject proxyGameObject
        {
            get
            {
                if (_proxyGameObject == null)
                {
                    if (gameObject.transform.childCount > 0)
                    {
                        // detach the child
                        _proxyGameObject =  gameObject.transform.GetChild(0).gameObject;
                    }
                }
                return _proxyGameObject;
            }
        }

        [RangeAttribute(1,4)]
        public int lod=1;

        public Material material;

        //[System.NonSerialized]
        //public ProxyBuilder proxyBuilder;

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
            GameObject _proxyGameObject = proxyGameObject;
            MeshRenderer proxyRender = _proxyGameObject.GetComponent<MeshRenderer>();

            // check for UI changes
            if (renderMode != old_renderMode  || proxyType  != old_proxyType  || old_lod != lod 
                || proxyRender.shadowCastingMode != shadowCastingMode
                || proxyRender.receiveShadows != receiveShadows)
            {
                rebuild();
            }
        }
#endif

        void OnEnable()
        {
            if (!proxyGameObject) return;
            proxyGameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        void OnDisable()
        {
            if (!proxyGameObject) return;
            proxyGameObject.GetComponent<MeshRenderer>().enabled = false;
        }

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

        // ===================================================================

        void rebuild()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

         //   volume = GetComponent<DFVolumeFilter>().volume;

            if (gameObject.transform.childCount == 0)
            {
                // metto il proxy object

                if (renderMode == DFVolumeRendererMode.Raycast)
                {

                    // ---------------

                    GameObject child = VXCMObjectProxy.CreateGameObject("VXCM Volume");

                    child.transform.parent = this.transform;
                    child.transform.setLocalToIdentity();

                    vxcmObject = proxyGameObject.AddComponent<VXCMObject>();

                    vxcmObject.volume = GetComponent<DFVolumeFilter>().volume;

                    // proxyGameObject.hideFlags = HideFlags.DontSave;
                    proxyGameObject.hideFlags = HideFlags.HideAndDontSave;

                }
                else
                {
                    GameObject child = VXCMMeshBuilder.CreateMeshTransvoxel(volume, volume.resolution, lod);

                    child.transform.parent = this.transform;
                    child.transform.setLocalToIdentity();
                }
            }
            else
                proxyGameObject.GetComponent<VXCMObjectProxy>().Rebuild();

            MeshRenderer proxyRender = proxyGameObject.GetComponent<MeshRenderer>();
            proxyRender.shadowCastingMode = shadowCastingMode;
            proxyRender.receiveShadows = receiveShadows;
            proxyRender.enabled = enabled;

            //  proxyGameObject.hideFlags = HideFlags.HideAndDontSave;
           // proxyGameObject.hideFlags = HideFlags.DontSave;

            // save
            old_renderMode = renderMode;
            old_proxyType = proxyType;
            old_lod = lod;
        }

    }
}
