using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Graphics;

namespace UnityEngine.Imp
{
    // 
    internal class RenderableObject
    {
        Axiom.Core.MovableObject movableObject=null;

        MaterialView materialView;
        MeshView meshView;

        public MaterialView MaterialView { get { return materialView; } }

        public MeshView MeshView { get { return meshView; } }

        public Axiom.Core.MovableObject MovableObject
        {
            get { return movableObject; }
            set { movableObject = value; }
        }

        public RenderableObject()
        {
            // primo di default
            movableObject = new MeshRenderable();

            materialView = new MaterialView(this);
            meshView = new MeshView(this);
        }
          
        public RenderableObject( Axiom.Core.Entity entity )
        {
            // primo di default
            movableObject = entity;

            materialView = new EntityMaterialView(this);
            meshView = new MeshView(this);
        }

    }

}
