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
        public bool axeMode = false;

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
            if (Volumetric.RaycastSpherical(volumeCollider, transform.position, out req) >0)
            {
                if (req==null) return;
                List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();

                req.FillCollidedEntries(entryList);
                foreach (VolumeRaycastRequestEntry e in entryList)
                {
                    if (axeMode)
                    {
                        if (e.direction.x == -1 || e.direction.x== 1)
                            Debug.DrawLine(transform.position, e.hit.point,  Color.red );
                        if (e.direction.y == -1 || e.direction.y == 1)
                            Debug.DrawLine(transform.position, e.hit.point, Color.green);
                        if (e.direction.z == -1 || e.direction.z == 1)
                            Debug.DrawLine(transform.position, e.hit.point, Color.blue);
                    }
                    else
                        Debug.DrawLine(transform.position, e.hit.point, Color.cyan);
                }

            }
        }

      

    }
}
