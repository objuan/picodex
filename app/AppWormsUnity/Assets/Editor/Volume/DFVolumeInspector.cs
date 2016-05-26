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

        int lod = 1;
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

                DFVolume volume = null;
                //make sure to only show the interface
                DFVolumeFilter comp = obj.GetComponent<DFVolumeFilter>();
                if (comp != null)
                {
                    volume = comp.volume;
                }
                if (volume)
                { 
                    //comp.Intensity = EditorGUI.FloatField(
                    //    new Rect(5, 30, position.width - 10, 16),
                    //    "Intensity",
                    //    comp.Intensity
                    //);

               
                    EditorGUILayout.BeginHorizontal();

                    res = EditorGUILayout.Vector3Field("resolution", res);

                    if (GUILayout.Button("Clear"))
                    {
                        // obj.AddComponent<VXCMVolume>();
                        volume.Clear();

                        UnityUtil.InvalidateObject(obj);
                        AssetManager.SaveAsset(volume);
                    }
                    if (GUILayout.Button("Invalidate"))
                    {
                        // obj.AddComponent<VXCMVolume>();
                        volume.Invalidate();

                        UnityUtil.InvalidateObject(obj);
                        AssetManager.SaveAsset(volume);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button( "Test Build"))
                    {
                        VolumePrimitive raster = new VolumePrimitive(volume);

                        GeometrySample sample = new GeometrySample();
                        sample.debugColor = new Vector3(1, 0, 0);
                        //raster.Raster(new Vector3(0, 0, 0), 10, sample);

                        raster.RasterSphere(new Vector3(-5, 0, 0), 10, sample);
                        raster.RasterSphere(new Vector3(5, 0, 0), 10, sample);

                        UnityUtil.InvalidateObject(obj);
                        AssetManager.SaveAsset(volume);
                    }
                    if (GUILayout.Button("Test Build1"))
                    {
                        VolumePrimitive raster = new VolumePrimitive(volume);

                        GeometrySample sample = new GeometrySample();
                        sample.debugColor = new Vector3(1, 0, 0);
                        //raster.Raster(new Vector3(0, 0, 0), 10, sample);

                        raster.RasterBox(new Vector3(10, 10, 10), new Vector3(20, 20, 20), sample);

                        UnityUtil.InvalidateObject(obj);
                        AssetManager.SaveAsset(volume);
                    }

                    EditorGUILayout.EndHorizontal();

                    // ===============

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Build Mesh"))
                    {
                        VXCMMeshBuilder.CreateMesh(volume, new Vector3i(64, 64, 64));
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    lod = EditorGUILayout.IntSlider("lod",lod, 1, 4);
                
                    if (GUILayout.Button("Build Mesh Trans"))
                    {
                        VXCMMeshBuilder.CreateMeshTransvoxel(volume, new Vector3i(64, 64, 64), lod);
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
