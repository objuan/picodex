using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Internal;

namespace Picodex.Vxcm
{
    class VolumeCollection 
    {
        public List<DFVolumeRenderer> volumeList = new List<DFVolumeRenderer>();

    }

    public struct VolumeRaycastHit
    {
    
        //
        // Summary:
        //     ///
        //     The distance from the ray's origin to the impact point.
        //     ///
        public float distance { get; set; }
     
        //
        // Summary:
        //     ///
        //     The normal of the surface the ray hit.
        //     ///
        public Vector3 normal { get; set; }
        //
        // Summary:
        //     ///
        //     The impact point in world space where the ray hit the collider.
        //     ///
        public Vector3 point { get; set; }

        //
        // Summary:
        //     ///
        //     The impact point in world space where the ray hit the collider.
        //     ///
        public Vector3 volumePoint { get; set; }
    }

    // ============================================================================
    // ============================================================================

    public class Volume
    {
        static VolumeCollection volumeCollection = new VolumeCollection();

        public static void AddVolume(DFVolumeRenderer volume)
        {
            volumeCollection.volumeList.Add(volume);

            Debug.Log("AddVolume count="+ volumeCollection.volumeList.Count);

        }
        public static void RemoveVolume(DFVolumeRenderer volume)
        {
            volumeCollection.volumeList.Remove(volume);

            Debug.Log("RemoveVolume count=" + volumeCollection.volumeList.Count);
        }

        // ====================================================================

        public static bool Raycast(Ray ray, out VolumeRaycastHit hitInfo)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Ray ray, float maxDistance)
        {
            VolumeRaycastHit hitInfo = new VolumeRaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction)
        {
            VolumeRaycastHit hitInfo = new VolumeRaycastHit();
            return Raycast(origin, direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo)
        {
            return Raycast(origin, direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }
      
        public static bool Raycast(Ray ray, out VolumeRaycastHit hitInfo, float maxDistance)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }
      
        public static bool Raycast(Ray ray, float maxDistance, int layerMask)
        {
            VolumeRaycastHit hitInfo = new VolumeRaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
        {
            VolumeRaycastHit hitInfo = new VolumeRaycastHit();
            return Raycast(origin, direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            VolumeRaycastHit hitInfo = new VolumeRaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }


        public static bool Raycast(Ray ray, out VolumeRaycastHit hitInfo, float maxDistance, int layerMask)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, float maxDistance)
        {
            return Raycast(origin, direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
        {
            VolumeRaycastHit hit = new VolumeRaycastHit();
            return Raycast(origin, direction, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal);
        }
        public static bool Raycast(Ray ray, out VolumeRaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, float maxDistance, int layerMask)
        {
            return Raycast(origin, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            VolumeRaycastHit hit = new VolumeRaycastHit();
            return Raycast(origin, direction, out hit , maxDistance, layerMask, queryTriggerInteraction);
        }

        // -------------------
        public static bool Raycast(Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            // return volumeCollection.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            foreach (DFVolumeRenderer volumeRenderer in volumeCollection.volumeList)
            {
                if (Raycast(volumeRenderer, origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction))
                    return true;
            }
            hitInfo = new VolumeRaycastHit();
            return false;
        }

        public static bool Raycast(DFVolumeRenderer volumeRenderer, Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo)
        {
            return Raycast(volumeRenderer,origin, direction,  out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(DFVolumeRenderer volumeRenderer, Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            hitInfo = new VolumeRaycastHit();
            float distance;
            float volumeDistance = 0;
            Vector3 surfaceNormal = new Vector3();

            GameObject go = volumeRenderer.proxyGameObject;
            if (!go) return false;
            MeshRenderer proxyRenderer = go.GetComponent<MeshRenderer>();
            VXCMVolume volume = volumeRenderer.volume;
            DFVolumeCollider collider = volumeRenderer.gameObject.GetComponent<DFVolumeCollider>();
            if (!collider) return false;

            Matrix4x4 worldToLocal = proxyRenderer.transform.worldToLocalMatrix;

            Vector3 localOrigin = worldToLocal.MultiplyPoint(origin);
            Vector3 localDir = worldToLocal.MultiplyVector(direction);
            Ray ray = new Ray(localOrigin, localDir);

            Bounds bounds = new Bounds(Vector3.zero, new Vector3(volume.resolution.x, volume.resolution.y, volume.resolution.z));

            if (bounds.IntersectRay(ray, out distance))
            {
                Vector3 localPos = ray.GetPoint(distance);

                Vector3 volumePos = volume.objectToVolumeTrx.MultiplyPoint(localPos);

                //Debug.Log("hit " + distance);
                //Debug.Log("loc " + localPos);
                //Debug.Log("vol " + volumePos);

                if (collider.Raycast(volumePos, localDir, ref volumeDistance, ref surfaceNormal))
                {
                    // volumeDistance is in volumeDistance
               //     Debug.Log("hit " + volumeDistance + " normal = " + surfaceNormal);

                    hitInfo.volumePoint = volumePos + direction * volumeDistance;
                    hitInfo.point = origin + (direction * (distance + volumeDistance * volume.resolution.x));
                    hitInfo.normal = surfaceNormal;
                    return true;
                }
            }
            return false;
        }


    }
}
