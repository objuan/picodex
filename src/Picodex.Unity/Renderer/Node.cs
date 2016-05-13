using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    public class Node : RenderNative
    {
        private Matrix4 localTranform = new Matrix4();

        public Matrix4 LocalTranform
        {
            get
            {
                return localTranform;
            }
            set
            {
                localTranform = value;
                float[] arr = Utils.MakeFloatArray(value);
                PXNode_setTrx(nativeClassPtr, arr);
            }
        }


        public Node(IntPtr nativePtr)
            : base(nativePtr)
		{
		}

    }
}
