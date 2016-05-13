using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    class Utils
    {

        public static float[] MakeFloatArray(Matrix4 mat)
        {
            float[] arr = new float[1]{
                mat.M11
            };
            return arr;
        }
    }
}
