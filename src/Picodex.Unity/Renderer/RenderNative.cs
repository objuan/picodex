using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Picodex.Render
{
    public class RenderNative
    {
        const string dllPath = @"C:\marco\saturn\bin\saturn.render_d.dll";
      /*  #if x64
         const string dllPath = @"saturn.render64.dll";
        #else
        const string dllPath = @"saturn.render.dll";
        #endif
     */

         protected IntPtr nativeClassPtr;
	//	private bool disposed = false;

        public IntPtr NativeClassPtr
		{
			get { return nativeClassPtr; }
		}

        public RenderNative(IntPtr nativePtr)
		{
			this.nativeClassPtr = nativePtr;
		}

		~RenderNative()
		{
			//Dispose();
		}


        // Felix
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXFelix_new(IntPtr  renderP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_destroy(IntPtr  felixP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_setScene(IntPtr felixP,IntPtr sceneP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_render(IntPtr  felixP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXFelix_loadParams(IntPtr felixP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_renderMainPass(IntPtr  felixP);
           [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_renderPostPass(IntPtr  felixP);
           [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]

           public static extern void PXFelix_renderScreenPass(IntPtr felixP);
           [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_bindMainTarget(IntPtr  felixP);

       [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_bindFinalTarget(IntPtr  felixP);



        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXFelix_getParams(IntPtr  felixP,ref PXFelix_RenderParams parm);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXFelix_setParams(IntPtr felixP, ref PXFelix_RenderParams parm);


      

        // NODE
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXNode_setTrx(IntPtr nodeP,float[] localToWorldTrx);

        // LIGHT
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXLight_new();
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXLight_destroy(IntPtr  lightP);

        // CAMERA
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXCamera_new();
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXCamera_destroy(IntPtr  cameraP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXCamera_setAspectRatio(IntPtr  cameraP,float aspect);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXCamera_setPerpective(IntPtr  cameraP,float fov,float _near,float _far,float[] lensOffset);


        // MESH
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXMesh_new();
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_destroy(IntPtr  meshP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setTrx(IntPtr meshP,float []localToWorldTrx);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXMesh_setWorldBBox(IntPtr meshP, float[] minW, float[] maxW);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setGeometry(IntPtr  meshP,
						        float[] verts,
						        float[] normals,
						        int[] triangles,
						        int nb_vert,
						        int nb_face);

         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXMesh_setTriangles(IntPtr meshP,
                               float[] verts,
                               float[] normals,
                               int nb_vert,
                               int startCount,
                               int triCount);

         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
         public static extern void PXMesh_setTrianglesUV(IntPtr meshP,
                                float[] uvs,
                                int nb_vert,
                                int startCount,
                                int triCount);

          [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
         public static extern void PXMesh_setStencil(IntPtr meshP, float[] data, int triCount);
  
         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
          public static extern void PXMesh_updateStencil(IntPtr meshP, float[] data, int triCount);

        public delegate void MeshDrawCallback(IntPtr meshP);
         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXMesh_setCallback(IntPtr meshP,MeshDrawCallback callback);


     /*   public static extern void PXMesh_setGeometryI(IntPtr meshP,
                              float[] verts,
                              float[] normals,
                              int[] triangles,
                              int nb_vert,
                              int nb_face);
      * */

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setMaterial(IntPtr  meshP,String matFile,String matName);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setMaterialValueF(IntPtr  meshP,String paramName,float value);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setMaterialValueC(IntPtr  meshP,String paramName,float[] value3);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setMaterialValueS(IntPtr  meshP,String paramName,String value);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXMesh_setMaterialValueI(IntPtr meshP, String paramName, int value);

       [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXMesh_setMaterialEffectName(IntPtr  meshP,String value);
  
        // SCENE

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXScene_new();
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXScene_destroy(IntPtr  sceneP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXScene_clearMeshes(IntPtr sceneP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int PXScene_getCameraCount(IntPtr sceneP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr  PXScene_getCamera(IntPtr sceneP,int idx);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]

        public  static extern int PXScene_getLightCount(IntPtr sceneP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXScene_getLight(IntPtr sceneP,int idx);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
	
        public  static extern int PXScene_getMeshCount(IntPtr sceneP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr PXScene_getMesh(IntPtr sceneP,int idx);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern IntPtr  PXScene_addNode(IntPtr sceneP,IntPtr nodeP,IntPtr parentNodeP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXScene_addMesh(IntPtr sceneP,IntPtr meshP,IntPtr parentNodeP);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXScene_removeMesh(IntPtr sceneP, IntPtr meshP);

        // ========================
        // RenderPlatform
        // ========================

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr PXRenderPlatform_new(String dataCachePath, float[] viewport);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_destroy(IntPtr renderP);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_setScreenViewport(IntPtr renderP, float[] viewport);
       
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_render(IntPtr renderP, IntPtr sceneP, IntPtr cameraP, IntPtr renderTargetP, int clearFlag);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_renderImmediate(IntPtr renderP, IntPtr sceneP, IntPtr cameraP, IntPtr overrideMaterialP, IntPtr renderTargetP, int clearFlag);

        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_beginFrame(IntPtr renderP, float globalTime);
        [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PXRenderPlatform_endFrame(IntPtr renderP);

        // ========================
        // Program
        // ========================

         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public  static extern void PXProgram_SetAttributeData(IntPtr renderP, String attributeName);

         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
         public static extern void PXProgram_DisableAttribute(IntPtr renderP, String attributeName);

         [DllImport(dllPath, ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
         public static extern void PXProgram_LoadUniformsMatrices(IntPtr renderP, float[] matrix);
    }
}
