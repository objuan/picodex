using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Graphics;

namespace UnityEngine.Imp
{
    // corrisponde ad una vista sul tipo axiom MovableObject
    internal class MeshView
    {
        RenderableObject renderableObject;

        internal RenderableObject RenderableObject
        {
            get { return renderableObject; }
            set { renderableObject = value; }
        }

        public Axiom.Core.MovableObject MovableObject
        {
            get
            {
                return renderableObject.MovableObject;
            }
        }

        public MeshView( RenderableObject renderableObject)
        {
            this.renderableObject = renderableObject;
        }

        public bool IsAttached
        {
            get
            {
                return renderableObject.MovableObject.IsAttached;
            }
        }
         
        public void Clear()
        {
            if (!(renderableObject.MovableObject is MeshRenderable))
            {
                renderableObject.MovableObject = new Imp.MeshRenderable();
            }
            ((Imp.MeshRenderable)renderableObject.MovableObject).Clear();
        }

        public void Rebuild(Mesh mesh)
        {
            if (!(renderableObject.MovableObject is MeshRenderable))
            {
                renderableObject.MovableObject = new Imp.MeshRenderable();
            }
            ((Imp.MeshRenderable)renderableObject.MovableObject).Rebuild(mesh);
        }

      
    }

}
