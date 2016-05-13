using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

namespace AppWorms
{

    public class UnitySceneTest_3 : UnitySceneTest
    {
        public class MyBehaviour : MonoBehaviour
        {
            void Start()
            {
            }

            void Update()
            {
                Material mat = GetComponent<MeshRenderer>().material;

                Debug.Log("Material : " + mat.name);
                Debug.Log("Color : " + mat.color.To_0_255_String());
                Debug.Log("Pass Count : " + mat.passCount);
            }
        }

        public override void CreateScene(UnityEngine.SceneManagement.Scene scene)
        {
            camera.transform.position = new Vector3(0, 0, 10);
            camera.transform.LookAt(new Vector3(0, 0, 0));
            orbit.distance = 200;

            Axiom.Core.Light light = scene._sceneManager.CreateLight("MainLight");
            light.Position = new Vector3(50, 80, 0);

           // Mesh mesh = (Mesh)Resources.Load("ogrehead.mesh", typeof(Mesh));
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.parent = root.transform;
            go.transform.scale = new Vector3(10, 10, 10);
            go.name = "sphere";

          //  go.renderer.material = new Material(Shader.Find("Diffuse/Sample")); 
          //  go.renderer.material = new Material(Shader.Find("Ogre/Compositor/GlassPass")); 
        

            GameObject go1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go1.transform.parent = root.transform;
            go1.transform.position = new Vector3(20, 0, 0);
            go1.transform.scale = new Vector3(10, 10, 10);
            go1.name = "Cube";
           
            Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Meshes/ogrehead.mesh", typeof(Mesh));

            GameObject go_mesh = new GameObject();
            go_mesh.AddComponent<MeshFilter>().mesh = mesh;
            go_mesh.AddComponent<MeshRenderer>();
            go_mesh.transform.parent = root.transform;
            go_mesh.name = "Mesh";

            // mio
            go_mesh.AddComponent<MyBehaviour>();

        }

    }
}
