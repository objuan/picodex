﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Color32
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Color32()
        {
        }

        public Color32(byte r,byte g,byte b,byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }
}
