
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
       // WormTestBuilder builder;

       // private SerializedProperty ray;

        void OnEnable()
        {
           // ray = serializedObject.FindProperty("ray");
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
                    WormTestBuilder builder = this.target as WormTestBuilder;
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
            WormTestBuilder builder = ((WormTestBuilder)target);

            EditorGUIHelper.ObjectFields(serializedObject);

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
                builder.Clear();
            }

            // end
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

    }

}