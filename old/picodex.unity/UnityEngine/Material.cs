using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.Imp;

namespace UnityEngine
{/*
      Common color names used by Unity's builtin shaders: 
        "_Color" is the main color of a material. This can also be accessed via color property. 
        "_SpecColor" is the specular color of a material (used in specular/glossy/vertexlit shaders). 
        "_EmissionColor" is the emissive color of a material (used in vertexlit shaders). 
        "_ReflectColor" is the reflection color of the material (used in reflective shaders).
        */

    [Serializable]
    public class Material : Object
    {
        // internals
        Shader _shader;
        ShaderMaterial shaderMaterial;

        private List<string> _shaderKeywords = new List<string>();

        //  Variables

        public Color color = new Color(); //  The main material's color. 
        
        //globalIlluminationFlags Defines how the material should interact with lightmaps and lightprobes. 
        public Texture mainTexture;// The material's texture. 
        //mainTextureOffset The texture offset of the main texture. 
        //mainTextureScale The texture scale of the main texture. 
        public int passCount { get { return shaderMaterial.PassList.Length; } } // How many passes are in this material (Read Only). 
        
        public int renderQueue;// Render queue of this material. 
       
        // shader The shader used by the material. 
        public Shader shader
        {
            get
            {
                return _shader;
            }
            set
            {
                _shader = value;
                shaderMaterial = value.shaderMaterial;
            }
        }
        // Additional shader keywords set by this material. 
        public string[] shaderKeywords
        {
            get { return _shaderKeywords.ToArray<string>(); }
        }

        // Public Functions

        public Material()
        {
        }

        public Material(Material mat)
        {
            this.shader = mat.shader;
        }

        public Material(Shader shader)
        {
            this.shader = shader;
        }
        
        //CopyPropertiesFromMaterial Copy properties from other material into this material. 
        // This function copies property values
         // (both serialized and set at runtime), as well as shader keywords,
        // render queue and global illumination flags from the other material. Material's shader is not changed.

        public void CopyPropertiesFromMaterial(Material mat)
        {
        }

        //EnableKeyword Set a shader keyword that is enabled by this material. 
        // Shaders can be internally compiled into multiple variants, 
        // and then the matching one is picked based on material keywords (EnableKeyword and DisableKeyword),
         //or globally set shader keywords (Shader.EnableKeyword and Shader.DisableKeyword).

        public void EnableKeyword(string keyword)
        {
            _shaderKeywords.Add(keyword);
        }

        //DisableKeyword Unset a shader keyword. 
        public void DisableKeyword(string keyword)
        {
            _shaderKeywords.Remove(keyword);
        }

        /*Get a named color value.
        Many shaders use more than one color. Use GetColor to get the propertyName color.

        Common color names used by Unity's builtin shaders: 
        "_Color" is the main color of a material. This can also be accessed via color property. 
        "_SpecColor" is the specular color of a material (used in specular/glossy/vertexlit shaders). 
        "_EmissionColor" is the emissive color of a material (used in vertexlit shaders). 
        "_ReflectColor" is the reflection color of the material (used in reflective shaders).
        */

       public Color GetColor(string propertyName)
       {
           return Color.white;
       }
         
       public Color GetColor(int nameID)
       {
           return Color.white;
       }

//GetFloat Get a named float value. 
//GetInt Get a named integer value. 
//GetMatrix Get a named matrix value from the shader. 
//GetTag Get the value of material's shader tag. 
//GetTexture Get a named texture. 
//GetTextureOffset Gets the placement offset of texture propertyName. 
//GetTextureScale Gets the placement scale of texture propertyName. 
//GetVector Get a named vector value. 
//HasProperty Checks if material's shader has a property of a given name. 
//IsKeywordEnabled Is the shader keyword enabled on this material? 
//Lerp Interpolate properties between two materials. 
//SetBuffer Set a ComputeBuffer value. 
//SetFloat Set a named float value. 
//SetInt Set a named integer value.When setting values on materials using the Standard Shader, you should be aware that you may need to use EnableKeyword to enable features of the shader that were not previously in use. For more detail, read Accessing Materials via Script. 
//SetMatrix Set a named matrix for the shader. 
//SetOverrideTag Sets an override tag/value on the material. 
     
       /*     Activate the given pass for rendering.
            Pass indices start from zero and go up to (but not including) passCount.
            This is mostly used in direct drawing code using GL class. For example, Image Effects use materials for implementing screen post-processing. For each pass in the material they activate the pass and draw a fullscreen quad.
            If SetPass returns false, you should not render anything. This is typically the case for special pass types that aren't meant for rendering, like GrabPass.
        */ 
        public bool SetPass(int pass){
            return true;
           // return shaderMaterial.SetPass(pass);
        }

        //SetTexture Set a named texture. 
        //SetTextureOffset Sets the placement offset of texture propertyName. 
        //SetTextureScale Sets the placement scale of texture propertyName. 
        //SetVector Set a named vector value. 

       //SetColor Set a named color value. 
       public void SetColor(string propertyName, Color value)
       {
       }
       public void SetColor(int nameID, Color value)
       {
       }
       public void SetFloat(string attName, float value)
       {
       }
       public void SetInt(string attName, int value)
       {
       }
       public void SetVector(string attName, Vector3 value)
       {
       }
       public void SetVector(string attName, Vector4 value)
       {
       }
       public void SetTexture(string attName, Texture txt)
       {
       }
       public void SetMatrix(string attName, Matrix4x4 mat)
       {
       }
    }
}
