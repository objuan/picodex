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

    [AddComponentMenu("Vxcm/DFVolumeCollider")]
    [ExecuteInEditMode]
    public class DFVolumeCollider : MonoBehaviour
    {
        DFVolume volume;
      //  GameObject proxyGameObject;
      //  VXCMObject vxcmObject;
           
        [System.NonSerialized]
        public Texture2D txt;// debug

        VectorInputComputeBuffer originBuffer;
        VectorInputComputeBuffer dirBuffer;
        VectorOutputComputeBuffer outBuffer;
        StdComputeShader compute = null;

        GameObject _proxyGameObject = null;

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

        //public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
        //{
        //    if (!volume)
        //    {
        //        hitInfo = new RaycastHit();
        //        return false;
        //    }
        //    hitInfo = new RaycastHit();
        //    return false;
        //    //
        //    //return volume.Raycast(ray,out hitInfo, maxDistance);
        //}

        void Start()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            volume = GetComponent<DFVolumeFilter>().volume;


            // buikd the mesh
            //
            //         Mesh mesh = new TransvoxelExtractor(new VXCMVolumeData(volume)).GenLodCell(new Vector3i(0, 0, 0), volume.resolution, 1);

            // proxyGameObject = GetComponent<DFVolumeRenderer>().proxyGameObject;

            // vxcmObject = proxyGameObject.GetComponent<VXCMObject>();

            Volume.AddVolume(this);

            compute = null;
        }

        void OnApplicationQuit()
        {
            Volume.RemoveVolume(this);
        }

        public bool Raycast(Vector3 origin, Vector3 dir, ref float distance, ref Vector3 surfaceNormal)
        {
            VolumeRaycastRequest req = new VolumeRaycastRequest();
            VolumeRaycastRequestEntry e = req.AddRaycast(origin, dir);
            e.localDir = dir;
            e.volumePos = origin;
            if (Raycast(req) >0)
            {
                VolumeRaycastHit hit = req.GetMinDistanceHit();
                distance = hit.distance;
                surfaceNormal = hit.normal;
                return true;
            }
            else
                return false;
        }

        public int Raycast(VolumeRaycastRequest request)
        {
            if (!volume) return 0;

            List<VolumeRaycastRequestEntry> entryList = new List<VolumeRaycastRequestEntry>();
            request.FillActiveEntries(entryList);


            if (entryList.Count == 0) return 0;
            if (compute == null)
            {
                compute = new StdComputeShader("Vxcm/Pick/ray_v01");

                originBuffer = new VectorInputComputeBuffer(1);
                dirBuffer = new VectorInputComputeBuffer(1);
                outBuffer = new VectorOutputComputeBuffer(originBuffer);
            }

            originBuffer.Resize(entryList.Count);
            dirBuffer.Resize(entryList.Count);
            outBuffer.OnInputResize();

            txt = originBuffer.texture;

            // load params
            for (int i = 0; i < entryList.Count; i++)
            {
                originBuffer.SetValue(0, entryList[i].volumePos);
                dirBuffer.SetValue(0, entryList[i].localDir);
            }

            originBuffer.Load();
            dirBuffer.Load();

            
            // load shader
            compute.material.SetTexture("_DirBuffer", dirBuffer.texture);

            // volume params
            compute.material.SetTexture("_Volume", proxyGameObject.GetComponent<VXCMObject>().texture);
            compute.material.SetFloat("DF_MIN", volume.distanceFieldRangeMin);
            compute.material.SetFloat("DF_MAX_MINUS_MIN", volume.distanceFieldRangeMax - volume.distanceFieldRangeMin);

            //compute.material.SetMatrix("u_objectToVolumeTrx", volume.objectToVolumeTrx);
            //compute.material.SetMatrix("u_objectToVolumeInvTrx", volume.objectToVolumeTrx.inverse);

            compute.material.SetVector("u_textureRes", volume.resolutionInv);

            // execute
            compute.Execute(originBuffer, outBuffer);

            // get out
            int hitCount = 0;
            for (int i = 0; i < entryList.Count; i++)
            {
                Color outColor = originBuffer[i];
                if (outColor.a > 0)
                {
                    VolumeRaycastRequestEntry entry = entryList[i];
                    entry.hit.distance = outColor.a;
                    entry.hit.normal = new Vector3(outColor.r, outColor.g, outColor.b);

                    ////     Debug.Log("hit " + volumeDistance + " normal = " + surfaceNormal);

                    entry.hit.volumePoint = entry.volumePos + entry.localDir * entry.hit.distance;

                  
                    entry.hit.point = entry.origin + (entry.direction * (entry.localDistance + entry.hit.distance * volume.resolution.x));

                    entry.hit.distance = (entry.localDistance + entry.hit.distance * volume.resolution.x);

                    entry.hit.colliderVolume = this;

                    hitCount++;
                }
            }
            return hitCount;
        }
            
    }
}