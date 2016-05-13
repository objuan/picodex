using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Behaviour : Component
    {
        public bool enabled;  //Enabled Behaviours are Updated, disabled Behaviours are not. 

        public bool isActiveAndEnabled;// Has the Behaviour had enabled called. 

    }
}
