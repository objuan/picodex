using System;
using System.Collections.Generic;

using System.Text;

using Picodex.Vxcm;

namespace picodex.worms
{
    public class Planet
    {
        public GameParams pars;

        private VXCMVolume volume;

        public VXCMVolume Volume
        {
            get { return volume; }
        }

        public Planet(GameParams pars)
        {
            this.pars = pars;
          //  volume = new VXCMVolume(pars.Ray * 2, pars.SubSampling, pars.distanceFieldMin, pars.distanceFieldMax);
        }
    }
}
