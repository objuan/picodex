using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Graphics
    {
        public static RenderBuffer activeColorBuffer; 
        public static RenderBuffer activeDepthBuffer; 

        //Static Functions

        //Blit Copies source texture into destination render texture with a shader. 
        public static void Blit(Texture source, RenderTexture dest)
        {
            Blit(source, null, null, -1);
        }
      
        public static void Blit(Texture source, Material mat, int pass ){
            Blit(source,null,mat,pass);
        }

        // dest Destination RenderTexture, or null to blit directly to screen. 
        // mat Material to use. Material's shader could do some post-processing effect, for example. 
        // pass If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        public static void Blit(Texture source, RenderTexture dest, Material mat, int pass ){
        }

        // 
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation){
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            DrawMesh(mesh, matrix, null, -1, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex){
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            DrawMesh(mesh, matrix, null, -1, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer){
             Matrix4x4 matrix =Matrix4x4.TRS(position,rotation,Vector3.one);
             DrawMesh(mesh, matrix, material, layer, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera){
              Matrix4x4 matrix =Matrix4x4.TRS(position,rotation,Vector3.one);
              DrawMesh(mesh, matrix, material, layer, camera, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera , int submeshIndex){
              Matrix4x4 matrix =Matrix4x4.TRS(position,rotation,Vector3.one);
              DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, Rendering.ShadowCastingMode castShadows){
             Matrix4x4 matrix =Matrix4x4.TRS(position,rotation,Vector3.one);
             DrawMesh(mesh,matrix,material,layer,camera,submeshIndex,properties,castShadows, true,null);
        }

        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, Rendering.ShadowCastingMode castShadows, bool receiveShadows , Transform probeAnchor){

             Matrix4x4 matrix =Matrix4x4.TRS(position,rotation,Vector3.one);
             DrawMesh(mesh,matrix,material,layer,camera,submeshIndex,properties,castShadows, receiveShadows,probeAnchor);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix){
            DrawMesh(mesh, matrix, null, -1, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, int materialIndex){
            DrawMesh(mesh, matrix, null, -1, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer){
            DrawMesh(mesh, matrix, material, layer, null, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }
                 
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera )
        {
            DrawMesh(mesh, matrix, material, layer, camera, 0, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera , int submeshIndex  )
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null, Rendering.ShadowCastingMode.On, true, null);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera , int submeshIndex , MaterialPropertyBlock properties)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, Rendering.ShadowCastingMode.On , true, null);
        }
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera , int submeshIndex , MaterialPropertyBlock properties , bool castShadows , bool receiveShadows )
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, (castShadows) ? Rendering.ShadowCastingMode.On : Rendering.ShadowCastingMode.Off, true, null);
        }

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, Rendering.ShadowCastingMode castShadows){
            DrawMesh(mesh,matrix,material,layer,camera,submeshIndex,properties,castShadows, true,null);
        }

        // -----------------------------

        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, Rendering.ShadowCastingMode castShadows, bool receiveShadows , Transform probeAnchor ){
        }


        // =============================

        //BlitMultiTap Copies source texture into destination, for multi-tap shader. 
        //ClearRandomWriteTargets Clear random write targets for Shader Model 5.0 level pixel shaders. 
        //DrawMesh Draw a mesh. 
        //DrawMeshNow Draw a mesh immediately. 

        //Draws a fully procedural geometry on the GPU. 

        public static void DrawProcedural(MeshTopology topology, int vertexCount)
        {
            DrawProcedural(topology, vertexCount, 1);
        }
        public static void DrawProcedural(MeshTopology topology, int vertexCount, int instanceCount){
        }

        //DrawProceduralIndirect Draws a fully procedural geometry on the GPU. 

        //DrawTexture Draw a texture in screen coordinates. 
        public static void DrawTexture(Rect screenRect, Texture texture){
        }

        public static void DrawTexture(Rect screenRect, Texture texture, Material mat){
            DrawTexture(screenRect,texture,new Rect(0,0,texture.width,texture.height),0,0,0,0,Color.white,mat);
        }

        public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat){
             DrawTexture(screenRect,texture,new Rect(0,0,texture.width,texture.height),leftBorder,rightBorder,topBorder,bottomBorder,Color.white,mat);
        }

        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat ){
            DrawTexture(screenRect,texture,sourceRect,leftBorder,rightBorder,topBorder,bottomBorder,Color.white,mat);
         
        }

        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color){
            DrawTexture(screenRect,texture,sourceRect,leftBorder,rightBorder,topBorder,bottomBorder,color);
        }

        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat){
        }


        //ExecuteCommandBuffer Execute a command buffer. 
        public static void ExecuteCommandBuffer(Rendering.CommandBuffer buffer)
        {

        }

        //SetRandomWriteTarget Set random write target for Shader Model 5.0 level pixel shaders. 
      
        // Sets current render target. 
        public static void SetRenderTarget(RenderTexture rt)
        {
        }


        // =======================================
        // The mesh will be just drawn once, 
        // it won't be per-pixel lit and will not cast or receive realtime shadows. 
        // If you want full integration with lighting and shadowing, use Graphics.DrawMesh instead.

        public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix)
        {
            DrawMeshNow(mesh, matrix, -1);
        }

        
        public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex){
            Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            DrawMeshNow(mesh, matrix, materialIndex);
        }

        public static void DrawMeshNow(Mesh mesh, Matrix4x4 m, int materialIndex)
        {

        }


    }
}
