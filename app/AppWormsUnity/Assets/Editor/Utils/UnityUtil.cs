
using UnityEditor;

namespace Picodex
{
    public static class UnityUtil
    {
        public static void InvalidateObject(UnityEngine.Object obj)
        {
            EditorUtility.SetDirty(obj);
            SceneView.RepaintAll();
        }

        public static string GetUniqueAssetPathNameOrFallback(string filename)
        {
            string path;
            try
            {
                // Private implementation of a filenaming function which puts the file at the selected path.
                System.Type assetdatabase = typeof(UnityEditor.AssetDatabase);
                path = (string)assetdatabase.GetMethod("GetUniquePathNameAtSelectedPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(assetdatabase, new object[] { filename });
            }
            catch
            {
                // Protection against implementation changes.
                path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/" + filename);
            }
            return path;
        }

        public static bool IsAssetAFolder(UnityEngine.Object obj)
        {
            string path = "";

            if (obj == null)
            {
                return false;
            }

            path = AssetDatabase.GetAssetPath(obj);

            if (path.Length > 0)
            {
                if (System.IO.Directory.Exists(path))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
