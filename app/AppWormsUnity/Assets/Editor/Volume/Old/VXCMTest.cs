using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    class VXCMTest
    {
        // TEST
        [MenuItem("Tools/TestImport")] //Add a menu item to the toolbar
        static void TestImport()
        {
            //VXCMObjectHeader objectHeader = new VXCMObjectHeader();
            //objectHeader.samplingRate =  4;
            //GameObject obj = GameObject.Find("AircraftFuselageBody"); //
            //Mesh selectedmesh = obj.GetComponent<MeshFilter>().sharedMesh;
            //Transform meshTrx = obj.transform;

            ////   selectedmesh = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);


            //VXCMContext.Instance.useContext();

            //VXCMVolume volume = VXCMContext.Instance.CreateVolume();
            //volume.ImportMesh(selectedmesh, meshTrx, objectHeader);

            //// save the txt

            //Texture3D txt = volume.CreateTexture();
            //txt.name = Path.GetFileNameWithoutExtension("texture test");

            //string path = "Assets/VXCM/Textures/" + txt.name + ".asset";

            //AssetDatabase.DeleteAsset(path);
            //AssetDatabase.CreateAsset(txt, path);

            //VXCMContext.Instance.freeContext();
        }
    }
}
