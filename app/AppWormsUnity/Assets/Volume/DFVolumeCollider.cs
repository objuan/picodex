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
 

    [AddComponentMenu("Vxcm/DFVolumeCollider")]
    [ExecuteInEditMode]
    public class DFVolumeCollider : MonoBehaviour
    {
        class ShaderHit
        {
            public VectorInputComputeBuffer originBuffer;
            public VectorInputComputeBuffer dirBuffer;
            public VectorOutputComputeBuffer outBuffer;
            public StdComputeShader compute = null;

            public void Start(int size)
            {
                if (compute == null)
                {
                    compute = new StdComputeShader("Vxcm/Pick/ray_v01");

                    originBuffer = new VectorInputComputeBuffer(size);
                    dirBuffer = new VectorInputComputeBuffer(size);
                    outBuffer = new VectorOutputComputeBuffer(originBuffer);
                }
                else {
                    originBuffer.Resize(size);
                    dirBuffer.Resize(size);
                    outBuffer.OnInputResize();
                }
            }

            public void Execute(GameObject proxyGameObject, DFVolume volume)
            {
                // load shader
                compute.material.SetTexture("_DirBuffer", dirBuffer.texture);

                // volume params
                compute.material.SetTexture("_Volume", proxyGameObject.GetComponent<VXCMObject>().texture);
                compute.material.SetFloat("DF_MIN", volume.distanceFieldRangeMin);
                compute.material.SetFloat("DF_MAX_MINUS_MIN", volume.distanceFieldRangeMax - volume.distanceFieldRangeMin);

                //compute.material.SetMatrix("u_objectToVolumeTrx", volume.objectToVolumeTrx);
                //compute.material.SetMatrix("u_objectToVolumeInvTrx", volume.objectToVolumeTrx.inverse);

                compute.material.SetVector("u_textureRes", volume.resolutionInv);

                compute.material.SetInt("u_count", originBuffer.count);

                compute.Execute(originBuffer, outBuffer);
            }
        }

        // ====================================

        DFVolume volume;
   
        [System.NonSerialized]
        public Texture2D txt;// debug

        ShaderHit rayHit;
        ShaderHit sphericalHit;
        VolumeRaycastRequest sphericalRequest;
        ShaderHit semisphericalHit;
        VolumeRaycastRequest semisphericalRequest;

        GameObject _proxyGameObject = null;

        static Vector3[] sphericalProbe = null;
        static Vector3[] semisphericalProbe = null;

        public GameObject proxyGameObject
        {
            get
            {
                if (_proxyGameObject == null)
                {
                    if (gameObject.transform.childCount > 0)
                    {
                        // detach the child
                        _proxyGameObject = gameObject.transform.GetChild(0).gameObject;
                    }
                }
                return _proxyGameObject;
            }
        }

        //RenderTexture renderTexture;

        void Start()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            volume = GetComponent<DFVolumeFilter>().volume;

            rayHit = new ShaderHit();
   

            Volumetric.AddVolume(this);
        }

        void OnApplicationQuit()
        {
            Volumetric.RemoveVolume(this);
        }

        // in world coordinate
        public bool Raycast(Vector3 origin, Vector3 dir, ref float distance, ref Vector3 surfaceNormal)
        {
            VolumeRaycastRequest req = new VolumeRaycastRequest();
            VolumeRaycastRequestEntry e = req.AddRaycast(origin, dir);
            e.localDir = dir;
            e.volumePos = origin;
            if (Raycast(req) > 0)
            {
                VolumeRaycastHit hit = req.GetMinDistanceHit();
                distance = hit.distance;
                surfaceNormal = hit.normal;
                return true;
            }
            else
                return false;
        }

        // =========================================================

        private int _Raycast(VolumeRaycastRequest request, ShaderHit rayHit)
        {
            //if (!volume) return 0;

            // second pass
            List<VolumeRaycastRequestEntry> entryList = request.entryList;
            //  List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();
            //  request.FillActiveEntries(entryList);

            //if (entryList.Count == 0) return 0;

            //rayHit.Start(entryList.Count);

            //txt = rayHit.originBuffer.texture;

            // load params
            //for (int i = 0; i < entryList.Count; i++)
            //{
            //    rayHit.originBuffer.SetValue(i, entryList[i].volumePos);
            //    rayHit.dirBuffer.SetValue(i, entryList[i].localDir);
            //}

            rayHit.originBuffer.Load();
            rayHit.dirBuffer.Load();


            // execute
            rayHit.Execute(proxyGameObject, volume);

            // get out
            int hitCount = 0;
            for (int i = 0; i < entryList.Count; i++)
            {
                Color outColor = rayHit.originBuffer[i];
                if (outColor.a > 0)
                {
                    VolumeRaycastRequestEntry entry = entryList[i];
                    entry.hit.distance = outColor.a;
                    entry.hit.normal = new Vector3(outColor.r, outColor.g, outColor.b);

                   // Debug.Log("hh " + i + " " + entry.hit.distance + " normal = " + entry.hit.normal);

                    entry.hit.volumePoint = entry.volumePos + entry.localDir * entry.hit.distance;

                    entry.hit.point = entry.origin + (entry.direction * (entry.localDistance + entry.hit.distance * volume.resolution.x));

                    entry.hit.distance = (entry.localDistance + entry.hit.distance * volume.resolution.x);

                    entry.hit.colliderVolume = this;

                    hitCount++;
                }
            }
            return hitCount;
        }

        public int Raycast(VolumeRaycastRequest request)
        {
            if (!volume) return 0;

            // first pass, build volumePos ,localDir
          //  float distance = 0;
            Matrix4x4 worldToLocal = proxyGameObject.transform.worldToLocalMatrix;
            Bounds bounds = new Bounds(Vector3.zero, new Vector3(volume.resolution.x, volume.resolution.y, volume.resolution.z));

            foreach (VolumeRaycastRequestEntry entry in request.entryList)
            {
                Vector3 localOrigin = worldToLocal.MultiplyPoint(entry.origin);
                entry.localDir = worldToLocal.MultiplyVector(entry.direction);
              //  Ray ray = new Ray(localOrigin, entry.localDir);

               // if (bounds.IntersectRay(ray, out distance))
                {
                    //    Vector3 localPos = ray.GetPoint(distance);
                    entry.volumePos = volume.objectToVolumeTrx.MultiplyPoint(localOrigin);
                    entry.localDistance = 0;// distance;
                 //   entry.active = true;
                }
                //else
                //    entry.active = false;
            }

            // second pass
            List<VolumeRaycastRequestEntry> entryList = request.entryList;
          //  List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();
          //  request.FillActiveEntries(entryList);

            if (entryList.Count == 0) return 0;

            rayHit.Start(entryList.Count);

            txt = rayHit.originBuffer.texture;

            // load params
            for (int i = 0; i < entryList.Count; i++)
            {
                rayHit.originBuffer.SetValue(i, entryList[i].volumePos);
                rayHit.dirBuffer.SetValue(i, entryList[i].localDir);
            }

            return _Raycast(request, rayHit);
        }


        // ===============================================

     
        public int RaycastSpherical(Vector3 origin, out VolumeRaycastRequest request)
        {
            request = sphericalRequest;

            if (!volume) return 0;

            if (sphericalProbe==null)
            {
               
                Mesh mesh = new Mesh();
                PrimitiveHelper.CreateSphere(mesh, 1);
             //   Matrix4x4 trx = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);
             //   PrimitiveHelper.TrasformMesh(mesh, trx); // best detail on Z

                sphericalProbe = new Vector3[mesh.vertices.Length];
                for (var i = 0; i < mesh.vertices.Length; i++)
                    sphericalProbe[i] = mesh.vertices[i].normalized;
            }
            if (sphericalHit == null)
                sphericalHit = new ShaderHit();

            Matrix4x4 worldToLocal = proxyGameObject.transform.worldToLocalMatrix;

            Vector3 localPos = worldToLocal.MultiplyPoint(origin);
            Vector3 volumeOrigin = volume.objectToVolumeTrx.MultiplyPoint(localPos);
         //   Bounds bounds = new Bounds(Vector3.zero, new Vector3(volume.resolution.x, volume.resolution.y, volume.resolution.z));

            // for scan
            sphericalHit.Start(sphericalProbe.Length);

            // first time
            if (sphericalRequest == null)
            {
                sphericalRequest = new VolumeRaycastRequest();
                for (int i = 0; i < sphericalProbe.Length; i++)
                {
                    sphericalRequest.AddRaycast(volumeOrigin, sphericalProbe[i]);
                    sphericalHit.dirBuffer.SetValue(i, sphericalProbe[i]);
                }
            }
            VolumeRaycastRequestEntry entry;
            // update origin
            for (int i = 0; i < sphericalProbe.Length; i++)
            {
                entry = sphericalRequest.entryList[i];
                sphericalHit.originBuffer.SetValue(i, volumeOrigin);

                entry.origin = origin;
                entry.volumePos = volumeOrigin; ;
                entry.localDistance = 0;
                entry.active = true;
                entry.hit.colliderVolume = null;
            }

            return _Raycast(sphericalRequest, sphericalHit);

            //List<VolumeRaycastRequestEntry> entryList = sphericalRequest.entryList;

            //sphericalHit.originBuffer.Load();
            //sphericalHit.dirBuffer.Load();


            //// execute
            //sphericalHit.Execute(proxyGameObject, volume);

            //// get out
            //int hitCount = 0;
            //for (int i = 0; i < entryList.Count; i++)
            //{
            //    Color outColor = sphericalHit.originBuffer[i];
            //    if (outColor.a > 0)
            //    {
            //        entry = entryList[i];
            //        entry.hit.distance = outColor.a;
            //        entry.hit.normal = new Vector3(outColor.r, outColor.g, outColor.b);

            //        //  Debug.Log("hh " + i+" " + entry.hit.distance + " normal = " + entry.hit.normal);

            //        entry.hit.volumePoint = entry.volumePos + entry.localDir * entry.hit.distance;

            //        entry.hit.point = entry.origin + (entry.direction * (entry.localDistance + entry.hit.distance * volume.resolution.x));

            //        entry.hit.distance = (entry.localDistance + entry.hit.distance * volume.resolution.x);

            //        entry.hit.colliderVolume = this;

            //        hitCount++;
            //    }
            //}
            //return hitCount;
        }

        // ===============================================

        public int RaycastSemispherical(Vector3 origin, out VolumeRaycastRequest request)
        {
            request = semisphericalRequest;

            if (!volume) return 0;

            if (semisphericalProbe == null)
            {

                Mesh mesh = new Mesh();
                PrimitiveHelper.CreateSphere(mesh, 1);
                //   Matrix4x4 trx = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);
                //   PrimitiveHelper.TrasformMesh(mesh, trx); // best detail on Z

                semisphericalProbe = new Vector3[mesh.vertices.Length];
                for (var i = 0; i < mesh.vertices.Length; i++)
                    semisphericalProbe[i] = mesh.vertices[i].normalized;
            }
            if (semisphericalHit == null)
                semisphericalHit = new ShaderHit();

            Matrix4x4 worldToLocal = proxyGameObject.transform.worldToLocalMatrix;
            Vector3 localPos = worldToLocal.MultiplyPoint(origin);
            Vector3 volumeOrigin = volume.objectToVolumeTrx.MultiplyPoint(localPos);
         
            // for scan
            semisphericalHit.Start(semisphericalProbe.Length);

            // first time
            if (semisphericalRequest == null)
            {
                semisphericalRequest = new VolumeRaycastRequest();
                for (int i = 0; i < semisphericalProbe.Length; i++)
                {
                    semisphericalRequest.AddRaycast(volumeOrigin, semisphericalProbe[i]);
                    semisphericalHit.dirBuffer.SetValue(i, semisphericalProbe[i]);
                }
            }
            VolumeRaycastRequestEntry entry;
            // update origin
            for (int i = 0; i < semisphericalProbe.Length; i++)
            {
                entry = semisphericalRequest.entryList[i];
                semisphericalHit.originBuffer.SetValue(i, volumeOrigin);

                entry.origin = origin;
                entry.volumePos = volumeOrigin; ;
                entry.localDistance = 0;
                entry.active = true;
                entry.hit.colliderVolume = null;
            }

            return _Raycast(sphericalRequest, semisphericalHit);
        }
    }
}