
using UnityEngine;

namespace UnityEditor
{
    public class AssetDatabaseEntry
    {
        public string name = "";
        public Object resource;
    }


    public class AssetDatabase
    {
        // Static

        //AddObjectToAsset Adds objectToAdd to an existing asset at path. 
        //AssetPathToGUID Get the GUID for the asset at path. 
        //ClearLabels Removes all labels attached to an asset. 
        //Contains Is object an asset? 
        //CopyAsset Duplicates the asset at path and stores it at newPath. 
        //CreateAsset Creates a new asset at path. 
        //CreateFolder Create a new folder. 
        //DeleteAsset Deletes the asset file at path. 
        //ExportPackage Exports the assets identified by assetPathNames to a unitypackage file in fileName. 
        
        // FindAssets Search the asset database using a search filter string. 
        /*
         * You can search for names, lables and types (classnames).
        'name': filter assets by their filename (without extension). Words separated by whitespace are treated as separate name searches. Use quotes for grouping multiple words into a single search.
        'labels': Use the keyword 'l'. Filtering by more than one label will return assets if just one asset label is matched (OR'ed)
        'types': Use the keyword 't'. Filtering by more than one type will return assets if just one type is matched (OR'ed). Types can be either builtin types e.g 'Texture2D' or user script class names. If all assets are wanted: use 'Object' as all assets derive from Object.
        */

        public static string[] FindAssets(string filter){

            Axiom.Core.ResourceGroupManager.Instance.FindResourceNames("Assets", filter);
            return null;
        }

        public static string[] FindAssets(string filter, string[] searchInFolders){
            return null;
        }


        //GenerateUniqueAssetPath Creates a new unique path for an asset. 
        //GetAllAssetBundleNames Return all the AssetBundle names in the asset database. 
        //GetAssetOrScenePath Returns the path name relative to the project folder where the asset is stored. 
       
        //GetAssetPath Returns the path name relative to the project folder where the asset is stored. 

        public static string GetAssetPath(int instanceID)
        {
            return AssetRuntime.GetAssetPath(instanceID);
        }

        public static string GetAssetPath(Object assetObject)
        {
            return AssetRuntime.GetAssetPath(assetObject.GetInstanceID());
        }


        //GetAssetPathFromTextMetaFilePath Gets the path to the asset file associated with a text .meta file. 
        //GetAssetPathsFromAssetBundle Get the paths of the assets which have been marked with the given assetBundle name. 
        //GetAssetPathsFromAssetBundleAndAssetName Get the asset paths from the given assetBundle name and asset name. 
        //GetCachedIcon Retrieves an icon for the asset at the given asset path. 
        //GetDependencies Given a pathName, returns the list of all assets that it depends on. 
        //GetLabels Returns all labels attached to a given asset. 
        //GetSubFolders Given an absolute path to a directory, this method will return an array of all it's subdirectories. 
        //GetTextMetaFilePathFromAssetPath Gets the path to the text .meta file associated with an asset. 
        //GetUnusedAssetBundleNames Return all the unused assetBundle names in the asset database. 
        
        //GUIDToAssetPath Translate a GUID to its current asset path. 
        //All paths are relative to the project folder, for example: "Assets/MyTextures/hello.png".

        public static string GUIDToAssetPath(string guid)
        {
            // assumo che guid sia il path nell'asset
            return guid;
        }
        //ImportAsset Import asset at path. 
        //ImportPackage Imports package at packagePath into the current project. 
        //IsForeignAsset Is asset a foreign asset? 
        //IsMainAsset Is asset a main asset in the project window? 
        //IsNativeAsset Is asset a native asset? 
        //IsOpenForEdit Use IsOpenForEdit to determine if the asset is open for edit by the version control. 
        //IsSubAsset Does the asset form part of another asset? 
        //IsValidFolder Given an absolute path to a folder, returns true if it exists, false otherwise. 
        //LoadAllAssetRepresentationsAtPath Returns all asset representations at assetPath. 
        //LoadAllAssetsAtPath Returns an array of all asset objects at assetPath. 

        //        LoadAssetAtPath Returns the first asset object of type type at given path assetPath. 
        /*
         * Some asset files may contain multiple objects. (such as a Maya file which may contain multiple Meshes and GameObjects). All paths are relative to the project folder, for example: "Assets/MyTextures/hello.png".
            Note:
            The assetPath parameter is not case sensitive.
            ALL asset names and paths in Unity use forward slashes, even on Windows.
            This returns only an asset object that is visible in the Project view. If the asset is not found LoadAssetAtPath returns Null.
            */
        public static Object LoadAssetAtPath(string assetPath, System.Type type)
        {
            Axiom.FileSystem.FileInfoList fileList =  Axiom.Core.ResourceGroupManager.Instance.FindResourceFileInfo("Assets", assetPath);
            if (fileList.Count > 0)
            {
                System.Collections.Generic.List<string> list = Axiom.Core.ResourceGroupManager.Instance.FindResourceNames("Assets", assetPath);
                if (list.Count > 0)
                {
                    if (type ==  typeof(Mesh))
                    {
                        Axiom.Core.Mesh _mesh = (Axiom.Core.Mesh)Axiom.Core.MeshManager.Instance.Load(list[0], "Assets");
                        return AssetRuntime.CreateMesh(assetPath,_mesh);
                    }
                }
            }
            return null;
            //Axiom.Core.ResourceManager.
           // Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/texture.jpg", typeof(Texture2D));
        }

        //LoadMainAssetAtPath Returns the main asset object at assetPath. 
        //MoveAsset Move an asset file from one folder to another. 
        //MoveAssetToTrash Moves the asset at path to the trash. 
        //OpenAsset Opens the asset with associated application. 
        //Refresh Import any changed assets. 
        //RemoveAssetBundleName Remove the assetBundle name from the asset database. The forceRemove flag is used to indicate if you want to remove it even it's in use. 
        //RemoveUnusedAssetBundleNames Remove all the unused assetBundle names in the asset database. 
        //RenameAsset Rename an asset file. 
        //SaveAssets Writes all unsaved asset changes to disk. 
        //SetLabels Replaces that list of labels on an asset. 
        //StartAssetEditing Begin Asset importing. This lets you group several asset imports together into one larger import. 
        //StopAssetEditing Stop Asset importing. This lets you group several asset imports together into one larger import. 
        //ValidateMoveAsset Checks if an asset file can be moved from one folder to another. (Without actually moving the file). 
        //WriteImportSettingsIfDirty Writes the import settings to disk. 

      
    }
}
