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

    public class VXCMPostprocessor : AssetPostprocessor
    {
        static String rootPath = "Assets/VXCM";

        public static Texture3D LoadVolume_VXCM(string assetVol)
        {
            //VXCMContext.Instance.useContext();

            //VXCMVolume volume = VXCMContext.Instance.CreateVolumeFromAsset(assetVol);

            ////Bounds volumeRegion = vol.header.VolumeRegion;

            //Texture3D txt = volume.CreateTexture();
            ////   txt.Apply();

            ////  txt.name = UnityEngine.Random.value.ToString();
            //txt.name = Path.GetFileNameWithoutExtension(assetVol);

            //string path = Path.GetDirectoryName(assetVol) + "/"+ txt.name+".asset";

            //AssetDatabase.DeleteAsset(path);

            //AssetDatabase.CreateAsset(txt, path);

            //// clear the original
            //AssetDatabase.DeleteAsset(assetVol);

            //VXCMContext.Instance.freeContext();

            //return txt;
            return null;

        }

        public static void CreatePrefab(string assetVol)
        {
            String name = Path.GetFileNameWithoutExtension(assetVol);

            //Create temporary game object with required components
            GameObject go = new GameObject(name);
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = go.AddComponent< MeshRenderer>();

            if (AssetDatabase.FindAssets(rootPath + "/Materials/" + name + " Material").Length > 0)
            {
                AssetDatabase.DeleteAsset(rootPath + "/Materials/" + name + " Material");
            }
            if (AssetDatabase.FindAssets(rootPath + "/Prefabs/" + name + ".prefab").Length > 0)
            {
                AssetDatabase.DeleteAsset(rootPath + "/Prefabs/" + name + ".prefab");
            }

            //Create material as an asset
            Material mat = new Material(Shader.Find("Vxcm/Texture3D/slice_v01"));
            mat.mainTexture = LoadVolume_VXCM(assetVol);
            mat.name = name+ " Material";// texture.name;

     
            AssetDatabase.CreateAsset(mat, rootPath + "/Materials/" + mat.name + ".mat");

            //Assign material and model
            meshRenderer.material = mat;
            //meshFilter.mesh = AssetDatabase.LoadAssetAtPath(rootPath +"/Models/box.fbx", typeof(Mesh)) as Mesh;

            //CreateCubeUVW.createCube(meshFilter);
            PrimitiveHelper.CreateCube(meshFilter.sharedMesh, 1, 1, 1);
            
         
            //Assign tag
            // if (!string.IsNullOrEmpty(tag)) go.tag = tag;

            //Create prefab
            PrefabUtility.CreatePrefab(rootPath+"/Prefabs/" + name + ".prefab", go);

            //Destroy GameObject
            GameObject.DestroyImmediate(go);
        }

        public static void CreatePrefab1(string assetVol)
        {
            String name = Path.GetFileNameWithoutExtension(assetVol);

            //Create temporary game object with required components
            GameObject go = new GameObject(name);

            VXCMObject_v02 txt3D = go.AddComponent<VXCMObject_v02>();

            //   Texture3D t = CreateTexture3D(assetVol);
            Texture3D t = LoadVolume_VXCM(assetVol);

            txt3D.texture = t;

            //Create prefab
            String path = rootPath + "/Prefabs/" + name + ".prefab";

            AssetDatabase.DeleteAsset(path);

            PrefabUtility.CreatePrefab(path, go);

            //Destroy GameObject
            GameObject.DestroyImmediate(go);

            // cancello il file originatio

            AssetDatabase.DeleteAsset(assetVol);
        }


   

        // [MenuItem("Test/CreateTexture()")]
        public static Texture3D CreateTexture3D(string assetVol)
        {
            int size = 128;
            Texture3D txt = new Texture3D(size, size, size, TextureFormat.ARGB32, false);
            txt.filterMode = FilterMode.Point;

            var cols = new Color[size * size * size];
            float mul = 1.0f / (size - 1);
            int idx = 0;
            Color c = Color.white;
            for (int z = 0; z < size; ++z)
            {
                for (int y = 0; y < size; ++y)
                {
                    for (int x = 0; x < size; ++x, ++idx)
                    {
                        if ((((x + y + z) % 2) != 0))
                        {
                            c.r = ((x % 2) != 0) ? x * mul : 0;
                            c.g = ((y % 2) != 0) ? y * mul : 0;
                            c.b = ((z % 2) != 0) ? z * mul : 0;
                            c.a = 1;
                        }
                        else
                            c = new Color(0, 0, 0, 0);

                        cols[idx] = c;
                    }
                }
            }
            txt.SetPixels(cols);
            txt.Apply();

            txt.name = Path.GetFileNameWithoutExtension(assetVol);

            // test.name = UnityEngine.Random.value.ToString();

            string path = rootPath + "/Textures/" + txt.name + ".asset";

            AssetDatabase.DeleteAsset(path);

            AssetDatabase.CreateAsset(txt, path);

            return txt;
            //byte[] pngTex = test.EncodeToPNG();
            //File.WriteAllBytes(path, pngTex);
            //Texture2D.DestroyImmediate(test);

            //// Overwrite import settings
            //AssetDatabase.ImportAsset(path);
            //TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
            //ti.textureType = TextureImporterType.Advanced;
            //ti.filterMode = FilterMode.Point;
            //ti.textureFormat = TextureImporterFormat.AutomaticTruecolor;

            //AssetDatabase.WriteImportSettingsIfDirty(path);
            //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// Handles when ANY asset is imported, deleted, or moved.  Each parameter is the full path of the asset, including filename and extension.
        /// </summary>
        /// <param name="importedAssets">The array of assets that were imported.</param>
        /// <param name="deletedAssets">The array of assets that were deleted.</param>
        /// <param name="movedAssets">The array of assets that were moved.  These are the new file paths.</param>
        /// <param name="movedFromPath">The array of assets that were moved.  These are the old file paths.</param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            foreach (string asset in importedAssets)
            {
                string extension = Path.GetExtension(asset);
                if (extension.ToLower() == ".vxcm")
                {
                    LoadVolume_VXCM(asset);
                }
            }

            foreach (string asset in deletedAssets)
            {
                if (asset.ToLower().EndsWith(".vxcm"))
                {
                    Debug.Log("Deleted: " + asset);
                }
            }

            for (int i = 0; i < movedAssets.Length; i++ )
            {
                // Debug.Log("Moved: from " + movedFromPath[i] + " to " + movedAssets[i]);
            }
        }
    }
}
