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

            //Mesh mesh = new Mesh();
            //PrimitiveHelper.CreateSphere(mesh, 1);
            //gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        }

        public void Update()
        {

            if (!volumeCollider) return;

            VolumeRaycastRequest req;
            if (Volumetric.RaycastSpherical(volumeCollider, transform.position, out req))
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
