using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.Rendering
{
  

    public class CommandBuffer : Object
    {
        //name Name of this command buffer. 
        int sizeInBytes;// Size of this command buffer in bytes (Read Only). 

         //Create a new empty command buffer. 
        public CommandBuffer()
        {
        }

        //Public Functions

       //  Add a "blit into a render texture" command. 

        public void Blit(Texture source, Rendering.RenderTargetIdentifier dest)
        {
        }
        public void Blit(Texture source, Rendering.RenderTargetIdentifier dest, Material mat)
        {
        }

        public void Blit(Texture source, Rendering.RenderTargetIdentifier dest, Material mat, int pass)
        {
        }

        public void Blit(Rendering.RenderTargetIdentifier source, Rendering.RenderTargetIdentifier dest)
        {
        }

        public void Blit(Rendering.RenderTargetIdentifier source, Rendering.RenderTargetIdentifier dest, Material mat)
        {
        }

        public void Blit(Rendering.RenderTargetIdentifier source, Rendering.RenderTargetIdentifier dest, Material mat, int pass)
        {
        }

        //Clear all commands in the buffer.
        //Removes all previously added commands from the buffer and makes it empty.
        public void Clear()
        {
        }

       
        //Adds a "clear render target" command. 

        public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor){
            ClearRenderTarget(clearDepth,clearColor,backgroundColor,1.0f);
        }

        public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, float depth){
        }


        // Add a "draw mesh" command. 

        //public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex = 0, int shaderPass = -1, MaterialPropertyBlock properties = null){

        public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass , MaterialPropertyBlock properties){
        }


        //DrawProcedural Add a "draw procedural geometry" command. 
        //DrawProceduralIndirect Add a "draw procedural geometry" command. 

        //DrawRenderer Add a "draw renderer" command. 

        public void DrawRenderer(Renderer renderer, Material material){
            DrawRenderer(renderer, material, 0, -1);
        }

        public void DrawRenderer(Renderer renderer, Material material, int submeshIndex , int shaderPass ){
        }

        // Add a "get a temporary render texture" command. 

        public void GetTemporaryRT(int nameID, int width, int height)
        {
            GetTemporaryRT(nameID,width,height,0,FilterMode.Point,RenderTextureFormat.Default, RenderTextureReadWrite.Default,1);
        }
        
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, FilterMode.Point, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1);
        }

        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer ,  FilterMode filter)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1);
        }

        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer,  FilterMode filter , RenderTextureFormat format)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, RenderTextureReadWrite.Default, 1);
        }

        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer , FilterMode filter , RenderTextureFormat format, RenderTextureReadWrite readWrite )
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, 1);
        }


        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer,  FilterMode filter , RenderTextureFormat format, RenderTextureReadWrite readWrite , int antiAliasing )
        {
        }


        //IssuePluginEvent Send a user-defined event to a native code plugin. 
        //ReleaseTemporaryRT Add a "release a temporary render texture" command. 
        public void ReleaseTemporaryRT(int nameID)
        {
        }

        //SetGlobalColor Add a "set global shader color property" command. 
        //SetGlobalFloat Add a "set global shader float property" command. 
        //SetGlobalMatrix Add a "set global shader matrix property" command. 
        //SetGlobalTexture Add a "set global shader texture property" command. 
        //SetGlobalVector Add a "set global shader vector property" command. 

        public void SetGlobalColor(string name, Color value)
        {
        }

        public void SetGlobalColor(int nameID, Color value)
        {
        }


        //SetRenderTarget Add a "set active render target" command. 
        /*Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, 
         * or one of built-in temporary textures (BuiltinRenderTextureType). All that is expressed by a RenderTargetIdentifier struct, which has implicit conversion operators to save on typing.
        You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        */
        public void SetRenderTarget(Rendering.RenderTargetIdentifier rt){
        }

        public void SetRenderTarget(Rendering.RenderTargetIdentifier color, Rendering.RenderTargetIdentifier depth){
        }
        
        public void SetRenderTarget(RenderTargetIdentifier[] colors, Rendering.RenderTargetIdentifier depth){
        }

        //SetShadowSamplingMode Add a "set shadow sampling mode" command. 

    }
}
