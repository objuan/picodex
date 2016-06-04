
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;


using Picodex;
using Picodex.Vxcm;

namespace Picodex
{
    [CustomEditor(typeof(DFVolumeModder))]
    public class DFVolumeModderEditor : DFVolumeEditor
    {
        private static bool m_editMode = false;

        void OnEnable()
        {
        
        }

        void OnSceneGUI()
        {
            if (m_editMode)
            {
                //if (Event.current.type == EventType.MouseDown)
                //{
                //}
                if (Event.current.type == EventType.MouseDown)
                {
                    DFVolumeModder builder = this.target as DFVolumeModder;
                    DFVolumeCollider volumeCollider = builder.gameObject.GetComponent<DFVolumeCollider>();

                    if (volumeCollider)
                    {
                        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        VolumeRaycastHit hit;

                        // pick il volume interessato
                        if (Picodex.Volumetric.Raycast(volumeCollider, worldRay.origin, worldRay.direction, out hit))
                        {

                            //Debug.Log("HIT");
                            ((WormTestBuilder)target).AddObstacle(hit.point);

                        }
                    }
                }
                Event.current.Use();
            }
        }

        public override void OnInspectorGUI()
        {
            DFVolumeModder modder = ((DFVolumeModder)target);

            //  serializedObject.Update();

            EditorGUIHelper.ObjectFields(serializedObject);
  
            //EditorGUILayout.PropertyField(ray, new GUIContent("Ray"));

            //if (m_editMode)
            //{
            //    if (GUILayout.Button("Disable Editing"))
            //    {
            //        m_editMode = false;
            //    }
            //}
            //else
            //{
            //    if (GUILayout.Button("Enable Editing"))
            //    {
            //        m_editMode = true;
            //    }
            //}

            if (GUILayout.Button("Reset"))
            {
                modder.Clear();
            }
            if (GUILayout.Button("Save"))
            {
                SaveVolume(); // SaveVolume to asset
            }

            // end
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }

}