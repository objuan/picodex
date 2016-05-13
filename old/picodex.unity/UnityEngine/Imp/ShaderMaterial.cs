using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Imp
{
    public class Pass
    {
    }
    [Serializable]
    public class ShaderMaterial//: Axiom.Graphics.Material
    {
        Pass[] passList = new Pass[1];
        public Shader shader;

        public Pass[] PassList
        {
            get { return passList; }
        }

        public ShaderMaterial()
        {
            passList[0] = new Pass();
        }

        public bool SetPass(int pass)
        {
            return true;
           // return passList[pass].Bind();
        }

      

        // crea dalla risorsa
        internal void Create(ShaderResource entry)
        {
          //  passList = new Axiom.Graphics.Pass[entry.passList.Count];

            //for(int i=0;i<entry.passList.Count;i++)
            //{
            //    passList[i] = ShaderMaterialPass.Create(entry.passList[i]);
            //}
        }

    }
}
