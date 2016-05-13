using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    class DFVolumeInspector : EditorWindow
    {
        public static DFVolumeInspector window;
        private static GameObject obj;

        Vector3 res = new Vector3(64,64,64);

        [MenuItem("Tools/Volume Inspector")] //Add a menu item to the toolbar
        static void OpenWindow()
        {
            window = (DFVolumeInspector)EditorWindow.GetWindow(typeof(DFVolumeInspector)); //create a window
            window.titleContent.text = "Volume Inspector"; //set a window title
        }

        void OnGUI()
        {
            if (window == null)
                OpenWindow();

            if (Selection.activeGameObject != null)
            {
                //gets the object you currently have selected in the scene view
                obj = Selection.activeGameObject;
                GUILayout.Label("Current selected object: " + obj.name);

                //make sure to only show the interface
                DFVolume comp = obj.GetComponent<DFVolume>();
                if (comp != null)
                {
                    //comp.Intensity = EditorGUI.FloatField(
                    //    new Rect(5, 30, position.width - 10, 16),
                    //    "Intensity",
                    //    comp.Intensity
                    //);

               
                    EditorGUILayout.BeginHorizontal();

                    res = EditorGUILayout.Vector3Field("resolution", res);

                    if (GUILayout.Button( "Create Mesh"))
                    {
                        // obj.AddComponent<VXCMVolume>();
                    }

                    EditorGUILayout.EndHorizontal();

                }
            }
            else
            {
                if (GUI.Button(new Rect(5, 30, position.width - 10, position.height - 40),
                "Add VXCMVolume"))
                {
                   // obj.AddComponent<VXCMVolume>();
                }
            }
        }

        void Update()
        {
            Repaint();
        }
    }
}
