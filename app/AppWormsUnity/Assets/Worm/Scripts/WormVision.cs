using System;
using System.Collections.Generic;
using UnityEngine;

namespace Picodex
{

    [ExecuteInEditMode]
    public class WormVision : MonoBehaviour
    {

        DFVolumeCollider volumeCollider;

        void Start()
        {
            volumeCollider = GameObject.FindGameObjectWithTag("Planet").GetComponent<DFVolumeCollider>();

            //TEST
            //GameObject go = PrimitiveHelper.CreatePrimitive(PrimitiveType.IsoSphere, 20);
            //go.transform.parent = transform;
        }

        public void Update()
        {

            if (!volumeCollider) return;

            VolumeRaycastRequest req;
            if (Volumetric.RaycastSpherical(volumeCollider, transform.position, out req)>0)
            {
                if (req == null) return;
                List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();

                req.FillCollidedEntries(entryList);
                foreach (VolumeRaycastRequestEntry e in entryList)
                    Debug.DrawLine(transform.position, e.hit.point, Color.yellow);

            }
        }

    }
}
