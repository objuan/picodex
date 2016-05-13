using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Imp
{
    [Serializable]
    public class ShaderMaterialPass1
    {
        public static ShaderMaterialPass1 Create(string shaderCode)
        {
            return null;
        }

        public bool Bind()
        {
            return true;
        }

        public void SetFragmentProgram(string program){
        }
    }

  
}
