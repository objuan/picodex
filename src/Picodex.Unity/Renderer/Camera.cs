using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    public class Camera : Node
    {
      
        public Camera()
            : base(PXCamera_new())
        {

        }

        ~Camera()
        {
            PXCamera_destroy(nativeClassPtr);
        }


        public void SetPerpective(float fov, float near, float far, Vector2 offLens)
        {
            PXCamera_setPerpective(nativeClassPtr, fov, near, far, new float[]{offLens.X,offLens.Y});
        }

        public void SetAspectRatio(float ratio)
        {
            PXCamera_setAspectRatio(nativeClassPtr, ratio);
        }
    }

  
}
