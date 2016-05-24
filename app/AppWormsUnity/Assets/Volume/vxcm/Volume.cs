using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Internal;

namespace Picodex.Vxcm
{
    class VolumeCollection 
    {
        public List<DFVolumeRenderer> volumeList = new List<DFVolumeRenderer>();

        public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            hitInfo = new RaycastHit();
            Ray ray = new Ray(origin, direction);
            float distance;
            foreach (DFVolumeRenderer volumeRenderer in volumeList)
            {
                GameObject go = volumeRenderer.proxyGameObject;
                MeshRenderer proxyRenderer = go.GetComponent<MeshRenderer>();
                Bounds bounds = proxyRenderer.bounds;

                if (bounds.IntersectRay(ray, out distance))
                {
                    Vector3 p = ray.GetPoint(distance);

                    VXCMVolume volume = volumeRenderer.volume;

                    // from world to volume coordinate
                    Vector3 localPos = proxyRenderer.worldToLocalMatrix.MultiplyPoint( p);
                    Vector3 volumePos = volume.objectToVolumeTrx.MultiplyPoint(localPos);
                    Vector3 dir = direction;// localPos - origin;
                    Debug.Log("hit " + distance);
                    Debug.Log("loc " + localPos);
                    Debug.Log("vol " + volumePos);

                    if (go.GetComponent<VXCMObject_v02>().Raycast(volumePos, dir, ref distance)){
                        hitInfo.point = localPos + (dir * distance) * volume.volumeToObjectScale.x;
                        return true;
                    }
                }
            }
            return false;
        }

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

        public static bool Raycast(Ray ray, out RaycastHit hitInfo)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Ray ray, float maxDistance)
        {
            RaycastHit hitInfo = new RaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction)
        {
            RaycastHit hitInfo = new RaycastHit();
            return Raycast(origin, direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
        {
            return Raycast(origin, direction, out hitInfo, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }
      
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }
      
        public static bool Raycast(Ray ray, float maxDistance, int layerMask)
        {
            RaycastHit hitInfo = new RaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance)
        {
            RaycastHit hitInfo = new RaycastHit();
            return Raycast(origin, direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Ray ray, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            RaycastHit hitInfo = new RaycastHit();
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }


        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
        {
            return Raycast(origin, direction, out hitInfo, maxDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
        {
            RaycastHit hit = new RaycastHit();
            return Raycast(origin, direction, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal);
        }
        public static bool Raycast(Ray ray, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            return Raycast(origin, direction, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            RaycastHit hit = new RaycastHit();
            return Raycast(origin, direction, out hit , maxDistance, layerMask, queryTriggerInteraction);
        }

        // -------------------
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
        {
            return volumeCollection.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
        }

    }
}
