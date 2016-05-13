using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Texture3D : Texture
    {
        public Texture3D(int w, int h, int d,TextureFormat format,bool mipmap)
        {
        }

        //       Apply Actually apply all previous SetPixel and SetPixels changes. 
        public void Apply()
        {
        }

//Compress Compress texture into DXT format. 
//EncodeToJPG Encodes this texture into JPG format. 
//EncodeToPNG Encodes this texture into PNG format. 
//GetPixel Returns pixel color at coordinates (x, y). 
//GetPixelBilinear Returns filtered pixel color at normalized coordinates (u, v). 
//GetPixels Get a block of pixel colors. 
//GetPixels32 Get a block of pixel colors in Color32 format. 
//GetRawTextureData Get raw data from a texture. 
//LoadImage Loads PNG/JPG image byte array into a texture. 
//LoadRawTextureData Fills texture pixels with raw preformatted data. 
//PackTextures Packs multiple Textures into a texture atlas. 
//Resize Resizes the texture. 
//SetPixel Sets pixel color at coordinates (x,y). 
//SetPixels Set a block of pixel colors. 
//UpdateExternalTexture Updates Unity texture to use different native texture object. 

        // Set a block of pixel colors. 
        public void SetPixels32(Color32[] buffer)
        { 
        }
        

        public void ReadPixels(Rect source, int destX, int destY)
        {
            ReadPixels(source,destX,destY);
        }

        // Read pixels from screen into the saved texture data. 
        public void ReadPixels(Rect source, int destX, int destY, bool recalculateMipMaps)
        {
        }

    }
}
