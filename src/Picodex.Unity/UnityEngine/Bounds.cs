using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Bounds
    {
        public Vector3 min;
        public Vector3 max;


        public Vector3 size { get { return max-min; } }
        public Vector3 center { get { return min + size * 0.5f; } }

        public Bounds(Vector3 center, Vector3 size)
        {
            this.min = center - size * 0.5f;
            this.max = center + size * 0.5f;
        }
    }
}
