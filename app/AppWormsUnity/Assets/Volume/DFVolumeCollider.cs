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
    public class _DFVolumeCollider : Collider
    {
        void Start()
        {
          //  bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(64, 64, 64));
        }

        public new bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            hitInfo = new RaycastHit();
            return true;
        }
        void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
            //if (collision.relativeVelocity.magnitude > 2)
            //    audio.Play();

        }
    }

    public class DFVolumeCollider : MonoBehaviour
    {
        public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            hitInfo = new RaycastHit();
            return true;
        }

        void Start()
        {
          //  gameObject.AddComponent<_DFVolumeCollider>().enabled=true;
        }

    }
}