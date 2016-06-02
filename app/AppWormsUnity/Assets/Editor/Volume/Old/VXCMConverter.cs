using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    class VXCMConverter : EditorWindow
    {
        public static VXCMConverter window;
        private static GameObject obj;

        string textureName = "Texture Name";
     //   bool assetDestEnabled;

     //   Mesh old_selectedmesh = null;
        Mesh selectedmesh = null;
        Transform meshTrx;
        string selectedAsset = "";

        VXCMVolumeDef objectHeader = new VXCMVolumeDef();


        [MenuItem("Tools/Volume Converter")] //Add a menu item to the toolbar
        static void OpenWindow()
        {
            window = (VXCMConverter)EditorWindow.GetWindow(typeof(VXCMConverter)); //create a window
            window.titleContent.text = "Volume Converter"; //set a window title
        }

        void OnGUI()
        {
            if (window == null)
                OpenWindow();

            //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            ////GUILayout.Label("aaaaa", EditorStyles.label);
            ////GUILayout.Label("aaaaa", EditorStyles.largeLabel);
            ////GUILayout.Label("aaaaa", EditorStyles.objectField);
            ////GUILayout.Label("aaaaa", EditorStyles.textArea);
            ////GUILayout.Label("aaaaa", EditorStyles.whiteLabel);
            ////GUILayout.Label("aaaaa", EditorStyles.wordWrappedLabel);
            //textureName = EditorGUILayout.TextField("Text Field", textureName);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();

            //EditorGUILayout.BeginVertical();
            //GUILayout.Label("Current Selection:", EditorStyles.boldLabel);

            //EditorGUILayout.EndVertical();

            ////if (GUI.Button(new Rect(0, 0, position.width, position.height), "Press me"))
            ////{
            ////    Debug.Log("hodor");
            ////}

            selectedmesh = null;
            if (Selection.activeGameObject != null)
            {
                //gets the object you currently have selected in the scene view
                obj = Selection.activeGameObject;
                //GUI.Label(new Rect(0, 0, position.width, 25), "Current selected object: " + obj.name);

                if (obj.GetComponent<MeshFilter>() != null)
                {
                    selectedmesh = obj.GetComponent<MeshFilter>().sharedMesh;
                    meshTrx = obj.transform;
                }
            }

            //if (old_selectedmesh != selectedmesh)
            //{
            //    old_selectedmesh =selectedmesh;
            //    textureName = selectedmesh.name;
            //}

            selectedAsset = "";

            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            foreach (UnityEngine.Object obj in selectedAssets)
            {
                if (UnityUtil.IsAssetAFolder(obj))
                {
                    selectedAsset = AssetDatabase.GetAssetPath(obj);
                    break;
                }
            }

            // ======================================


            //    // if (GUI.Button(new Rect(5, 30, position.width - 10, position.height - 40), "Create Texture"))
            //    if (GUILayout.Button( "Create Texture3D"))
            //    {
            //        //obj.AddComponent<VXCMObject>();
            //    }
            //    if (GUILayout.Button("Test"))
            //    {
            //        //ObjectSelectorWrapper.ShowSelector(typeof(Texture3D));
            //    }
            //}
            //else
            //{
            //    //if (GUI.Button(new Rect(5, 30, position.width - 10, position.height - 40),
            //    //"Add VXCMObject"))
            //    //{
            //    //    obj.AddComponent<VXCMObject>();
            //    //}
            //}

            // GUI

            GUILayout.Label("Select a mesh", EditorStyles.boldLabel);


            if (selectedmesh != null)
            {
                Vector3 size = Vector3.Scale( meshTrx.lossyScale, selectedmesh.bounds.size);

               String meshInfo = "dimensions: " + size.x + "," + size.y + "," + size.z + "\n----------";

                GUILayout.Label("Current selected object: " + obj.name, EditorStyles.objectField);
                GUILayout.Label(meshInfo, EditorStyles.helpBox);

                // opsioni


            }
            else
                GUILayout.Label("[please select one]", EditorStyles.label);


            GUILayout.Label("Select a destination", EditorStyles.boldLabel);

            if (selectedAsset != "")
            {
                GUILayout.Label(selectedAsset, EditorStyles.label);
                
            }
            else
            {
                GUILayout.Label("[please select one]", EditorStyles.label);
            }

            if (selectedAsset != "" && selectedmesh != null)
            {
                GUILayout.Label("Build Params", EditorStyles.boldLabel);

                textureName = EditorGUILayout.TextField("Name", textureName);

                objectHeader.name = textureName;
                objectHeader.gradientMin = EditorGUILayout.Slider("Gradient Min", objectHeader.gradientMin, -3, -1);
                objectHeader.gradientMax = EditorGUILayout.Slider("Gradient Max", objectHeader.gradientMax, 1, 3);
                objectHeader.distanceFieldMax = EditorGUILayout.Slider("Distance Field Max", objectHeader.distanceFieldMax, 2, 8);
                objectHeader.samplingRate = EditorGUILayout.IntSlider("Sampling Rate", objectHeader.samplingRate, 1,8);

                // -------------

                GUILayout.Label("Volume", EditorStyles.boldLabel);

                Vector3 volumeSize = Vector3.Scale(meshTrx.lossyScale, selectedmesh.bounds.size);
                // add distance field 
                volumeSize += new Vector3(objectHeader.distanceFieldMax, objectHeader.distanceFieldMax, objectHeader.distanceFieldMax);
                Vector3.Scale(volumeSize, new Vector3(objectHeader.samplingRate, objectHeader.samplingRate, objectHeader.samplingRate));

                String volumeDesc = "Volume: " + volumeSize.x + "x" + volumeSize.y + "x" + volumeSize.z + "";

                Vector3i txtInfo = VXCMVolume.GetTextureInfo(volumeSize, objectHeader.samplingRate);

                volumeDesc+="\nTexture: " + +txtInfo.x + "x" + txtInfo.y + "x" + txtInfo.z + "";

                GUILayout.Label(volumeDesc, EditorStyles.label);

                if (GUILayout.Button("Build MC"))
                {
                    build();
                }

                //if (GUILayout.Button("Build Trans"))
                //{

                //    buildTrans();
                //}
            }

                //var text = new string[] { "Asset Folder:\n" + ((assetSelected != "") ? assetSelected : "[please select one]" ),"jjj"};
                //GUILayout.SelectionGrid(0, text, 1, EditorStyles.radioButton);

                // EditorGUILayout.BeginVertical();
                //assetDestEnabled = EditorGUILayout.BeginToggleGroup("Destination Asset Folder", assetDestEnabled);

                //if (assetSelected != "")
                //{
                //    //GUILayout.Label("Destination Asset Folder:", EditorStyles.boldLabel);
                //    GUILayout.Label(assetSelected, EditorStyles.label);

                //}
                //else
                //{
                //    GUILayout.Label("[please select one]", EditorStyles.label);
                //}

                //EditorGUILayout.EndToggleGroup();

                // GUILayout.SelectionGrid(0,text,count,"toggle");

                //foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
                //{
                //    path = AssetDatabase.GetAssetPath(obj);
                //    if (File.Exists(path))
                //    {
                //        path = Path.GetDirectoryName(path);

                //        GUILayout.Label(path, EditorStyles.helpBox);
                //    }
                //    break;
                //}

        }

        void Update()
        {
            Repaint();
        }

        void build()
        {
            //VXCMContext.Instance.useContext();

            //VXCMVolume volume = VXCMContext.Instance.CreateVolume();
            //volume.ImportMesh(selectedmesh, meshTrx, objectHeader);

            //// save the txt

            //Texture3D txt = volume.CreateTexture();
            //txt.name = Path.GetFileNameWithoutExtension(textureName);

            //string path = selectedAsset + "/" + txt.name + ".asset";

            //AssetDatabase.DeleteAsset(path);
            //AssetDatabase.CreateAsset(txt, path);

            //VXCMContext.Instance.freeContext();

        }
    }
}
