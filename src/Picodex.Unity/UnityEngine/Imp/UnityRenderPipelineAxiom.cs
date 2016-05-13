using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Core;
using Axiom.Graphics;

namespace UnityEngine.Platform
{
    public interface ExternalWindow
    {
        OpenTK.Graphics.IGraphicsContext Context { get; }
        OpenTK.Platform.IWindowInfo WindowInfo { get; }
        OpenTK.Graphics.GraphicsMode GraphicsMode { get; }
        IntPtr Handle { get; }
        Axiom.Graphics.RenderSystem CreateRenderSystem();
    }

    public class UnityRenderPipelineAxiom : UnityRenderPipeline
    {
        Root engine;
        Axiom.Graphics.RenderSystem renderSystem;
        RenderWindow window;
        Axiom.CgPrograms.CgProgramFactory cgFactory;

        Axiom.Demos.TechDemo demo;

        public UnityRenderPipelineAxiom(string dataPath,int width, int height, ExternalWindow mainWindow)
            : base(dataPath)
        {
            engine = new Root("render.log",false);

            
            renderSystem = mainWindow.CreateRenderSystem();
            engine.RenderSystems.Add(renderSystem);
            engine.RenderSystem = renderSystem;

            Axiom.Collections.NamedParameterList miscParams = new Axiom.Collections.NamedParameterList();

            miscParams.Add("vsync","true");
            miscParams.Add("externalWindowInfo", mainWindow.WindowInfo);
            miscParams.Add("externalGraphicsMode", mainWindow.GraphicsMode);

            engine.Initialize(false, "Axiom Engine Demo Window");

            // unity parser
            Axiom.Scripting.Compiler.ScriptCompilerManager.Instance.TranslatorManagers.Add(new Imp.UnityScriptTranslatorManager());

            window = renderSystem.CreateRenderWindow("UnityWin", width, height, false, miscParams);

            Screen._renderWindow = window;

            engine.OneTimePostWindowInit();

            renderSystem.InitRenderTargets();

            // --------------------

            GL.renderSystem = renderSystem;
            
            // CODECS

            Axiom.Media.CodecManager.Instance.RegisterCodecs();

            // CG

           cgFactory = new Axiom.CgPrograms.CgProgramFactory();

           HighLevelGpuProgramManager.Instance.AddFactory(cgFactory);

            // --------------

           ResourceGroupManager.Instance.CreateResourceGroup("Assets");
           ResourceGroupManager.Instance.AddResourceLocation("Assets", "Folder", "Assets", true, false);
           // ResourceGroupManager.Instance.AddResourceLocation("Media/Materials", "Folder", true);

           // MaterialManager.Instance.ReloadAll();

            ResourceGroupManager.Instance.InitializeAllResourceGroups();
        }

        virtual public void Dispose()
        {
            if (engine != null)
            {
                // remove event handlers
              //  engine.FrameStarted -= OnFrameStarted;
               // engine.FrameEnded -= OnFrameEnded;
            }
            //if (scene != null)
            //{
            //    scene.RemoveAllCameras();
            //}
            //camera = null;
            if (Root.Instance != null)
            {
                Root.Instance.RenderSystem.DetachRenderTarget(window);
            }
            if (window != null)
            {
                window.Dispose();
            }
            if (engine != null)
            {
                engine.Dispose();
            }
        }

        public override void OnResize(int width, int height)
        {
           // renderPlatform.SetScreenViewport(new Picodex.Render.Viewport(0, 0, width, height));
        }

        protected override void Initialize()
        {
          
        }


        public override void OnStartFrame()
        {
        }

        public override void OnEndFrame()
        {
            engine.RenderOneFrame();
        }
     

    
    }
}
