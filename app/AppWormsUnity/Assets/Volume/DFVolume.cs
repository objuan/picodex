using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;
using UnityEngine.Rendering;

using Picodex.Vxcm;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Picodex
{
    [ExecuteInEditMode]
    public class DFVolume : MonoBehaviour
    {
        public Vector3i resolution
        {
            get
            {
                return GetComponent<DFVolumeFilter>().volumeData.resolution;
            }
        }

  
        //protected void OnValidate()
        //{
        //    if (resolution != mData.Region.size)
        //    {
        //        this.mData.Resize(resolution);
        //    }
        //}


        private DFVolumeRenderer renderer;

        [System.NonSerialized]
        public GameObject proxyGameObject;

        void Awake()
        {
            proxyGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            DestroyImmediate(proxyGameObject.GetComponent<Collider>());
            proxyGameObject.hideFlags = HideFlags.DontSave;
        }

        void Start()
        {
            if (GetComponent<DFVolumeFilter>()==null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            VXCMObject_v02 obj = proxyGameObject.AddComponent<VXCMObject_v02>();
            obj.volume = GetComponent<DFVolumeFilter>().volumeData.volume;

            renderer = GetComponent<DFVolumeRenderer>();

            proxyGameObject.hideFlags = HideFlags.HideAndDontSave;
           // proxyGameObject.hideFlags = HideFlags.DontSave;

            proxyGameObject.transform.parent = this.transform;
            proxyGameObject.transform.setLocalToIdentity();

        }


        void OnApplicationQuit()
        {
            DestroyImmediate(proxyGameObject);
        }



        private void FixedUpdate()
        {
            Camera camera = Camera.current;
            if (!camera) return;

        }

        private void Update()
        {
 
        }
        /// \endcond

        /* \param data The volume data which should be attached to the construced volume.

         * \param addRenderer Specifies whether a renderer component should be added so that the volume is displayed.

         * \param addCollider Specifies whether a collider component should be added so that the volume can participate in collisions.

         */
		public static GameObject CreateGameObject(DFVolumeData data, bool addRenderer, bool addCollider)
        {
            // Create our main game object representing the volume.
            GameObject volumeGameObject = new GameObject("DFVolume");

            //Add the required volume component.
            DFVolume dfVolume = volumeGameObject.GetOrAddComponent<DFVolume>();


            // Set the provided data.
            //  dfVolume.data = data;

            DFVolumeFilter volumeFilter = volumeGameObject.AddComponent<DFVolumeFilter>();
            volumeFilter.volumeData = data;


            // Add the renderer and collider if desired.
            if (addRenderer) { volumeGameObject.AddComponent<DFVolumeRenderer>(); }
            if (addCollider) { volumeGameObject.AddComponent<DFVolumeCollider>(); }

            // Return the created object
            return volumeGameObject;
        }

        // It seems that we need to implement this function in order to make the volume pickable in the editor.
        // It's actually the gizmo which get's picked which is often bigger than than the volume (unless all
        // voxels are solid). So somtimes the volume will be selected by clicking on apparently empty space.
        // We shold try and fix this by using raycasting to check if a voxel is under the mouse cursor?
        void OnDrawGizmos()
        {
            DFVolumeUI.OnDrawGizmos(this);
        }


    }
}