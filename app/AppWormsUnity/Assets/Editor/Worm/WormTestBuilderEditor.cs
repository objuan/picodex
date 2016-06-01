
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;


using Picodex;
using Picodex.Vxcm;

namespace Picodex
{
    [CustomEditor(typeof(WormTestBuilder))]
    public class WormTestBuilderEditor : DFVolumeEditor
    {
        private static bool m_editMode = false;
        WormTestBuilder builder;

        void OnSceneGUI()
        {
            if (m_editMode)
            {
                //if (Event.current.type == EventType.MouseDown)
                //{
                //}
                if (Event.current.type == EventType.MouseDown)
                {
                    WormTestBuilder builder = this.target as WormTestBuilder;
                    DFVolumeRenderer volumeRenderer = builder.gameObject.GetComponent<DFVolumeRenderer>();

                    Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    VolumeRaycastHit hit;

                    // pick il volume interessato
                    if (Picodex.Vxcm.Volume.Raycast(volumeRenderer, worldRay.origin, worldRay.direction, out hit))
                    {

                        Debug.Log("HIT");
                    }
                }
                Event.current.Use();

            }

        }

        public override void OnInspectorGUI()
        {
            if (m_editMode)
            {
                if (GUILayout.Button("Disable Editing"))
                {
                    m_editMode = false;
                }
            }
            else
            {
                if (GUILayout.Button("Enable Editing"))
                {
                    m_editMode = true;
                }
            }

            if (GUILayout.Button("Reset"))
            {
                ((WormTestBuilder)target).Build();
            }
            if (GUILayout.Button("Save"))
            {
                SaveVolume(); // SaveVolume to asset
            }
        }

    }

}