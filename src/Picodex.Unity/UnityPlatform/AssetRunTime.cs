using System.Collections.Generic;

using UnityEngine.SceneManagement;

namespace UnityEngine
{
 
    //  classe per conservare il link tra le istanze della scena e l'asset originarion
    public class AssetRuntime 
    {
        class Entry
        {
            public Object obj;
            public string assetPath = "";
        }

        static Dictionary<string, string> nameMap = new Dictionary<string, string>();
        static Dictionary<int, Entry> instanceMap = new Dictionary<int, Entry>();

        public static string GetAssetPath(int instanceID)
        {
            if (instanceMap.ContainsKey(instanceID))
                return instanceMap[instanceID].assetPath;
            return "";
        }

        public static string GetUniqueName(string baseName)
        {
            int idx=0;
            string name;
            do
            {
                name = baseName + idx;
                idx++;
            }
            while (nameMap.ContainsKey(name));
            return name;
        }

        // 
        public static Mesh CreateMesh(string assetPath,Axiom.Core.Mesh _mesh)
        {
            Mesh mesh = new Mesh();

            Axiom.Core.Entity entity = Scene.current._sceneManager.CreateEntity(GetUniqueName(_mesh.Name), _mesh);

            mesh._meshView.RenderableObject.MovableObject = entity;

            Entry entry = new Entry();
            entry.obj = mesh;
            entry.assetPath = assetPath;
            instanceMap.Add(mesh.GetInstanceID(), entry);

            return mesh;
        }

        //internal static Imp.RenderableObject CreatePrefab(string assetPath, Axiom.Core.Mesh _mesh)
        //{
        //    Mesh mesh = new Mesh();

        //    Axiom.Core.Entity entity = Scene.current._sceneManager.CreateEntity(GetUniqueName(_mesh.Name), _mesh);

        //    // mesh._renderable.Load(_mesh);

        //    return new Imp.RenderableObject(entity);
        //}
    }
}
