using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    public class Program : RenderNative
    {

        private RenderPlatform renderPlatform;

        public Program(RenderPlatform renderPlatform)
            : base(renderPlatform.NativeClassPtr)
		{
            this.renderPlatform = renderPlatform;
        }


        public void SetAttributeData(string attributeName)
        {
            PXProgram_SetAttributeData(renderPlatform.NativeClassPtr, attributeName);
        }
        
        public void DisableAttribute(string attributeName)
        {
            PXProgram_DisableAttribute(renderPlatform.NativeClassPtr, attributeName);
        }

        public void LoadUniformsMatrices(Matrix4 value )
        {
            float[] arr = Utils.MakeFloatArray(value);
            PXProgram_LoadUniformsMatrices(renderPlatform.NativeClassPtr, arr);
        }
       

    }
}
