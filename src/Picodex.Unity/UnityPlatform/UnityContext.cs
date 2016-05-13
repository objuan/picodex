using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Platform
{

    public class UnityContext : IDisposable
    {
        private static UnityContext singleton = null;
        private UnityRenderPipeline renderPipeline;
        private string assetPath="";

        public string AssetPath
        {
            get { return assetPath; }
            set { assetPath = value.Replace("\\","/"); }
        }

        private UnityEngine.SceneManagement.Scene currentScene;
        System.Timers.Timer aTimer;
        System.Diagnostics.Stopwatch watch;

        public static UnityContext Singleton
        {
            get { return UnityContext.singleton; }
        }

        public UnityEngine.SceneManagement.Scene CurrentScene
        {
            get { return currentScene; }
            set { currentScene = value; }
        }

        public UnityContext(string dataPath, int width, int height, ExternalWindow externalWindow)
        {
            Debug.Log("Starting UNITY System V0.1");

            AssetPath = dataPath;
            singleton = this;
            renderPipeline = new UnityRenderPipelineAxiom(dataPath,width,height, externalWindow);
            Screen.SetResolution(width, height);

            //TODO ??
            Resources.Load("assets");

            currentScene = new UnityEngine.SceneManagement.Scene();

            aTimer = new System.Timers.Timer(30.0 / 1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            watch = System.Diagnostics.Stopwatch.StartNew();

            Time.frameCount=0;

            Debug.Log("Started");
        }

        public void Dispose()
        {
            aTimer.Stop();
            aTimer.Dispose();
            watch.Stop();
        }

        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        public void OnResize(int width, int height)
        {
            Screen.SetResolution(width, height);
            renderPipeline.OnResize( width, height);
        }


        public void OnRender(MouseState mouseState)
        {
            // TIME
            double globalTime = (double)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            float deltaTime = (float)(globalTime - Time.time);
            Time.time = globalTime;
            Time.deltaTime = deltaTime;

            // INPUT
            Input.changeState(mouseState);

            // FASI 

            // SOLO Allinizio
            if (Time.frameCount == 0)
            {
                foreach(GameObject go in currentScene.GetRootGameObjects())
                {
                    go.BroadcastMessage("Start");
                }
            }

            // SYM EVENTS

            // ogni ciclo

            foreach(GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("FixedUpdate");
            }
    

            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("Update");
            }
             
            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("LateUpdate");
            }

            //// 
            //// Called before the camera culls the scene. Culling determines which objects are visible to the camera. 
            //foreach (GameObject go in currentScene.GetRootGameObjects())
            //{
            //    go.BroadcastMessage("OnPreCull");
            //}
              
            // Called when an object becomes visible/invisible to any camera
            //foreach (GameObject go in currentScene.GetRootGameObjects())
            //{
            //    go.BroadcastMessage("OnBecameVisible"); // OnBecameInvisible
            //}
             
            ////  Called once for each camera if the object is visible
            //foreach (GameObject go in currentScene.GetRootGameObjects())
            //{
            //    go.BroadcastMessage("OnWillRenderObject");
            //}

            ////  Called before the camera starts rendering the scene
            // foreach (GameObject go in currentScene.GetRootGameObjects())
            //{
            //    go.BroadcastMessage("OnPreRender");
            //}

            // --------------------------------------------
            // render

            renderPipeline.RenderAll();
           // Camera.main.Render();

            // --------------------------------------------

            //  Called after all regular scene rendering is done
            // You can use GL class or Graphics.DrawMeshNow to draw custom geometry at this point
            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnRenderObject");
            }

            //// Called after a camera finishes rendering the scene
            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnPostRender");
            }

            //  Called after scene rendering is complete to allow postprocessing of the image,
            foreach (GameObject go in currentScene.GetRootGameObjects())
            {
                go.BroadcastMessage("OnRenderImage");
            }

            //•OnGUI: Called multiple times per frame in response to GUI events. The Layout and Repaint events are processed first, followed by a Layout and keyboard/mouse event for each input event.•OnDrawGizmos Used for drawing Gizmos in the scene view for visualisation purposes.
            
            Time.frameCount++;
          //  renderPipeline.OnRender(globalTime);
        }
    }
}
