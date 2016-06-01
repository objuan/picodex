using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace Picodex
{ 
     [CustomEditor(typeof(DFVolumeFilter))]
     public class DFVolumeEditor : Editor
     {
        public DFVolumeFilter filter
        {
            get
            {
                return (target as MonoBehaviour).GetComponent<DFVolumeFilter>();
            }
        }

        public void SaveVolume()
        {
            if (!filter) return;

            UnityUtil.InvalidateObject(filter.gameObject);
            AssetManager.SaveAsset(filter.volume);
        }
    }
}
