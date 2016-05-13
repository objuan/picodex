using System;
using System.Collections.Generic;

using System.Text;

namespace picodex.worms
{
    public class GameContext
    {
        private GameParams pars;
        private Planet planet;

        public Planet Planet
        {
            get { return planet; }
        }

        public GameContext()
        {
        }

        public void Initialize(GameParams pars)
        {
            this.pars = pars;
            planet = new Planet(pars);
        }
    }
}
