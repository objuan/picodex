using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Internal;

namespace Picodex
{
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

        public DFVolumeCollider colliderVolume { get; set; }
    }

    // ============================================================================
    // ============================================================================

    public class VolumeRaycastRequestEntry
    {
        public Vector3 origin;
        public Vector3 direction;
        public VolumeRaycastHit hit; 
        public bool hasCollision
        {
            get { return hit.colliderVolume != null; }
        }
        // runtime
        public bool active = false;
        public Vector3 localDir;
        public Vector3 volumePos;
        public float localDistance;
    }

    public class VolumeRaycastRequest
    {
        public List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();

        public int Count
        {
           get { return entryList.Count; }
        }

        public void Clear()
        {
            entryList.Clear();
        }

        public VolumeRaycastRequestEntry AddRaycast(Ray ray)
        {
            return AddRaycast(ray.origin, ray.direction);
        }

        public VolumeRaycastRequestEntry AddRaycast(Vector3 origin, Vector3 direction)
        {
            VolumeRaycastRequestEntry entry = new VolumeRaycastRequestEntry();
            entry.origin = origin;
            entry.direction = direction;
            entry.hit.colliderVolume = null;
            entryList.Add(entry);
            return entry;
        }

        public VolumeRaycastHit GetMinDistanceHit()
        {
            float d = 999999;
            VolumeRaycastHit min = new VolumeRaycastHit();
            min.colliderVolume = null;
            foreach (VolumeRaycastRequestEntry entry in entryList)
            {
                if (entry.hasCollision && entry.hit.distance < d)
                {
                    d = entry.hit.distance;
                    min = entry.hit;
                }
            }
            return min;
        }

        public void FillActiveEntries(List<VolumeRaycastRequestEntry> out_entryList)
        {
            foreach (VolumeRaycastRequestEntry e in entryList)
                if (e.active) out_entryList.Add(e);
        }
        public void FillCollidedEntries(List<VolumeRaycastRequestEntry> out_entryList)
        {
            foreach (VolumeRaycastRequestEntry e in entryList)
                if (e.hasCollision) out_entryList.Add(e);
        }
    }
    // ============================================================================
    // ============================================================================

    public class Volumetric
    {
        static List<DFVolumeCollider> volumeList = new List<DFVolumeCollider>();

        public static void AddVolume(DFVolumeCollider volume)
        {
            volumeList.Add(volume);

            Debug.Log("AddVolume count=" + volumeList.Count);

        }
        public static void RemoveVolume(DFVolumeCollider volume)
        {
            volumeList.Remove(volume);

            Debug.Log("RemoveVolume count=" + volumeList.Count);
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
            return Raycast(origin, direction, out hit, maxDistance, layerMask, queryTriggerInteraction);
        }

        // -------------------
        public static bool Raycast(Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            // return volumeCollection.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            foreach (DFVolumeCollider volumeCollider in volumeList)
            {
                if (Raycast(volumeCollider, origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction))
                    return true;
            }
            hitInfo = new VolumeRaycastHit();
            return false;
        }

        public static bool Raycast(DFVolumeCollider volumeCollider, Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo)
        {
            return Raycast(volumeCollider, origin, direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(DFVolumeCollider collider, Vector3 origin, Vector3 direction, out VolumeRaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            VolumeRaycastRequest req = new VolumeRaycastRequest();
            VolumeRaycastRequestEntry entry = req.AddRaycast(origin, direction);
            if (Raycast(collider, req))
            {
                hitInfo = entry.hit;
                return true;
            }
            else {
                hitInfo = new VolumeRaycastHit();
                return false;
            }
        }

        public static bool Raycast(DFVolumeCollider collider, VolumeRaycastRequest request)
        {
            //    hitInfo = new VolumeRaycastHit();
          //  float distance;
            //float volumeDistance = 0;
            //Vector3 surfaceNormal = new Vector3();

            //GameObject go = collider.proxyGameObject;
            //if (!go) return false;
            //MeshRenderer proxyRenderer = go.GetComponent<MeshRenderer>();
            //Vxcm.VXCMVolume volume = collider.GetComponent<DFVolumeFilter>().volume;
            //// DFVolumeCollider collider = volumeCollider.gameObject.GetComponent<DFVolumeCollider>();
            //////f (!collider) return false;

            //Matrix4x4 worldToLocal = proxyRenderer.transform.worldToLocalMatrix;
            //Bounds bounds = new Bounds(Vector3.zero, new Vector3(volume.resolution.x, volume.resolution.y, volume.resolution.z));

            //foreach (VolumeRaycastRequestEntry entry in request.entryList)
            //{
            //    Vector3 localOrigin = worldToLocal.MultiplyPoint(entry.origin);
            //    entry.localDir = worldToLocal.MultiplyVector(entry.direction);
            //    Ray ray = new Ray(localOrigin, entry.localDir);

            //    if (bounds.IntersectRay(ray, out distance))
            //    {
            //        Vector3 localPos = ray.GetPoint(distance);
            //        entry.volumePos = volume.objectToVolumeTrx.MultiplyPoint(localPos);
            //        entry.localDistance = distance;
            //        entry.active = true;
            //    }
            //    else
            //        entry.active = false;
            //}
            ////Debug.Log("hit " + distance);
            ////Debug.Log("loc " + localPos);
            ////Debug.Log("vol " + volumePos);

            if (collider.Raycast(request) > 0)
            {
                return true;
            }
            //if (collider.Raycast(volumePos, localDir, ref volumeDistance, ref surfaceNormal))
            //{
            //    // volumeDistance is in volumeDistance
            ////     Debug.Log("hit " + volumeDistance + " normal = " + surfaceNormal);

            //    hitInfo.volumePoint = volumePos + direction * volumeDistance;
            //    hitInfo.point = origin + (direction * (distance + volumeDistance * volume.resolution.x));
            //    hitInfo.normal = surfaceNormal;
            //    hitInfo.colliderVolume = collider;
            //    return true;
            //}
            else
                return false;
        }

     
        public static bool RaycastSpherical(DFVolumeCollider collider, Vector3 origin, out VolumeRaycastRequest req)
        {

            //GameObject go = collider.proxyGameObject;
            //if (!go) return 0;
            //MeshRenderer proxyRenderer = go.GetComponent<MeshRenderer>();
            //Vxcm.VXCMVolume volume = collider.GetComponent<DFVolumeFilter>().volume;

            //Matrix4x4 worldToLocal = proxyRenderer.transform.worldToLocalMatrix;
            //Bounds bounds = new Bounds(Vector3.zero, new Vector3(volume.resolution.x, volume.resolution.y, volume.resolution.z));

            //Vector3 localOrigin = worldToLocal.MultiplyPoint(origin);
            //Vector3 volumePos = volume.objectToVolumeTrx.MultiplyPoint(localOrigin);

            return collider.RaycastSpherical(origin, out req)>0;
           
        }
    }
}
