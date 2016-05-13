using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class MySubMesh
    {
    }

    public enum MeshTopology
    {
        Triangles,// Mesh is made from triangles. 
        Quads,// Mesh is made from quads. 
        Lines,//Mesh is made from lines. 
        LineStrip,// Mesh is a line strip. 
        Points// Mesh is made from points. 

    }

    public class Mesh : Object
    {
        List<MySubMesh> subMeshList = new List<MySubMesh>();

        Vector3[] _vertices  = new Vector3[] {};
        Vector3[] _normals = new Vector3[]{} ;
        Vector2[] _uv = new Vector2[] {};
        int[] _triangles = new int[] {};

        //bindposes The bind poses. The bind pose at each index refers to the bone with the same index. 
        //blendShapeCount Returns BlendShape count on this mesh. 
        //boneWeights The bone weights of each vertex. 
       
        //The bounding volume of the mesh. 
        public Bounds bounds;

        //colors Vertex colors of the mesh. 
        //colors32 Vertex colors of the mesh. 
        //isReadable Returns state of the Read/Write Enabled checkbox when model was imported. 

        // //  The normals of the mesh. 
        public Vector3[] normals
        {
            get{
                return _normals;
            }
            set{
                _normals = value;
            }
        }
        //  The number of submeshes. Every material has a separate triangle list. 
        public int subMeshCount
        {
            get{
                return subMeshList.Count;
            }
        }
        //public Vector3 tangents The tangents of the mesh. 
        // An array containing all triangles in the mesh. 
        public int[] triangles
        {
            get
            {
                return _triangles;
            }
            set
            {
                _triangles = value;
            }
        }
        //  The base texture coordinates of the mesh. 
        public Vector2[] uv
        {
            get
            {
                return _uv;
            }
            set
            {
                _uv = value;
            }
        }
        //uv2 The second texture coordinate set of the mesh, if present. 
        //uv3 The third texture coordinate set of the mesh, if present. 
        //uv4 The fourth texture coordinate set of the mesh, if present. 
        // Returns the number of vertices in the mesh (Read Only). 
        public int vertexCount 
        {
            get
            {
                return _vertices.Length;
            }
        }
       // Returns a copy of the vertex positions or assigns a new vertex positions array. 
        public Vector3[] vertices 
        {
            get{
                return _vertices;
            }
            set{
                _vertices = value;
            }
        }

        // ==========================

        public Mesh()
        {
        }

        public void Clear() { }

        public void RecalculateBounds() { }

        public void Optimize() { }

        // ==========================

        //AddBlendShapeFrame Adds a new blend shape frame. 
        //Clear Clears all vertex data and all triangle indices. 
        //ClearBlendShapes Clears all blend shapes from Mesh. 
        //CombineMeshes Combines several meshes into this mesh. 
        //GetBlendShapeFrameCount Returns the frame count for a blend shape. 
        //GetBlendShapeFrameVertices Retreives deltaVertices, deltaNormals and deltaTangents of a blend shape frame. 
        //GetBlendShapeFrameWeight Returns the weight of a blend shape frame. 
        //GetBlendShapeIndex Returns index of BlendShape by given name. 
        //GetBlendShapeName Returns name of BlendShape by given index. 
        //GetIndices Returns the index buffer for the submesh. 
        //GetTopology Gets the topology of a submesh. 
        //GetTriangles Returns the triangle list for the submesh. 
        //GetUVs Get the UVs for a given chanel. 
        //MarkDynamic Optimize mesh for frequent updates. 
        //Optimize Optimizes the mesh for display. 
        //RecalculateBounds Recalculate the bounding volume of the mesh from the vertices. 
        //RecalculateNormals Recalculates the normals of the mesh from the triangles and vertices. 
        //SetColors Vertex colors of the mesh. 
        //SetIndices Sets the index buffer for the submesh. 
        //SetNormals Set the normals of the mesh. 
        //SetTangents Set the tangents of the mesh. 
        //SetTriangles Sets the triangle list for the submesh. 
        //SetUVs Set the UVs for a given chanel. 
        //SetVertices Assigns a new vertex positions array. 
        //UploadMeshData Upload previously done mesh modifications to the graphics API. 


    }
}
