using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Picodex.Render
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi), Serializable]
    public class PXFelix_RenderParams
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool ShadowEnabled;
        [MarshalAs(UnmanagedType.U1)]
        public bool SAOEnabled;
        [MarshalAs(UnmanagedType.U1)]
        public bool SKYEnabled;
        [MarshalAs(UnmanagedType.U1)]
        public bool SKYAmbientEnabled;
        [MarshalAs(UnmanagedType.U1)]
        public bool DOFEnabled;
        public float GammaCorrection;
        public float Exposure;
        public float AmbientFactor;
        public float DOF_focalDistance;
        public float DOF_focalRange;
        public float DOF_fstop;
    };

    public class FelixScene : RenderNative
    {

        private PXFelix_RenderParams pars = new PXFelix_RenderParams();
        bool initialized = false;

        public PXFelix_RenderParams Params
        {
            get
            {
                if (!initialized)
                {
                    PXFelix_getParams(nativeClassPtr, ref pars);
                    initialized = true;
                }
                return pars;
            }
        }

        public FelixScene(RenderPlatform renderPlatform)
            : base(PXFelix_new(renderPlatform.NativeClassPtr))
        {
          
        }

        ~FelixScene()
        {
            PXFelix_destroy(nativeClassPtr);
        }

        public void SetScene(Scene scene)
        {
            PXFelix_setScene(nativeClassPtr, scene.NativeClassPtr);
        }

        public void Render()
        {
            PXFelix_render(nativeClassPtr);
        }

        public void RenderMainPass()
        {
            PXFelix_renderMainPass(nativeClassPtr);
        }

         public void RenderPostPass()
        {
            PXFelix_renderPostPass(nativeClassPtr);
        }
         public void RenderScreenPass()
         {
             PXFelix_renderScreenPass(nativeClassPtr);
         }
         public void BindMainTarget()
        {
            PXFelix_bindMainTarget(nativeClassPtr);
        }

         public void BindFinalTarget()
         {
             PXFelix_bindFinalTarget(nativeClassPtr);
         }
        public  void LoadParams()
        {
            PXFelix_loadParams(nativeClassPtr);

        }

        public void UpdateParams()
        {
            PXFelix_setParams(nativeClassPtr, ref pars);
        }
    }
}
