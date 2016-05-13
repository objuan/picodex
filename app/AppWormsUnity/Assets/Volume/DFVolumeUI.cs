using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;

using Picodex.Vxcm;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Picodex
{
    public class DFVolumeUI 
    {
      
        // It seems that we need to implement this function in order to make the volume pickable in the editor.
        // It's actually the gizmo which get's picked which is often bigger than than the volume (unless all
        // voxels are solid). So somtimes the volume will be selected by clicking on apparently empty space.
        // We shold try and fix this by using raycasting to check if a voxel is under the mouse cursor?
        public static void OnDrawGizmos(GameObject volumeObj)
        {
            DFVolume data = volumeObj.GetComponent<DFVolumeFilter>().volume;
            if (data != null)
            {
                // Compute the size of the volume.
                int width = (data.region.max.x - data.region.min.x) + 1;
                int height = (data.region.max.y - data.region.min.y) + 1;
                int depth = (data.region.max.z - data.region.min.z) + 1;
                float offsetX = width / 2;
                float offsetY = height / 2;
                float offsetZ = depth / 2;

                // The origin is at the centre of a voxel, but we want this box to start at the corner of the voxel.
                Vector3 halfVoxelOffset = new Vector3(0.5f, 0.5f, 0.5f);

                // + new Vector3(offsetX, offsetY, offsetZ)
                // Draw an invisible box surrounding the olume. This is what actually gets picked.
                Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.9f);
                Gizmos.DrawWireCube(volumeObj.transform.position , new Vector3(width, height, depth)); //  - halfVoxelOffset
            }
        }

        // Public so that we can manually drive it from the editor as required,
        // but user code should not do this so it's hidden from the docs.
        /// \cond
        public void OnGUI()
        {
            // This code doesn't belong in Volume? There should
            // probably be one global copy of this, not one per volume.

            /*GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            string debugPanelMessage = "Cubiquity Debug Panel\n";
            if(isMeshSyncronized)
            {
                debugPanelMessage += "Mesh sync: Completed";
            }
            else
            {
                debugPanelMessage += "Mesh sync: In progress...";
            }
            GUILayout.Box(debugPanelMessage);
            GUILayout.EndArea();*/
        }
        /// \endcond

  
    }
}