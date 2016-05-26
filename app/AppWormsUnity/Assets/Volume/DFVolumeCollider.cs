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
        VXCMObject_v02 vxcmObject;
           
        [System.NonSerialized]
        public Texture2D txt;// debug

        VectorInputComputeBuffer originBuffer;
        VectorInputComputeBuffer dirBuffer;
        VectorOutputComputeBuffer outBuffer;
        StdComputeShader compute = null;

        RenderTexture renderTexture;

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

            proxyGameObject = GetComponent<DFVolumeRenderer>().proxyGameObject;

            vxcmObject = proxyGameObject.GetComponent<VXCMObject_v02>();

            compute = null;
        }

     
        public bool Raycast(Vector3 origin, Vector3 dir, ref float distance,ref Vector3 surfaceNormal)
        {
            if (!vxcmObject) return false;

            if (compute==null)
            {
                compute = new StdComputeShader("Vxcm/Pick/ray_v01");

                originBuffer = new VectorInputComputeBuffer(1);
                dirBuffer = new VectorInputComputeBuffer(1);
                outBuffer = new VectorOutputComputeBuffer(originBuffer);

                // debug
                txt = originBuffer.texture;
                renderTexture = outBuffer.renderTexture;

            }

            // load params
            originBuffer.SetValue(0, origin);
            dirBuffer.SetValue(0, dir);

            originBuffer.Load();
            dirBuffer.Load();

            
            // load shader
            compute.material.SetTexture("_DirBuffer", dirBuffer.texture);

            // volume params
            compute.material.SetTexture("_Volume", vxcmObject.texture);
            compute.material.SetFloat("DF_MIN", volume.distanceFieldRangeMin);
            compute.material.SetFloat("DF_MAX_MINUS_MIN", volume.distanceFieldRangeMax - volume.distanceFieldRangeMin);

            //compute.material.SetMatrix("u_objectToVolumeTrx", volume.objectToVolumeTrx);
            //compute.material.SetMatrix("u_objectToVolumeInvTrx", volume.objectToVolumeTrx.inverse);

            compute.material.SetVector("u_textureRes", volume.resolutionInv);

            // execute
            compute.Execute(originBuffer, outBuffer);

            // get out
            Color outColor = originBuffer[0];
            if (outColor.a > 0)
            {
                distance = outColor.a;
                surfaceNormal.x = outColor.r;
                surfaceNormal.y = outColor.g;
                surfaceNormal.z = outColor.b;
                return true;
            }
            else
                return false;
        }

    }
}