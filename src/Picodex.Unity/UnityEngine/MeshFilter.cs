using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class MeshFilter : Component
    {
        Mesh _mesh = null;

        // Returns the instantiated Mesh assigned to the mesh filter. 
        public Mesh mesh
        {
            get
            {
                return _mesh;
            }
            set
            {
                if (_mesh != null && _mesh._meshView.IsAttached)
                {
                    transform._node.DetachObject(_mesh._meshView.MovableObject);
                }
                _mesh = value;
                _OnAttach();
            }
        }

        public Mesh sharedMesh;// Returns the shared mesh of the mesh filter. 

        private bool isAttach
        {
            get
            {
                return _mesh != null && mesh._meshView.IsAttached;
            }
        }

        internal override void _OnAttach()
        {
            if (!isAttach)
            {
                if (_mesh != null)
                    transform._node.AttachObject(mesh._meshView.MovableObject);
            }

        }

        internal override void _OnLinkTo(Object trasformParent)
        {
        }
    }
}
