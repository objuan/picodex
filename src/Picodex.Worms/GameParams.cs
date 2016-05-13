using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace picodex.worms
{
    public struct GameParams
    {
        public int Ray;
        public int SubSampling; // 1 = no , 2 = *2
        public int distanceFieldMin ;
        public int distanceFieldMax ;

        //private PlanetParams()
        //{
        //    Ray = 16;
        //    SubSampling = 1;
        //    distanceFieldMin = 2;
        //    distanceFieldMax = 2;
        //}
    }
}
