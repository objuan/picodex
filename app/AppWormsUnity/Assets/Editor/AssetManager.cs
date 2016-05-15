using UnityEngine;
using UnityEditor;

using System.Collections;
using System.IO;


//using Picodex.Unity.Vxcm;
using Picodex;
using Picodex.Vxcm;

namespace Picodex
{
    public class AssetManager
    {
        //public static VolumeDataType CreateFromVoxelDatabase<VolumeDataType>(string relativePathToVoxelDatabase) where VolumeDataType : DFVolumeData
        //{
        //    VolumeDataType data = DFVolumeData.CreateFromVoxelDatabase<VolumeDataType>(relativePathToVoxelDatabase);
        //    string assetName = Path.GetFileNameWithoutExtension(relativePathToVoxelDatabase);
        //    CreateAssetFromInstance<VolumeDataType>(data, assetName);
        //    return data;
        //}

        public static VolumeDataType CreateEmptyVolume<VolumeDataType>(Vector3i size) where VolumeDataType : DFVolume
        {
            //VolumeDataType data = DFVolumeData.CreateEmptyVolumeData<VolumeDataType>(size, PathUtils.GenerateRandomVoxelDatabaseName());
            //CreateAssetFromInstance<VolumeDataType>(data);
            //return data;
            return null;
        }

        // The contents of this method are taken/derived from here:
        // http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        public static void CreateAssetFromInstance<T>(T instance, string assetName = "") where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            if (assetName == "")
            {
                assetName = "New " + typeof(T).Name;
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + assetName + ".asset");

            AssetDatabase.CreateAsset(instance, assetPathAndName);

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = instance;
        }

        public static void SaveAsset(Object t)
       {
            EditorUtility.SetDirty(t);
            EditorApplication.SaveAssets();
            AssetDatabase.SaveAssets();
            }
    }
}
