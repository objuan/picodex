
using UnityEngine;
using System.Collections;
using System;
using UnityEditor;


using Picodex;
using Picodex.Vxcm;


[CustomEditor(typeof(WormTestBuilder))]
public class WormTestBuilderEditor : Editor {

    private static bool m_editMode = false;
    WormTestBuilder builder;

    void OnSceneGUI()
    {

        //if (m_editMode)
        //{
        //    if (Event.current.type == EventType.MouseDown)
        //    {
        //    }
        //    if (Event.current.type == EventType.MouseUp)
        //    {
        //        WormTestBuilder builder = this.target as WormTestBuilder;
        //        DFVolumeRenderer volumeRenderer = builder.gameObject.GetComponent<DFVolumeRenderer>();

        //        Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        //        VolumeRaycastHit hit;

        //        // pick il volume interessato
        //        if (Picodex.Vxcm.Volume.Raycast(volumeRenderer, worldRay.origin, worldRay.direction, out hit))
        //        {

                    
        //          }
        //    }
        //    Event.current.Use();

        //}

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

                builder = this.target as WormTestBuilder;
            }
        }

        if (GUILayout.Button("Reset"))
        {
            builder.Build();

        }

    }

}
