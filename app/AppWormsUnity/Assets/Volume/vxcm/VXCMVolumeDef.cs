using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;

namespace Picodex
{
    [System.Serializable]
    public class VXCMVolumeDef
    {
        public float gradientMin= -2;
        public float gradientMax = 2;
        public float distanceFieldMax = 4;

        public string name;

        public int samplingRate = 1;

        public Vector3i resolution;


    }
}
