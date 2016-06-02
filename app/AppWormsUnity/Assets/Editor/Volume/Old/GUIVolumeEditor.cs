using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    class GUIVolumeEditor : EditorWindow
    {
        public static GUIVolumeEditor window;
      //  private static GameObject obj;

      //  Vector3 res = new Vector3(64,64,64);

        //DFVolumeModder volumeEditor =null;

        [MenuItem("Tools/Volume Editor")] //Add a menu item to the toolbar
        static void OpenWindow()
        {
            window = (GUIVolumeEditor)EditorWindow.GetWindow(typeof(GUIVolumeEditor)); //create a window
            window.titleContent.text = "Volume Editor"; //set a window title
        }


        void OnGUI()
        {
            if (window == null)
                OpenWindow();

            if (Selection.activeGameObject != null)
            {
                //gets the object you currently have selected in the scene view
              //  obj = Selection.activeGameObject;
               // DFVolume volume = null;
                //make sure to only show the interface
            //    volumeEditor = obj.GetComponent<DFVolumeModder>();
                //if (volumeEditor != null)  volume = volumeEditor.volume;
    
                //if (volume)
                //{
                //    GUILayout.Label("Volume: " + obj.name);

                //    // GUI

                //    volumeEditor.mode = (VolumeEditor_Mode)EditorGUILayout.EnumPopup(volumeEditor.mode);

                //    volumeEditor.shape = (VolumeEditor_ShapeType)EditorGUILayout.EnumPopup(volumeEditor.shape);
                //    volumeEditor.ray = EditorGUILayout.Slider("Ray", volumeEditor.ray,1, 10);


                //    volumeEditor.matColor = EditorGUILayout.ColorField("Color", volumeEditor.matColor);
                //}
                //else
                //    GUILayout.Label("Please, select a Volume");

            }
        }

        void Update()
        {
            Repaint();
        }
    }
}
