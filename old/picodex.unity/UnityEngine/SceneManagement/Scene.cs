using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.SceneManagement
{
    public class RootNode : Transform
    {
        public UnityEngine.SceneManagement.Scene scene;

        public RootNode(UnityEngine.SceneManagement.Scene scene)
        {
            this.scene = scene;
        }
    }

    public class Scene
    {
        public Axiom.Core.SceneManager _sceneManager;

        private List<GameObject> rootGameObjects = new List<GameObject>();
        private List<Camera> cameraList = new List<Camera>();
        private List<Renderer> rendererList = new List<Renderer>();
        private List<Light> lightList = new List<Light>();

        public List<Light> LightList
        {
            get { return lightList; }
        }

        public List<Renderer> RendererList
        {
            get { return rendererList; }
        }

        public Scene()
        {
            //rootGameObjects.Add(new GameObject());
            _sceneManager = Axiom.Core.Root.Instance.CreateSceneManager("DefaultSceneManager", "UnityScene");
            _sceneManager.ClearScene();
        }

        public List<GameObject> GetRootGameObjects()
        {
            return rootGameObjects;
        }

        public void AddRoot(GameObject so)
        {
            so._rootScene = this;
            so.transform._node = _sceneManager.RootSceneNode.CreateChildSceneNode();
            rootGameObjects.Add(so);
        }

        // CAMERA

        public Axiom.Core.Camera AddCamera(Camera cam)
        {
            if (cameraList.Count == 0) cam.tag = "Main Camera";
            cameraList.Add(cam);

            Axiom.Core.Camera _camera = new Axiom.Core.Camera("", _sceneManager);

            return _camera;
        }

        public void RemoveCamera(Camera cam)
        {
            cameraList.Remove(cam);
        }

        public int allCamerasCount
        {
            get
            {
                return cameraList.Count;
            }
        }

        public Camera[] allCameras
        {
            get
            {
                //TODO attive
                return cameraList.ToArray();
            }
        }

        public Camera MainCamera
        {
            get
            {
                foreach (Camera c in cameraList)
                {
                    if (c.tag == "Main Camera") return c;
                }
                return null;
            }
        }


        public void GetAllCameras(Camera[] cameras)
        {
            for (int i = 0; i < Math.Min(cameras.Length, cameraList.Count); i++) cameras[i] = cameraList[i];

        }

        // RENDERER

        public void AddRenderer(Renderer renderer)
        {
            rendererList.Add(renderer);
        }

        public void RemoveRenderer(Renderer renderer)
        {
            rendererList.Remove(renderer);
        }

        // LIGHT

        public void AddLight(Light light)
        {
            lightList.Add(light);
        }

        public void RemoveLight(Light light)
        {
            lightList.Remove(light);
        }
    }
}
