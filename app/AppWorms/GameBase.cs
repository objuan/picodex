#region Using Statements
using System;
using System.Collections.Generic;
using System.Timers;
using OpenTK;
#endregion

using UnityEngine;
using UnityEngine.Platform;

namespace AppWorms
{


    public class MyExternalWindow : ExternalWindow
    {
        GLControl control;
     
        public MyExternalWindow(GLControl control)
        {
            this.control = control;
        }
        public OpenTK.Graphics.IGraphicsContext Context { get { return control.Context; } }
        public OpenTK.Platform.IWindowInfo WindowInfo { get { return control.WindowInfo; } }
        public OpenTK.Graphics.GraphicsMode GraphicsMode { get { return control.GraphicsMode; } }
        public IntPtr Handle { get { return control.Handle; } }
        public Axiom.Graphics.RenderSystem CreateRenderSystem() { return  new Axiom.RenderSystems.OpenGL.GLRenderSystem(); } 
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameBase 
    {
        UnityContext unityContext;
        public MouseState MouseState = new MouseState();
        UnitySceneTest scene;
        MyExternalWindow win;

        public GameBase()
        {
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public virtual void Initialize(GLControl control)
        {
            string dataCache = @"C:\marco\saturn\bin";

            win = new MyExternalWindow(control);
            unityContext = new UnityContext(dataCache, control.Width, control.Height, win);
            MouseState.init();


            // TEST SCENE

            //scene = new UnitySceneTest_1().Build(unityContext);
            scene = new UnitySceneTest_3().Build(unityContext);
        }

        public virtual void OnResize(int width, int height)
        {
            unityContext.OnResize( width, height);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Render()
        {
            unityContext.OnRender( MouseState);
        }
    }
}
