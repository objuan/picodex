using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.Imp;

namespace UnityEngine
{
    public class Shader : Object
    {
        private static List<string> _shaderKeywords = new List<string>();

        internal ShaderMaterial shaderMaterial;

        // Static Variables
        //globalMaximumLOD Shader LOD level for all shaders. 

        //Variables
        public bool isSupported;// Can this shader run on the end-users graphics card? (Read Only) 
        public int maximumLOD ;//Shader LOD level for this shader. 
        public int renderQueue{get{return 0;}} //Render queue of this shader. (Read Only) 

       //   Static Functions

        //  Finds a shader with the given name. 
        public static Shader Find(string name)
        {
            return (Shader)Resources.Find(name,typeof(Shader));
        }

        // PropertyToID Gets unique identifier for a shader property name. 
        public static int PropertyToID(string id)
        {
            return 0;
        }

//        Static Functions
        // EnableKeyword Set a global shader keyword. 

        public static void EnableKeyword(string keyword)
        {
            _shaderKeywords.Add(keyword);
        }

      //  DisableKeyword Unset a global shader keyword. 

        public static void DisableKeyword(string keyword)
        {
            _shaderKeywords.Remove(keyword);
        }

        // IsKeywordEnabled Is global shader keyword enabled? 

        public static bool IsKeywordEnabled(string keyword)
        {
            return _shaderKeywords.Contains(keyword);
        }

//SetGlobalBuffer Sets a global compute buffer property for all shaders. 
//SetGlobalColor Sets a global color property for all shaders. 
//SetGlobalFloat Sets a global float property for all shaders. 
//SetGlobalInt Sets a global int property for all shaders. 
//SetGlobalMatrix Sets a global matrix property for all shaders. 
//SetGlobalTexture Sets a global texture property for all shaders. 
//SetGlobalVector Sets a global vector property for all shaders. 
//WarmupAllShaders Fully load all shaders to prevent future performance hiccups. 

        internal static Shader Parse(String striptFile)
        {
            return null;
        }
    }
}
