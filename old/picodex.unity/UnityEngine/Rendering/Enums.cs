using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Rendering
{
    public enum BuiltinRenderTextureType
    {
        CurrentActive,// Currently active render target. 
        CameraTarget,// Target texture of currently rendering camera. 
        Depth ,//Camera's depth texture. 
        DepthNormals,// Camera's depth+normals texture. 
        PrepassNormalsSpec,// Deferred lighting (normals+specular) G-buffer. 
        PrepassLight,// Deferred lighting light buffer. 
        PrepassLightSpec,// Deferred lighting HDR specular light buffer (Xbox 360 only). 
        GBuffer0,// Deferred shading G-buffer #0 (typically diffuse color). 
        GBuffer1 ,//Deferred shading G-buffer #1 (typically specular + roughness). 
        GBuffer2,// Deferred shading G-buffer #2 (typically normals). 
        GBuffer3,// Deferred shading G-buffer #3 (typically emission/lighting). 
        Reflections,//Reflections gathered from default reflection and reflections probes. 

    }

    public enum ShadowCastingMode
    {
        Off, //No shadows are cast from this object. 
        On, // Shadows are cast from this object. 
        TwoSided, // Shadows are cast from this object, treating it as two-sided. 
        ShadowsOnly //
    }

    public enum CameraEvent
    {
        BeforeDepthTexture, // Before camera's depth texture is generated. 
        AfterDepthTexture, // After camera's depth texture is generated. 
        BeforeDepthNormalsTexture, // Before camera's depth+normals texture is generated. 
        AfterDepthNormalsTexture, // After camera's depth+normals texture is generated. 
        BeforeImageEffectsOpaque, // Before image effects that happen between opaque & transparent objects. 
        AfterImageEffectsOpaque, // After image effects that happen between opaque & transparent objects. 
        BeforeSkybox, // Before skybox is drawn. 
        AfterSkybox, //After skybox is drawn. 
        BeforeImageEffects, // Before image effects. 
        AfterImageEffects, // After image effects. 
        AfterEverything , //After camera has done rendering everything. 

        // forward
        BeforeForwardOpaque, // Before opaque objects in forward rendering. 
        AfterForwardOpaque, //After opaque objects in forward rendering. 
        BeforeForwardAlpha, // Before transparent objects in forward rendering. 
        AfterForwardAlpha, // After transparent objects in forward rendering. 
     
        // deferred
        BeforeGBuffer, // Before deferred rendering G-buffer is rendered. 
        AfterGBuffer, // After deferred rendering G-buffer is rendered. 
        BeforeLighting, // Before lighting pass in deferred rendering. 
        AfterLighting, // After lighting pass in deferred rendering. 
        BeforeReflections, // Before reflections pass in deferred rendering. 
        AfterReflections, // After reflections pass in deferred rendering. 
        BeforeFinalPass, // Before final geometry pass in deferred lighting. 
        AfterFinalPass, // After final geometry pass in deferred lighting. 
      
   
    }

    public enum CullMode
    {
        Off , // Disable culling. 
        Front, //  Cull front-facing geometry. 
        Back // Cull back-facing geometry. 

    }

    public enum BlendMode
    {
        Zero, // Blend factor is (0, 0, 0, 0). 
        One, // Blend factor is (1, 1, 1, 1). 
        DstColor, // Blend factor is (Rd, Gd, Bd, Ad). 
        SrcColor, // Blend factor is (Rs, Gs, Bs, As). 
        OneMinusDstColor, // Blend factor is (1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad). 
        SrcAlpha, // Blend factor is (As, As, As, As). 
        OneMinusSrcColor, // Blend factor is (1 - Rs, 1 - Gs, 1 - Bs, 1 - As). 
        DstAlpha, // Blend factor is (Ad, Ad, Ad, Ad). 
        OneMinusDstAlpha, // Blend factor is (1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad). 
        SrcAlphaSaturate , //Blend factor is (f, f, f, 1); where f = min(As, 1 - Ad). 
        OneMinusSrcAlpha  //Blend factor is (1 - As, 1 - As, 1 - As, 1 - As). 

    }

    public enum BlendOp
    {
        Add, // Add (s + d). 
        Subtract, // Subtract. 
        ReverseSubtract, // Reverse subtract. 
        Min , //Min. 
        Max, // Max. 
        LogicalClear , //Logical Clear (0). 
        LogicalSet, // Logical SET (1) (D3D11.1 only). 
        LogicalCopy , //Logical Copy (s) (D3D11.1 only). 
        LogicalCopyInverted, // Logical inverted Copy (!s) (D3D11.1 only). 
        LogicalNoop, // Logical No-op (d) (D3D11.1 only). 
        LogicalInvert, // Logical Inverse (!d) (D3D11.1 only). 
        LogicalAnd, // Logical AND (s & d) (D3D11.1 only). 
        LogicalNand, // Logical NAND !(s & d). D3D11.1 only. 
        LogicalOr, // Logical OR (s | d) (D3D11.1 only). 
        LogicalNor, // Logical NOR !(s | d) (D3D11.1 only). 
        LogicalXor, // Logical XOR (s XOR d) (D3D11.1 only). 
        LogicalEquivalence, // Logical Equivalence !(s XOR d) (D3D11.1 only). 
        LogicalAndReverse, // Logical reverse AND (s & !d) (D3D11.1 only). 
        LogicalAndInverted, // Logical inverted AND (!s & d) (D3D11.1 only). 
        LogicalOrReverse , //Logical reverse OR (s | !d) (D3D11.1 only). 
        LogicalOrInverted, // Logical inverted OR (!s | d) (D3D11.1 only). 
        Multiply, // Multiply (Advanced OpenGL blending). 
        Screen, // Screen (Advanced OpenGL blending). 
        Overlay, // Overlay (Advanced OpenGL blending). 
        Darken, // Darken (Advanced OpenGL blending). 
        Lighten, // Lighten (Advanced OpenGL blending). 
        ColorDodge, // Color dodge (Advanced OpenGL blending). 
        ColorBurn, // Color burn (Advanced OpenGL blending). 
        HardLight, // Hard light (Advanced OpenGL blending). 
        SoftLight, // Soft light (Advanced OpenGL blending). 
        Difference, // Difference (Advanced OpenGL blending). 
        Exclusion, // Exclusion (Advanced OpenGL blending). 
        HSLHue, // HSL Hue (Advanced OpenGL blending). 
        HSLSaturation, // HSL saturation (Advanced OpenGL blending). 
        HSLColor, // HSL color (Advanced OpenGL blending). 
        HSLLuminosity // HSL luminosity (Advanced OpenGL blending). 

    }

}
