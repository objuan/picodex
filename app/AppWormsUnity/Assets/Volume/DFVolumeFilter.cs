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
    [AddComponentMenu("Vxcm/DFVolumeFilter")]
    [ExecuteInEditMode]
    public class DFVolumeFilter : MonoBehaviour
    {
        [SerializeField]
        public DFVolume volume = null;


        //void FixedUpdate()
        //{
        //    if (volume.lastFrameChanged)
        //    {
        //        volume.lastFrameChanged = false;

        //    }
        //}
        // pulisco a fine frame
        //void OnRenderImage(RenderTexture src, RenderTexture dest) 
        //{
        //    if (volume)
        //        volume.lastFrameChanged = false;
        //}
    }

}