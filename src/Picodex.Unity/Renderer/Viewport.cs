using System;
using System.Collections.Generic;
using System.Text;

namespace Picodex.Render
{
    public struct Viewport
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Viewport(int X,int Y,int W,int H)
        {
            this.X=X;
            this.Y=Y;
            this.Width=W;
            this.Height=H;
        }

        public float[] Vector
        {
            get
            {
                return new float[] { X, Y, Width, Height };
            }
        }

    }
}
