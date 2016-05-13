using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Graphics;

namespace UnityEngine.Imp
{
    // corrisponde ad una vista sul tipo axiom MovableObject
    internal class MaterialView
    {
        RenderableObject renderableObject;

        Material currentMaterial;
        protected List<Material> _materials;
        protected List<Material> _sharedMaterials;

        internal RenderableObject RenderableObject
        {
            get { return renderableObject; }
            set { renderableObject = value; }
        }


        public MaterialView( RenderableObject renderableObject)
        {
            this.renderableObject = renderableObject;
        }

     
        public virtual Material material
        {
            get
            {
                return _materials.SingleOrDefault(m => m._isActive);
            }
            set
            {
            }
        }

        // Returns all the instantiated materials of this object. 
        public virtual Material[] materials
        {
            get
            {
                return _materials.ToArray();
            }
        }

        public virtual Material sharedMaterial
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        // Returns all the instantiated materials of this object. 
        public virtual Material[] sharedMaterials
        {
            get
            {
                return _sharedMaterials.ToArray();
            }
        }

        protected virtual int GetMaterialCount()
        {
            return 0;
        }

        protected virtual Material GetMaterial(int idx)
        {
            return null;
        }

    }

}
