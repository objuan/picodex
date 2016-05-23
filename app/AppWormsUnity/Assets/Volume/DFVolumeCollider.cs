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
    //  [ExecuteInEditMode]
    //public class _DFVolumeCollider : Collider
    //{
    //    void Start()
    //    {
    //      //  bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(64, 64, 64));
    //    }

    //    public new bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
    //    {
    //        hitInfo = new RaycastHit();
    //        return true;
    //    }
    //    void OnCollisionEnter(Collision collision)
    //    {
    //        foreach (ContactPoint contact in collision.contacts)
    //        {
    //            Debug.DrawRay(contact.point, contact.normal, Color.white);
    //        }
    //        //if (collision.relativeVelocity.magnitude > 2)
    //        //    audio.Play();

    //    }
    //}

    public class DFVolumeCollider : MonoBehaviour
    {
        DFVolume volume;
        GameObject proxyGameObject;
        MeshCollider collider1;

        //public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        //{
        //    hitInfo = new RaycastHit();
        //    return true;
        //}

        void Start()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            volume = GetComponent<DFVolumeFilter>().volume;

            // buikd the mesh
//
   //         Mesh mesh = new TransvoxelExtractor(new VXCMVolumeData(volume)).GenLodCell(new Vector3i(0, 0, 0), volume.resolution, 1);

            proxyGameObject = GetComponent<DFVolumeRenderer>().proxyGameObject;

            collider1 = proxyGameObject.AddComponent<MeshCollider>();
            //collider1.convex = true;
            collider1.sharedMesh = proxyGameObject.GetComponent<MeshFilter>().sharedMesh;

            // gameObject.AddComponent<_DFVolumeCollider>().enabled=true;
        }

    }
}