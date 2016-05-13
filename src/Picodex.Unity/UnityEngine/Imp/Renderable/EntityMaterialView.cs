using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Graphics;
using Axiom.Core;

namespace UnityEngine.Imp
{
    // corrisponde ad una vista sul tipo axiom MovableObject
    internal class EntityMaterialView : MaterialView
    {
        Entity entity;

        public EntityMaterialView(RenderableObject renderableObject)
            : base(renderableObject)
        {
            // loadup

            entity = renderableObject.MovableObject as Entity;

            int count = entity.SubEntityCount;

            

            for(int i=0;i<count;i++)
            {
                SubEntity se = entity.GetSubEntity(i);
                
                Material material = new Material();

             //   se.Material

               // entity.
            }
        }


    }

}
