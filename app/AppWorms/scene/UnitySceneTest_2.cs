using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

namespace AppWorms
{


    public class UnitySceneTest_2 : UnitySceneTest
    {

        public override void CreateScene(UnityEngine.SceneManagement.Scene scene)
        {
            camera.transform.position = new Vector3(0, 0, 10);
            camera.transform.LookAt(new Vector3(0, 0, 0));
            orbit.distance = 500;

            Axiom.Core.Light light = scene._sceneManager.CreateLight("MainLight");
            light.Position = new Vector3(50, 80, 0);

           // Mesh mesh = (Mesh)Resources.Load("ogrehead.mesh", typeof(Mesh));
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.parent = root.transform;
            go.transform.scale = new Vector3(10, 10, 10);

          //  go.renderer.material = new Material(Shader.Find("Diffuse/Sample")); 
          //  go.renderer.material = new Material(Shader.Find("Ogre/Compositor/GlassPass")); 
        

            GameObject go1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go1.transform.parent = root.transform;
            go1.transform.position = new Vector3(20, 0, 0);
            go1.transform.scale = new Vector3(10, 10, 10);

            //scene.AmbientLight = new Color(.4f, .4f, .4f);

            //Axiom.Core.Entity head = scene._sceneManager.CreateEntity("OgreHead", "ogrehead.mesh");
            Axiom.Core.Entity head = scene._sceneManager.CreateEntity("OgreHead", "ninja.mesh");
      
            ////  entityList.Add(head);
            Axiom.Core.SceneNode node = scene._sceneManager.RootSceneNode.CreateChildSceneNode();
            node.Position = new Vector3(10, 0, 0);
            node.AttachObject(head);

            head.GetSubEntity(0).MaterialName = "Diffuse/Sample";

            Axiom.Graphics.Material mat = head.GetSubEntity(0).Material;
            Axiom.Graphics.Pass pass = mat.GetTechnique(0).GetPass(0);

            // TEST Asset mesh
            if (true)
            {
                Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Meshes/ogrehead.mesh", typeof(Mesh));

            }
            //Axiom.Graphics.TextureUnitState _textureunit = pass.CreateTextureUnitState();

            ////_textureunit.TextureNameAlias = "axiomlogo.png";

            //_textureunit.SetTextureName("axiomlogo.png", Axiom.Graphics.TextureType.TwoD);

            //_textureunit.BindingType = Axiom.Graphics.TextureBindingType.Fragment;
            //_textureunit.DesiredFormat = Axiom.Media.PixelUtil.GetFormatFromName("axiomlogo.png", true);

            //_textureunit.IsAlpha = false;
            //_textureunit.MipmapCount = 1;
            //_textureunit.IsHardwareGammaEnabled = false;
            //_textureunit.TextureCoordSet = 0;

        }

    }
}
