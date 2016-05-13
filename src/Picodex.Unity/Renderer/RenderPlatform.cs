using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    public enum ClearFlags
    {
        CLEAR_NULL = 0,
        CLEAR_COLOR = 0x00004000,
        CLEAR_DEPTH = 0x00000100,
        CLEAR_STENCIL = 0x00000400,
        CLEAR_COLOR_DEPTH = CLEAR_COLOR | CLEAR_DEPTH,
        CLEAR_COLOR_STENCIL = CLEAR_COLOR | CLEAR_STENCIL,
        CLEAR_DEPTH_STENCIL = CLEAR_DEPTH | CLEAR_STENCIL,
        CLEAR_COLOR_DEPTH_STENCIL = CLEAR_COLOR | CLEAR_DEPTH | CLEAR_STENCIL,
        CLEAR_ALL = CLEAR_COLOR | CLEAR_DEPTH | CLEAR_STENCIL
    };

    public class RenderPlatform :  RenderNative
    {
        private static RenderPlatform singleton = null;
 
        private Viewport viewport = new Viewport();

        public static RenderPlatform Singleton
        {
            get
            {
               // if (singleton == null) singleton = new RenderPlatform();
                return singleton;
            }
        }

        public RenderPlatform(String dataCachePath, Viewport viewport)
            : base(RenderNative.PXRenderPlatform_new(dataCachePath, viewport.Vector))
        {
            singleton = this;
          
        }

        ~RenderPlatform()
        {
            PXRenderPlatform_destroy(nativeClassPtr);
        }

        public void SetScreenViewport(Viewport viewport)
        {
            if (this.viewport.X != viewport.X || this.viewport.Y != viewport.Y || this.viewport.Width != viewport.Width || this.viewport.Height != viewport.Height)
            {
                this.viewport = viewport;
                PXRenderPlatform_setScreenViewport(nativeClassPtr, viewport.Vector);
            }
        }

        public void BeginFrame(double time)
        {
            PXRenderPlatform_beginFrame(nativeClassPtr, (float)time);
        }

        public void EndFrame()
        {
            PXRenderPlatform_endFrame(nativeClassPtr);
        }

        public void render(Scene scene, Camera camera, RenderTarget renderTargetP, ClearFlags clearFlag)
        {
            PXRenderPlatform_render(nativeClassPtr, scene.NativeClassPtr, camera.NativeClassPtr,
                IntPtr.Zero, (int)clearFlag);
        }
    
        public void renderImmediate(Scene scene, Camera camera, RenderTarget renderTargetP, ClearFlags clearFlag)
        {
            PXRenderPlatform_renderImmediate(nativeClassPtr, scene.NativeClassPtr, camera.NativeClassPtr,
                IntPtr.Zero, IntPtr.Zero,  (int)clearFlag);
        }

       /* public void Initialize(string dataPath)
        {
           native = new RenderNative(dataPath);
        }
        * */

 
    }
}
