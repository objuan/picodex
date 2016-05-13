using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public struct RenderBuffer
    {
        // Returns native RenderBuffer. Be warned this is not native Texture, but rather pointer to unity struct that can be used with native unity API. Currently such API exists only on iOS. 
        public IntPtr GetNativeRenderBufferPtr()
        {
            return IntPtr.Zero;
        }
    }

    public enum RenderTextureFormat
    {
        ARGB32 = 0,// Color render texture format, 8 bits per channel. 
        Depth,// A depth render texture format. 
        ARGBHalf ,//Color render texture format, 16 bit floating point per channel. 
        Shadowmap,// A native shadowmap render texture format. 
        RGB565 ,//Color render texture format. 
        ARGB4444 ,//Color render texture format, 4 bit per channel. 
        ARGB1555,// Color render texture format, 1 bit for Alpha channel, 5 bits for Red, Green and Blue channels. 
        Default,// Default color render texture format: will be chosen accordingly to Frame Buffer format and Platform. 
        ARGB2101010,// Color render texture format. 10 bits for colors, 2 bits for alpha. 
        DefaultHDR,// Default HDR color render texture format: will be chosen accordingly to Frame Buffer format and Platform. 
        ARGBFloat ,//Color render texture format, 32 bit floating point per channel. 
        RGFloat,// Two color (RG) render texture format, 32 bit floating point per channel. 
        RGHalf,// Two color (RG) render texture format, 16 bit floating point per channel. 
        RFloat,// Scalar (R) render texture format, 32 bit floating point. 
        RHalf,// Scalar (R) render texture format, 16 bit floating point. 
        R8 ,//Scalar (R) render texture format, 8 bit fixed point. 
        ARGBInt,// Four channel (ARGB) render texture format, 32 bit signed integer per channel. 
        RGInt,// Two channel (RG) render texture format, 32 bit signed integer per channel. 
        RInt// Scalar (R) render texture format, 32 bit signed integer. 
    }

    public enum RenderTextureReadWrite
    {
        Default,// The correct color space for the current position in the rendering pipeline. 
        Linear,// No sRGB reads or writes to this render texture. 
        sRGB 
    }

    public class RenderTexture : Texture
    {
        public static RenderTexture active;

        public int antiAliasing;// The antialiasing level for the RenderTexture. 
        public RenderBuffer  colorBuffer;// Color buffer of the render texture (Read Only). 
        public int depth ;//The precision of the render texture's depth buffer in bits (0, 16, 24 are supported). 
        public RenderBuffer depthBuffer ;//Depth/stencil buffer of the render texture (Read Only). 
        public bool enableRandomWrite ;//Enable random access write into this render texture on Shader Model 5.0 level shaders. 
        public RenderTextureFormat format;//The color format of the render texture. 
        public bool generateMips;// Should mipmap levels be generated automatically? 
        public int height;// The height of the render texture in pixels. 
        public bool isCubemap;// If enabled, this Render Texture will be used as a Cubemap. 
        public bool isVolume;// If enabled, this Render Texture will be used as a Texture3D. 
       // sRGB Does this render texture use sRGB read/write conversions (Read Only). 
        public bool useMipMap ;//Use mipmaps on a render texture? 
       // volumeDepth Volume extent of a 3D render texture. 
        public int width;// The width of the render texture in pixels. 

        public RenderTexture(int w, int h, int d)
        {
        }

        public void DiscardContents(bool diascartCVolor, bool discardDepth)
        {
        }

    }
}
