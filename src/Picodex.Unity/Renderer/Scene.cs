using System;
using System.Collections.Generic;
using System.Text;

namespace Picodex.Render
{
    public class Scene : RenderNative
    {
        private List<Camera> cameraList = new List<Camera>();
        private List<Light> lightList = new List<Light>();
        private List<Mesh> meshList = new List<Mesh>();

        public List<Camera> Cameras
        {
            get { return cameraList; }
        }

        public List<Light> Lights
        {
            get { return lightList; }
        }
        public List<Mesh> Meshes
        {
            get { return meshList; }
        }

        public Scene()
            : base(PXScene_new())
        {

        }

        ~Scene()
        {
            PXScene_destroy(nativeClassPtr);
        }

        public void AddNode(Node node)
        {
            AddNode(node, null);
        }

        public void AddNode(Node node, Node parent)
        {
            if (node is Camera) cameraList.Add((Camera)node);
            if (node is Light) lightList.Add((Light)node);
            PXScene_addNode(nativeClassPtr, node.NativeClassPtr, (parent != null ) ? parent.NativeClassPtr : IntPtr.Zero);
        }

        public void AddMesh(Mesh mesh)
        {
            AddMesh(mesh, null);
        }
        public void AddMesh(Mesh mesh, Node parent)
        {
            PXScene_addMesh(nativeClassPtr, mesh.NativeClassPtr, (parent != null) ? parent.NativeClassPtr : IntPtr.Zero);
            meshList.Add(mesh);
            mesh.Scene = this;
        }
        public void RemoveMesh(Mesh mesh)
        {
            PXScene_removeMesh(nativeClassPtr, mesh.NativeClassPtr);
            mesh.Scene = null;
        }
        public void ClearMeshes()
        {
            PXScene_clearMeshes(nativeClassPtr);
        }
    }
}
