using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public struct Rect
    {
        int xMin;
        int xMax;
        int yMin;
        int yMax;

        public int x { get { return xMin; } set { xMax += (x -xMin); xMin= x; } }
        public int y { get { return yMin; } set { yMax += (y - yMin); yMin = y; } }
        public int width { get { return xMax - xMin; } set {  } }
        public int height { get { return yMax - yMin; } set {  } }
       
        public Rect(int x, int y, int w, int h)
        {
            xMin = x;
            yMin = y;
            xMax = x + w;
            yMax = y + h;
        }
    }
}
