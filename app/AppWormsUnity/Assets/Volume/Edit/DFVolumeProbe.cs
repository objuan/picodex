using System;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using Picodex.Vxcm;

namespace Picodex
{
  
    [ExecuteInEditMode]
    public class DFVolumeProbe : MonoBehaviour
    {
        DFVolume volume=null;
        DFVolumeRenderer volumeRenderer;
        DFVolumeCollider volumeCollider;

        public GameObject volumeObject;


        void Start()
        {
            if (!volumeObject) return;

            volume = volumeObject.GetComponent<DFVolumeFilter>().volume;
            volumeRenderer = volumeObject.GetComponent<DFVolumeRenderer>();
            volumeCollider = volumeObject.GetComponent<DFVolumeCollider>();
        }

        public void Update()
        {
            if (!volume) return;

            VolumeRaycastRequest req;
            if (Volumetric.RaycastSpherical(volumeCollider, transform.position, out req) )
            {
                if (req==null) return;
                List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();

                req.FillCollidedEntries(entryList);
                foreach(VolumeRaycastRequestEntry e in entryList)
                    Debug.DrawLine(transform.position,e.hit.point,Color.cyan);

            }
        }

      

    }
}
