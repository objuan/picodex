using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.Rendering;

namespace UnityEngine.Platform
{
 /*
  * When Deferred Shading is used, the rendering process in Unity happens in two passes:
    1.G-buffer Pass: objects are rendered to produce screen-space buffers with diffuse color, specular color, smoothness, world space normal, emission and depth.
    2.Lighting pass: the previously generated buffers are used to add lighting into emission buffer.
    Objects with shaders that can’t handle deferred shading are rendered after this process is complete, using the forward rendering path.
    The default g-buffer layout is as follows:
    •RT0, ARGB32 format: Diffuse color (RGB), occlusion (A).
    •RT1, ARGB32 format: Specular color (RGB), roughness (A).
    •RT2, ARGB2101010 format: World space normal (RGB), unused (A).
    •RT3, ARGB2101010 (non-HDR) or ARGBHalf (HDR) format: Emission + lighting + lightmaps + reflection probes buffer.
    •Depth+Stencil buffer.
    So the default g-buffer layout is 160 bits/pixel (non-HDR) or 192 bits/pixel (HDR).
    Emission+lighting buffer (RT3) is logarithmically encoded to provide greater dynamic range than is usually possible with an ARGB32 texture, when camera is not using HDR.
    Note that when the camera is using HDR rendering, then there’s no separate render target being created for Emission+lighting buffer (RT3); instead the render target that the camera will render into (i.e. the one that will be passed to the image effects) is used as RT3.
    G-Buffer Pass
    The g-buffer pass renders each object once. Diffuse and specular colors, surface smoothness, world space normal, and emission+ambient+reflections+lightmaps are rendered into g-buffer textures. The g-buffer textures are setup as global shader properties for later access by shaders (CameraGBufferTexture0 .. CameraGBufferTexture3 names).
    Lighting Pass
    The lighting pass computes lighting based on g-buffer and depth. Lighting is computed in screen space, so the time it takes to process is independent of scene complexity. Lighting is added to the emission buffer.
    Point and spot lights that do not cross the camera’s near plane are rendered as 3D shapes, with the Z buffer’s test against the scene enabled. This makes partially or fully occluded point and spot lights very cheap to render. Directional lights and point/spot lights that cross the near plane are rendered as fullscreen quads.
    If a light has shadows enabled then they are also rendered and applied in this pass. Note that shadows do not come for “free”; shadow casters need to be rendered and a more complex light shader must be applied.
    The only lighting model available is Standard. If a different model is wanted you can modify the lighting pass shader, by placing the modified version of the Internal-DeferredShading.shader file from the Built-in shaders into a folder named “Resources” in your “Assets” folder. Then go to the Edit->Project Settings->Graphics window. Changed the “Deferred” dropdown to “Custom Shader”. Then change the Shader option which appears to the shader you are using.
    */
    // ForwardBase and ForwardAdd passes are used.
    public class DeferredRenderingPipeline : RenderingPipeline
    {
        private CommandBuffer commandBuffer = new CommandBuffer();

        public DeferredRenderingPipeline()
        {
        }

        //BeforeGBuffer, // Before deferred rendering G-buffer is rendered. 
        //AfterGBuffer, // After deferred rendering G-buffer is rendered. 
        //BeforeLighting, // Before lighting pass in deferred rendering. 
        //AfterLighting, // After lighting pass in deferred rendering. 
        //BeforeReflections, // Before reflections pass in deferred rendering. 
        //AfterReflections, // After reflections pass in deferred rendering. 
        //BeforeFinalPass, // Before final geometry pass in deferred lighting. 
        //AfterFinalPass, // After final geometry pass in deferred lighting. 

        public void Render(Camera camera)
        {
            // PASSI

            // SHADOW MAP

            // GBUFFER

            // LIGHTING

            // REFLECTIONS

            // FINAL PASS

        }


    }
}
