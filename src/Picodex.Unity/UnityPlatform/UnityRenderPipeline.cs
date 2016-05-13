using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Platform
{
    public class UnityRenderPipeline
    {
        private static UnityRenderPipeline singleton = null;

        public static UnityRenderPipeline Singleton
        {
            get { return UnityRenderPipeline.singleton; }
        }

        private ForwardRenderingPipeline forwardRendering;
        private DeferredRenderingPipeline deferredRendering;
        int cameraCount;
        private Camera[] cameras = new Camera[20];

        private Camera currentCamera=null;

        public Camera CurrentCamera
        {
            get
            {
                return currentCamera;
            }
 
        }

        public UnityRenderPipeline(string dataPath)
        {
            singleton = this;

            forwardRendering = new ForwardRenderingPipeline();
            deferredRendering = new DeferredRenderingPipeline();

            Initialize();
        }

        public virtual void OnResize(int width, int height)
        {
           // renderPlatform.SetScreenViewport(new Picodex.Render.Viewport(0, 0, width, height));
        }

        protected virtual void Initialize()
        {
        }

        public virtual void RenderAll()
        {
            // RENDER
            OnStartFrame();

            // RENDER

              cameraCount = UnityContext.Singleton.CurrentScene.allCamerasCount;
              UnityContext.Singleton.CurrentScene.GetAllCameras(cameras);
              for(int i=0;i<cameraCount;i++)
              {
                  if (!cameras[i].gameObject.activeInHierarchy) continue;

                  currentCamera = cameras[i];
                  Render(currentCamera);
              }

              OnEndFrame();
        }

        public virtual void OnStartFrame()
        {
        }
        public virtual void OnEndFrame()
        {
        }
        // =======================================================================

        public virtual void Render(Camera camera)
        {
            UnityEngine.SceneManagement.Scene scene = camera.gameObject.scene;
            if (scene != UnityContext.Singleton.CurrentScene)
            {
                // scena cambiata
                UnityContext.Singleton.CurrentScene = camera.gameObject.scene;
            }

            // Called before the camera culls the scene. Culling determines which objects are visible to the camera. 
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnPreCull");
            }

            //  Called once for each camera if the object is visible
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnWillRenderObject");
            }

            //  Called before the camera starts rendering the scene
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnPreRender");
            }

            // WORK  JOB


            if (camera.renderingPath == RenderingPath.Forward)
            {
                forwardRendering.Render(camera);
            }

            //// Called after a camera finishes rendering the scene
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnPostRender");
            }
            
        }
    }
}
