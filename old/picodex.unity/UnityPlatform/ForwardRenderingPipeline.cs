using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEngine.Platform
{
    /*In Forward Rendering, some number of brightest lights that affect each object are rendered in fully per-pixel lit mode. Then, up to 4 point lights are calculated per-vertex. The other lights are computed as Spherical Harmonics (SH), which is much faster but is only an approximation. Whether a light will be a per-pixel light or not is dependent on this:
    •Lights that have their Render Mode set to Not Important are always per-vertex or SH.•Brightest directional light is always per-pixel.
    •Lights that have their Render Mode set to Important are always per-pixel.•If the above results in less lights than current Pixel Light Count Quality Setting, then more lights are rendered per-pixel, in order of decreasing brightness.Rendering of each object happens as follows:
    •Base Pass applies one per-pixel directional light and all per-vertex/SH lights.
    •Other per-pixel lights are rendered in additional passes, one pass for each light.
    For example, if there is some object that’s affected by a number of lights (a circle in a picture below, affected by lights A to H): 
     
    Let’s assume lights A to H have the same color and intensity and all of them have Auto rendering mode, so they would be sorted in exactly this order for this object. The brightest lights will be rendered in per-pixel lit mode (A to D), then up to 4 lights in per-vertex lit mode (D to G), and finally the rest of lights in SH (G to H):
     
    Note that light groups overlap; for example last per-pixel light blends into per-vertex lit mode so there are less “light popping” as objects and lights move around.
    Base Pass
    Base pass renders object with one per-pixel directional light and all SH lights. This pass also adds any lightmaps, ambient and emissive lighting from the shader. Directional light rendered in this pass can have Shadows. Note that Lightmapped objects do not get illumination from SH lights.
    Additional Passes
    Additional passes are rendered for each additional per-pixel light that affect this object. Lights in these passes can’t have shadows (so in result, Forward Rendering supports one directional light with shadows).
    */

    // ForwardBase and ForwardAdd passes are used.
    public class ForwardRenderingPipeline : RenderingPipeline
    {
        private CommandBuffer commandBuffer = new CommandBuffer();
     
        public ForwardRenderingPipeline()
        {
        }

        public void Render(Camera camera)
        {
            // lista luci
            // lista render

            Scene scene = camera.gameObject.scene;

            // per ogni luce faccio render

            foreach(Light light in scene.LightList)
            {
                foreach(Renderer renderer in scene.RendererList)
                {
                    Material mat = renderer.material;
                    if (mat != null)
                    {
                        if (mat.SetPass(0))
                        {
                        }
                    }
                }
            }

            //camera.GetComponents<
        }


    }
}
