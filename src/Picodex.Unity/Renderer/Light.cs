using System;
using System.Collections.Generic;
using System.Text;

namespace Picodex.Render
{
    public class Light : Node
    {
        public Light()
            : base(PXLight_new())
        {

        }

        ~Light()
        {
            PXLight_destroy(nativeClassPtr);
        }


    }
}
