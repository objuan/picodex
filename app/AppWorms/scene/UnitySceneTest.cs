using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Picodex;

namespace AppWorms
{
    public abstract class UnitySceneTest
    {
        protected GameObject root;
        protected Camera camera;
        protected DragMouseOrbit orbit;

        public virtual void CreateScene(UnityEngine.SceneManagement.Scene scene)
        {
        }

        public virtual UnitySceneTest Build(UnityEngine.Platform.UnityContext unityContext)
        {
            root = GameObject.CreateNull();
            unityContext.CurrentScene.AddRoot(root);

            GameObject camObj = new GameObject();
            camObj.transform.parent = root.transform;
            camObj.name = "camera";

            // camera
            //GameObject lookNode = new GameObject();
            //   lookNode.transform.parent = root.transform;

            camera = new Camera();
            camObj.AddComponent(camera);
            // camera.transform.parent = root.transform;

            //camera._camera.Position = new Vector3(0, 0, 500);

            camera.transform.position = new Vector3(0, 0, 500);
            camera.transform.LookAt(new Vector3(0, 0, 0));
            //    camera._camera.LookAt(new Vector3(0, 0, -300));

            //// set the near clipping plane to be very close
            camera.nearClipPlane = 5;

           
            //demo = new Axiom.Demos.FrustumCulling();
            //demo.Setup();

            //       DragMouseOrbit cam = new DragMouseOrbit();
            //       cam.target = camera.GetComponent<Transform>();
            //       cam.transform.parent = root.transform;

            //       // mesh
            //       BillboardPlane plane = new BillboardPlane();
            //       plane.transform.parent = root.transform;
            ////       plane.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/SingleColor"));

            //       // light

            //       GameObject lightGameObject = new GameObject("The Light");
            //       Light lightComp = lightGameObject.AddComponent<Light>();
            //       lightComp.color = Color.blue;
            //       lightGameObject.transform.position = new Vector3(0, 5, 0);
            //       lightGameObject.transform.parent = root.transform;


            // TEST

            // TestOrtho test = new TestOrtho();
            // camera.AddComponent(test);

            GameObject ancor = new GameObject();
            ancor.transform.parent = root.transform;
            ancor.name = "ancor";

            orbit = new DragMouseOrbit();
            orbit.distance = 500;
            orbit.distanceMax = 1000;
            orbit.target = ancor.transform;
            camObj.AddComponent(orbit);

            CreateScene(UnityEngine.Platform.UnityContext.Singleton.CurrentScene);

            return this;
        }
    }

    public class UnitySceneTest_1 : UnitySceneTest
    {
        public override void CreateScene(UnityEngine.SceneManagement.Scene scene)
        {
            scene._sceneManager.AmbientLight = new Color(.4f, .4f, .4f);

            Axiom.Core.Light light = scene._sceneManager.CreateLight("MainLight");
            light.Position = new Vector3(50, 80, 0);

            Axiom.Core.Entity head = scene._sceneManager.CreateEntity("OgreHead", "ogrehead.mesh");
          //  entityList.Add(head);
            scene._sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(head);

            Axiom.Core.Entity box = scene._sceneManager.CreateEntity("Box1", "cube.mesh");
           // entityList.Add(box);
            scene._sceneManager.RootSceneNode.CreateChildSceneNode(new Vector3(100, 0, 0), Quaternion.Identity).AttachObject(box);

            box = scene._sceneManager.CreateEntity("Box2", "cube.mesh");
           // entityList.Add(box);
            scene._sceneManager.RootSceneNode.CreateChildSceneNode(new Vector3(0, 100, 0), Quaternion.Identity).AttachObject(box);

            box = scene._sceneManager.CreateEntity("Box3", "cube.mesh");
           // entityList.Add(box);
            scene._sceneManager.RootSceneNode.CreateChildSceneNode(new Vector3(0, 200, 0), Quaternion.Identity).AttachObject(box);

            //Axiom.Core.Frustum frustum = new Axiom.Core.Frustum("PlayFrustum");
            //frustum.Near = 10;
            //frustum.Far = 300;

            // create a node for the frustum and attach it
           //frustumNode = scene.RootSceneNode.CreateChildSceneNode(new Vector3(0, 0, 200), Quaternion.Identity);

            // set the camera in a convenient position
            //camera.Position = new Vector3( 0, 759, 680 );
            //camera.LookAt( Vector3.Zero );

          //  CreateCamera();

            //frustumNode.AttachObject(frustum);
            //frustumNode.AttachObject(camera2);
        }

      
    }
}
