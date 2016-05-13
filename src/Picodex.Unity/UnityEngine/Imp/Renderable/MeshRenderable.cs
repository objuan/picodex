using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Graphics;

namespace UnityEngine.Imp
{
    internal class MeshRenderable : SimpleRenderable
    {
        private const int POSITION = 0;
        private const int NORMAL = 1;
        private const int UV = 2;
        private HardwareVertexBuffer posBuffer, colorBuffer;
        
        float boundingRadius;

        public MeshRenderable()
        {
        }

        protected void Dispose(bool disposeManagedResources)
        {
            //if (!IsDisposed)
            //{
            //    if (disposeManagedResources)
            //    {
            //        MaterialManager.Instance.Remove("TriMat");
            //        colorBuffer.Dispose();
            //        posBuffer.Dispose();
            //    }
            //}
            //base.dispose(disposeManagedResources);
        }

        public override float GetSquaredViewDepth(Axiom.Core.Camera camera)
        {
            Vector3 min, max, mid, dist;
            min = box.Minimum;
            max = box.Maximum;
            mid = ((min - max) * 0.5f) + min;
            dist = camera.DerivedPosition - mid;

            return dist.LengthSquared;
        }

        public override float BoundingRadius { get { return boundingRadius; } }

        public void Clear()
        { }

        public void Rebuild(Mesh mesh)
        {
            int triCount = mesh.triangles.Length;

            vertexData = new VertexData();
            indexData = new IndexData();

            renderOperation.vertexData = vertexData;
            renderOperation.vertexData.vertexCount = mesh.vertices.Length;
            renderOperation.vertexData.vertexStart = 0;
            renderOperation.indexData = indexData;
            renderOperation.useIndices = true;

            //setup buffers

            VertexDeclaration decl = vertexData.vertexDeclaration;
            VertexBufferBinding bind = vertexData.vertexBufferBinding;
            int offset = 0;
            offset += decl.AddElement(POSITION, 0, VertexElementType.Float3, VertexElementSemantic.Position).Size;
            offset += decl.AddElement(NORMAL, offset, VertexElementType.Float3, VertexElementSemantic.Normal).Size;
            offset += decl.AddElement(UV, offset, VertexElementType.Float2, VertexElementSemantic.TexCoords).Size;


            HardwareIndexBuffer indexBuffer =
                HardwareBufferManager.Instance.CreateIndexBuffer(IndexType.Size32, triCount, BufferUsage.StaticWriteOnly);

            HardwareVertexBuffer vertexBuffer =
                HardwareBufferManager.Instance.CreateVertexBuffer(decl.GetVertexSize(POSITION), mesh.vertices.Length, BufferUsage.StaticWriteOnly);

            HardwareVertexBuffer normalBuffer =
              HardwareBufferManager.Instance.CreateVertexBuffer(decl.GetVertexSize(NORMAL), mesh.vertices.Length, BufferUsage.StaticWriteOnly);

            HardwareVertexBuffer uvBuffer =
                        HardwareBufferManager.Instance.CreateVertexBuffer(decl.GetVertexSize(UV), mesh.vertices.Length, BufferUsage.StaticWriteOnly);

            indexData.indexBuffer = indexBuffer;
            indexData.indexCount = triCount;
            indexData.indexStart = 0;

            indexBuffer.WriteData(0, indexBuffer.Size, mesh.triangles, true);

            vertexBuffer.WriteData(POSITION, vertexBuffer.Size, mesh.vertices);
            normalBuffer.WriteData(NORMAL, normalBuffer.Size, mesh.normals);
            uvBuffer.WriteData(UV, uvBuffer.Size, mesh.uv);
            //vertices = null;
            // faces = null;

            // Now make the render operation
            renderOperation.operationType = OperationType.TriangleList;
            renderOperation.indexData = indexData;
            renderOperation.vertexData = vertexData;


            bind.SetBinding(POSITION, vertexBuffer);
            bind.SetBinding(NORMAL, normalBuffer);
            bind.SetBinding(UV, uvBuffer);


            // Compute bbox, TODO BETTER
            Vector3 min = new Vector3(99999, 99999, 99999);
            Vector3 max = new Vector3(-99999, -99999, -99999);
            foreach (Vector3 v in mesh.vertices)
            {
                min.x = Mathf.Min(min.x,v.x);
                min.y = Mathf.Min(min.y, v.y);
                min.z = Mathf.Min(min.z, v.z);
                max.x = Mathf.Max(max.x, v.x);
                max.y = Mathf.Max(max.y, v.y);
                max.z = Mathf.Max(max.z, v.z);
            }
            box = new AxisAlignedBox(min,max);
            Vector3 size = max-min;
            boundingRadius = Mathf.Max(size.x, size.y, size.z) * 0.5f;

            //// add a position and color element to the declaration
            //decl.AddElement(POSITION, 0, VertexElementType.Float3, VertexElementSemantic.Position);
            //decl.AddElement(COLOR, 0, VertexElementType.Color, VertexElementSemantic.Diffuse);

            //// POSITIONS
            //// create a vertex buffer for the position
            //posBuffer = HardwareBufferManager.Instance.CreateVertexBuffer(decl.GetVertexSize(POSITION), vertexData.vertexCount, BufferUsage.StaticWriteOnly);

            //// write the positions to the buffer
            //posBuffer.WriteData(0, posBuffer.Size, mesh.vertices, true);

            //// bind the position buffer
            //binding.SetBinding(POSITION, posBuffer);

        }
    }

}
