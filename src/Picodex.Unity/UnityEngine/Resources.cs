using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnityEngine
{
    public class ResourceEntry
    {
        public string name = "";
        public Object resource;
    }

    //public class ShaderResource : ResourceEntry
    //{
    //    public string name = "";
    //    public List<string> properties = new List<string>();
    //    public List<string> subshader = new List<string>();
    //    public List<string> passList = new List<string>();

    //    public Shader shader = null;
    //}

    public class Resources
    {

        enum StateParse
        {
            Out,
            Properties,
            SubShader,
            Pass
        }
        // leggo lo shader da disco
        //private static void LoadShader(string path,StreamReader shaderReader)
        //{
        //    ShaderResource entry = new ShaderResource();
        //    String line=null;
        //    String bulkLine;
        //    StateParse state = StateParse.Out;
        //    int stackCount=0;
        //    StringBuilder content = new StringBuilder();
        //    do
        //    {
        //        line = shaderReader.ReadLine();
        //        if (line != null)
        //        {
        //            if (state == StateParse.Out)
        //            {
        //                bulkLine = line.Trim().Replace("/t", "");
        //                if (bulkLine.StartsWith("Shader"))
        //                {
        //                    entry.name = line.Substring(7).Trim().Replace("\"", "");
        //                }
        //                else if (bulkLine.StartsWith("Properties"))
        //                {
        //                    state = StateParse.Properties;
        //                    stackCount = 0;
        //                    content.Length = 0;
        //                }
        //                else if (bulkLine.StartsWith("SubShader"))
        //                {
        //                     state = StateParse.SubShader;
        //                     stackCount = 0;
        //                     content.Length = 0;
        //                }
        //                else if (bulkLine.StartsWith("Pass"))
        //                {
        //                     state = StateParse.Pass;
        //                     stackCount=0;
        //                     content.Length = 0;
        //                }
        //            }
        //            else{
        //                if (state == StateParse.Properties)
        //                {
        //                    if (line.Contains("{")) stackCount++;
        //                    else if (line.Contains("}"))
        //                    {
        //                        stackCount--;
        //                        if (stackCount == 0) state = StateParse.Out;
        //                    }
        //                    else 
        //                        entry.properties.Add(line);
        //                }
        //                else if (state == StateParse.SubShader)
        //                {
        //                    bulkLine = line.Trim().Replace("/t", "");

        //                    if (line.Contains("{")) stackCount++;
        //                    else if (line.Contains("}"))
        //                    {
        //                        stackCount--;
        //                        if (stackCount == 0)
        //                            state = StateParse.Out;
        //                    }
        //                    else if (bulkLine.StartsWith("Pass")) state = StateParse.Pass;
        //                    else
        //                    {
        //                        if (stackCount>=1) 
        //                            entry.subshader.Add(line);
        //                    }
        //                }
        //                else if (state == StateParse.Pass)
        //                {
        //                    if (line.Contains("{")) stackCount++;
        //                    else if (line.Contains("}"))
        //                    {
        //                        stackCount--;
        //                        if (stackCount == 0)
        //                        {
        //                            entry.passList.Add(content.ToString());
        //                            state = StateParse.Out;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        content.Append(line);
        //                        content.Append("\n");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    while (line != null & !shaderReader.EndOfStream);

        //    entry.path = path;

        //    Debug.Log("RESOURCE:  Add Shader '" + entry.name + "'");
        //    resourceMap.Add(entry.name, entry);
        //}

        // ======================================================================

        static Dictionary<string, ResourceEntry> resourceMap = new Dictionary<string, ResourceEntry>();

        public static Object Load(string path)
        {
            //if (!Directory.Exists(path))
            //{
            //    path = UnityEngine.Platform.UnityContext.Singleton.AssetPath + "/" + path;
            //}
            //if (Directory.Exists(path))
            //{
            //    Debug.Log("RESOURCE:  Load folder '" + path + "'");

            //    foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
            //    {
            //        if (file.Extension == ".shader")
            //        {
            //            using (StreamReader reader = file.OpenText())
            //            {
            //                LoadShader(file.FullName.Replace("\\", "/"), reader);
            //            }
            //        }
            //    }
            //    foreach (DirectoryInfo dir in new DirectoryInfo(path).GetDirectories())
            //    {
            //        Load(dir.FullName.Replace("\\","/"));   
            //    }
            //}
            //else
            //    Debug.LogError("RESOURCE:  Load not found '" + path + "'");
            return null;
        }

        /*
         * Loads an asset stored at path in a Resources folder.
            Returns the asset at path if it can be found otherwise returns null.
         * Only objects of type T will be returned. 
         * The path is relative to any Resources folder inside the Assets folder of your project,
         * extensions must be omitted.
        */
        public static Object Load(string name, Type type)
        {
            if (type == typeof(Shader))
            {
                Axiom.Graphics.Material mat = ( Axiom.Graphics.Material) Axiom.Graphics.MaterialManager.Instance.GetByName(name);
                if (mat != null)
                {
                    ResourceEntry entry = new ResourceEntry();
                    entry.name = name;
                    Shader shader = new Shader();
                    entry.resource = shader;

                    resourceMap.Add(name, entry);

                    return shader;
                }
                //String content = LoadRawFile(path);
                //if (content != null)
                //    return Shader.Parse(content);
                //else
                //    return null;
            }
            return null;
        }

        //public static String LoadRawFile(string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        path = UnityEngine.Platform.UnityContext.Singleton.AssetPath + "/" + path;
        //    }
        //    if (File.Exists(path))
        //    {
        //        using (StreamReader fs = new StreamReader(path, true))
        //        {
        //            return fs.ReadToEnd();
        //        }
        //    }
        //}

        //FindObjectsOfTypeAll Returns a list of all objects of Type type. 
       
        //LoadAll Loads all assets in a folder or file at path in a Resources folder. 
        //LoadAsync Asynchronously loads an asset stored at path in a Resources folder. 
        //UnloadAsset Unloads assetToUnload from memory. 
        //UnloadUnusedAssets Unloads assets that are not used. 

        public static Shader Find(string name, Type type)
        {
            if (type == (typeof(Shader)))
            {
                if (!resourceMap.ContainsKey(name))
                   return (Shader)Load(name,type);
                else if (resourceMap.ContainsKey(name))
                {
                    ResourceEntry entry = (ResourceEntry)resourceMap[name];
                    //if (entry.shader == null)
                    //{
                    //    entry.shader = new Shader();
                    //    entry.shader.shaderMaterial = new UnityEngine.Imp.ShaderMaterial();
                    //    entry.shader.shaderMaterial.Create(entry);
                    //}
                    return (Shader)entry.resource;
                }
            }
            return null;
        }

    }
}
