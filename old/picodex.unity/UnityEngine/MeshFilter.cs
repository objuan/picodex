using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class MeshFilter : Component
    {
        public Mesh mesh ; // Returns the instantiated Mesh assigned to the mesh filter. 
        public Mesh sharedMesh;// Returns the shared mesh of the mesh filter. 
    }
}
