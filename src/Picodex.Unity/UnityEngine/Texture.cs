using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public enum TextureWrapMode
    {   
        Repeat,
        Clamp 
    }

    public enum TextureFormat
    {
        RGBA32
    }

    public enum FilterMode
    {
        Point,
        Bilinear,
        Trilinear

    }

    public class Texture
    {
        public int width; // Width of the texture in pixels. (Read Only) 
        public int height; // Height of the texture in pixels. (Read Only) 

        public int anisoLevel;// Anisotropic filtering level of the texture. 
        public FilterMode filterMode = FilterMode.Point;// Filtering mode of the texture. 
        public float mipMapBias;// Mip map bias of the texture. 

        public TextureWrapMode wrapMode;// Wrap mode (Repeat or Clamp) of the texture. 

//        GetNativeTexturePtr Retrieve native ('hardware') pointer to a texture. 
        public IntPtr GetNativeTexturePtr()
        {
            return IntPtr.Zero;
        }
    }
}
