using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.OpenGL;
using OGL = OpenTK.Graphics.OpenGL.GL;

namespace UnityEngine
{
    /*GL immediate drawing functions use whatever is the "current material" set up right now (see Material.SetPass). The material controls how the rendering is done (blending, textures, etc.), so unless you explicitly set it to something before using GL draw functions, the material can happen to be anything. Also, if you call any other drawing commands from inside GL drawing code, they can set material to something else, so make sure it's under control as well.

GL drawing commands execute immediately. That means if you call them in Update(), they will be executed before the camera is rendered (and the camera will most likely clear the screen, making the GL drawing not visible).

*/

    public static class GL
    {
        private static Stack<Matrix4x4> modelViewStack = new Stack<Matrix4x4>();
        private static Stack<Matrix4x4> projectionStack = new Stack<Matrix4x4>();
        private static Matrix4x4 _modelview;
        private static Matrix4x4 _projection;

        private static Color old_backgroundColor = new Color();
        private static float old_depth = -1;

       //MatriSt

       //  Static Variables

        public static int LINES = 0; // Mode for Begin: draw lines. 
        public static int QUADS = 1;// Mode for Begin: draw quads. 
        public static int TRIANGLE_STRIP = 2;//Mode for Begin: draw triangle strip. 
        public static int TRIANGLES = 3;//Mode for Begin: draw triangles. 

        public static bool invertCulling;// Select whether to invert the backface culling (true) or not (false). 
        // // The current modelview matrix. 
        public static Matrix4x4 modelview{
            get{return _modelview;}
            set{
                _modelview = value;

                unsafe
                {
                    OGL.MatrixMode(MatrixMode.Modelview);
                    OGL.LoadMatrix(&value.m00);
                }
            }
        }
        // // The current projection matrix. 
        public static Matrix4x4 projection{
         get{return _projection;}
            set{ 
                _projection = value;

                unsafe
                {
                    OGL.MatrixMode(MatrixMode.Projection);
                    OGL.LoadMatrix(&value.m00);
                }
            }
        }
        public static bool sRGBWrite;// Controls whether Linear-to-sRGB color conversion is performed while rendering. 
        public static bool  wireframe;// Should rendering be done in wireframe? 

        internal static Axiom.Graphics.RenderSystem renderSystem;

        static GL()
        {
            _modelview = Matrix4x4.Identity;
            _projection = Matrix4x4.Identity;
        }

        // Static Functions

        //  Begin drawing 3D primitives. 
        public static void Begin(int mode)
        {
            switch (mode)
            {
                case 0: OGL.Begin(BeginMode.Lines); break;
                case 1: OGL.Begin(BeginMode.Quads); break;
                case 2: OGL.Begin(BeginMode.TriangleStrip); break;
                case 3: OGL.Begin(BeginMode.Triangles); break;
            }
        
        }

        //End End drawing 3D primitives. 
        public static void End()
        {
            OGL.End();
        }
        
        // Clear the current render buffer. 
        public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
        {
            Clear(clearDepth, clearColor, backgroundColor, 1.0f);
        }

        public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, float depth )
        {
            ClearBufferMask mask = 0;// ClearBufferMask.None;
            if (clearDepth) mask |= ClearBufferMask.DepthBufferBit;
            if (clearColor) mask |= ClearBufferMask.ColorBufferBit;
            if (old_backgroundColor.getHash() != backgroundColor.getHash())
            {
                old_backgroundColor = backgroundColor;
                OGL.ClearColor(backgroundColor.r, backgroundColor.g, backgroundColor.b, backgroundColor.a);
            }
            if (depth != old_depth)
            {
                old_depth = depth;
                OGL.ClearDepth(depth);
            }
            OGL.Clear(mask);
        }

        // Clear the current render buffer with camera's skybox. 
        public static void ClearWithSkybox()
        {
        }
 
        /*
         * In Unity, projection matrices follow OpenGL convention. However on some platforms they have to be transformed a bit to match the native API requirements. Use this function to calculate how the final projection matrix will be like. The value will match what comes as UNITY_MATRIX_P matrix in a shader.
        The renderIntoTexture value should be set to true if you intend to render into a RenderTexture with this projection matrix. On some platforms it affects how the final matrix will look like.
        */
        // Compute GPU projection matrix from camera's projection matrix. 
        public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
        {
            return new Matrix4x4(proj);
        }

        // Invalidate the internally cached render state. 
        public static void InvalidateState()
        {
        }

        // Load the identity matrix to the current modelview matrix. 
        public static void LoadIdentity()
        {
            modelview = Matrix4x4.Identity;
        }

        // Helper function to set up an ortho perspective transform. 
        // After calling LoadOrtho, the viewing frustum goes from (0,0,-1) to (1,1,100).
        //LoadOrtho can be used for drawing primitives in 2D.

        public static void LoadOrtho()
        {
            //OGL.Ortho(0, 1, -1, 0, 1, 100);
            projection = Matrix4x4.Ortho(0, 1, -1, 0, 1, 100);
        }

        // Setup a matrix for pixel-correct rendering.
        // This sets up modelview and projection matrices so that X, Y coordinates map directly to pixels. 
        // The (0,0) is at the bottom left corner of current camera's viewport. The Z coordinate goes from -1 to +1.
        //  This function overrides current camera's parameters, so most often you want to save and restore matrices 
        // using GL.PushMatrix and GL.PopMatrix.
        public static void LoadPixelMatrix()
        {
            //OGL.Ortho(0, Screen.width, -1,0, Screen.height, 1);
            projection = Matrix4x4.Ortho(0, Screen.width, -1, 0, Screen.height, 1);
        }

        /*Load an arbitrary matrix to the current projection matrix.
        This function overrides current camera's projection parameters, so most often you want to save and restore projection matrix using GL.PushMatrix and GL.PopMatrix.
        */
        public static void LoadProjectionMatrix(Matrix4x4 mat)
        {
            projection = mat;
        }

      
        // Multiplies the current modelview matrix with the one specified. 
        public static void MultMatrix(Matrix4x4 mat)
        {
            modelview = mat * modelview;
        }
        
//        Saves both projection and modelview matrices to the matrix stack.
  //      Changing modelview or projection matrices overrides current camera's parameters. These matrices can be saved and restored using GL.PushMatrix and GL.PopMatrix.
        public static void PushMatrix()
        {
            modelViewStack.Push(modelview);
            projectionStack.Push(projection);
            //OGL.PopMatrix();
        }

        // Restores both projection and modelview matrices off the top of the matrix stack. 
        public static void PopMatrix()
        {
            Debug.Assert(modelViewStack.Count > 0);
            modelview = modelViewStack.Pop();
            projection = projectionStack.Pop();
        }

        //Viewport Set the rendering viewport. 
        // All rendering is constrained to be inside the passed pixelRect. 
        // If the Viewport is modified, all the rendered content inside of it gets stretched.

        public static void Viewport(Rect pixelRect)
        {
            OGL.Viewport(pixelRect.x, pixelRect.y, pixelRect.width, pixelRect.height);
        }

        //RenderTargetBarrier Resolves the render target for subsequent operations sampling from it. 
        // At the moment the advanced OpenGL blend operations are the only case requiring this barrier.
        public static void RenderTargetBarrier()
        {
        }

        /*
       * Sets current texture coordinate (v.x,v.y,v.z) to the actual texture unit.
          In OpenGL this matches glMultiTexCoord for the given texture unit if multi-texturing is available. 
       * On other graphics APIs the same functionality is emulated.

          The Z component is used only when:
          1. You access a cubemap (which you access with a vector coordinate, hence x,y & z).
          2. You do "projective texturing", where the X & Y coordinates are divided by Z to get the final coordinate. This would be mostly useful for water reflections and similar things.

          This function can only be called between GL.Begin and GL.End functions.
          */
        public static void MultiTexCoord(int unit, Vector3 v)
        {
            OGL.MultiTexCoord3(TextureUnit.Texture0, v.x, v.y, v.z);
        }
        // Sets current texture coordinate (x,y) for the actual texture unit. 
        public static void MultiTexCoord2(int unit, float x, float y)
        {
            OGL.MultiTexCoord2(TextureUnit.Texture0, x, y);
        }
        // Sets current texture coordinate (x,y,z) to the actual texture unit. 
        public static void MultiTexCoord3(int unit, float x, float y, float z)
        {
            OGL.MultiTexCoord3(TextureUnit.Texture0, x, y, z);
        }

        //TexCoord Sets current texture coordinate (v.x,v.y,v.z) for all texture units. 
        public static void TexCoord(Vector3 v)
        {
            OGL.TexCoord3(v.x,v.y,v.z);
        }
        //TexCoord2 Sets current texture coordinate (x,y) for all texture units. 
        public static void TexCoord2(float x, float y)
        {
            OGL.TexCoord2(x, y);
        }
        //TexCoord3 Sets current texture coordinate (x,y,z) for all texture units. 
        public static void TexCoord3(float x, float y, float z)
        {
            OGL.TexCoord3(x,y,z);
        }

        //Vertex Submit a vertex. 
        public static void Vertex(Vector3 v)
        {
             OGL.Vertex3(v.x, v.y, v.z);
        }
        //Vertex3 Submit a vertex. 
        public static void Vertex3(float x, float y, float z)
        {
            OGL.Vertex3(x, y, z);
        }

        // Sets current vertex color. 
        public static void Color(Color color)
        {
            OGL.Color4(color.r, color.g, color.b, color.a);
        }

     
    }
}
