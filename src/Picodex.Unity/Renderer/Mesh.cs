using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;

namespace Picodex.Render
{
    public interface IMesh
    {
        void DrawGeometry();
    }

    public enum MaterialParamName
    {
        Diffuse,
        Ambient,
        DiffuseMap,
        Selected,
        Side
    }

    // public delegate void MeshOnDrawHandler(Mesh mesh);
       
    public delegate void MeshDrawHandler(Mesh mesh);
    public class Mesh : Node
    {
        public static float NormalLayer = 0.01f;
        public static float SelectLayer = 0.01f;

         
       Matrix4 localTranform = new Matrix4();

        private MeshDrawCallback native_drawCallback;   // Ensure it doesn't get garbage collected

        public event MeshDrawHandler BeforeDraw;
        public  Scene Scene=null;

        public new Matrix4 LocalTranform
        {
            get
            {
                return localTranform;
            }
            set
            {
                localTranform = value;
                float[] arr = Utils.MakeFloatArray(value);
                PXMesh_setTrx(nativeClassPtr, arr);
            }
        }

        public Mesh()
            : base(PXMesh_new())
        {

        }

        ~Mesh()
        {
         //   PXMesh_destroy(nativeClassPtr);

        }

        public void RemoveFromScene()
        {
            if (Scene != null)
                Scene.RemoveMesh(this);
        }

        public void setWorldBBox(Vector3 minW, Vector3 maxW)
        {
            PXMesh_setWorldBBox(nativeClassPtr,new float[]{ minW.X,minW.Y,minW.Z}, new float[]{ maxW.X,maxW.Y,maxW.Z});
        }


        //public void SetGeometry(float[] verts,  float[] normals,  uint[] triangles,int nb_vert, int nb_face)
        //{
        //    PXMesh_setGeometry(nativeClassPtr, verts, normals, triangles, nb_vert, nb_face);
        //}
        public void SetGeometry(float[] verts, float[] normals, int[] triangles)
        {
            PXMesh_setGeometry(nativeClassPtr, verts, normals, triangles, verts.Length / 3, triangles.Length);
        }
        // no index
        public void SetTriangles(float[] verts, float[] normals,int startCount,int triCount)
        {
            PXMesh_setTriangles(nativeClassPtr, verts, normals, verts.Length / 3, startCount, triCount / 3);
        }
         public void SetTriangles(float[] uvs,int startCount,int triCount)
        {
            PXMesh_setTrianglesUV(nativeClassPtr,  uvs, uvs.Length / 3, startCount, triCount / 3);
        }

         public void SetStencil(float[] stencil, int triCount)
         {
             PXMesh_setStencil(nativeClassPtr, stencil, triCount / 3);
         }

         public void UpdateStencil(float[] stencil, int triCount)
         {
             PXMesh_updateStencil(nativeClassPtr, stencil, triCount / 3);
         }

        // ===============================================================
       

        public void SetMaterialParam(MaterialParamName name, float value)
        {
            PXMesh_setMaterialValueF(nativeClassPtr, name.ToString().ToLower(), value);
        }
        public void SetMaterialParam(MaterialParamName name, Vector3 value)
        {
            PXMesh_setMaterialValueC(nativeClassPtr, name.ToString().ToLower(), new float[]{ value.X,value.Y,value.Z});
        }
        public void SetMaterialParam(MaterialParamName name, float x,float y,float z)
        {
            PXMesh_setMaterialValueC(nativeClassPtr, name.ToString().ToLower(), new float[] {x,y,z });
        }
        public void SetMaterialParam(MaterialParamName name, String value)
        {
            PXMesh_setMaterialValueS(nativeClassPtr, name.ToString().ToLower(), value);
        }
        public void SetMaterialParam(MaterialParamName name, bool value)
        {
            PXMesh_setMaterialValueI(nativeClassPtr, name.ToString().ToLower(), value ? 1 : 0);
        }

        public void SetMaterialEffectName( String value)
        {
            PXMesh_setMaterialEffectName(nativeClassPtr, value);
        }

        public void NotifyCallback()
        {
            native_drawCallback = new MeshDrawCallback(OnNativeDraw);
            PXMesh_setCallback(nativeClassPtr, native_drawCallback);

       //     PXMesh_setTriangles(mesh);
        }

        private void OnNativeDraw(IntPtr meshP)
        {
            if (meshP == nativeClassPtr) 
                OnDraw();
        }

        protected virtual void OnDraw()
        {
            if (BeforeDraw!=null) BeforeDraw(this);
        }

    }
}
