using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Platform;
using UnityEngine.Rendering;

namespace UnityEngine
{
    public enum CameraType
    {
        Game ,//Used to indicate a regular in-game camera. 
        SceneView,// Used to indicate that a camera is used for rendering the Scene View in the Editor. 
        Preview 
    }
     
    public enum ClearFlags
    {
        Skybox = 1 , // Clear with the skybox. 
        SolidColor=2,// Clear with a background color. 
        Depth = 4,// Clear only the depth buffer. 
        Nothing  = 0
    };

    public enum RenderingPath
    {
        Forward,
        Deferred
    }

    public class Camera :  Behaviour
    {
        public delegate void CameraCallback(Camera cam);

        public Axiom.Core.Camera _camera;
        public Axiom.Core.Viewport _viewport;

        // STATIC

        // Returns all enabled cameras in the scene. 
        public static Camera[] allCameras
        {
           get{
               return UnityContext.Singleton.CurrentScene.allCameras;
           }
        }
        //  The number of cameras in the current scene. 
        public static int  allCamerasCount
        {
            get
            {
                return UnityContext.Singleton.CurrentScene.allCamerasCount;
            }
        }
        // The camera we are currently rendering with, for low-level render control only (Read Only). 
        public static Camera current
        {
            get
            {
                return UnityRenderPipeline.Singleton.CurrentCamera;
            }
        }

        // The first enabled camera tagged "MainCamera" (Read Only). 
        public static Camera main
        {
            get
            {
                return UnityContext.Singleton.CurrentScene.MainCamera;
            }
        }

        public static event Camera.CameraCallback onPostRender;
        public static event Camera.CameraCallback onPreCull;
        public static event Camera.CameraCallback onPreRender; 

        //VARS

        // The aspect ratio (width divided by height). 
        public float aspect 
        {
            get 
            {
                return (float)Screen.width / (float)Screen.height; 
            } 
        }
        //  The color with which the screen will be cleared. 
        public Color backgroundColor
        {
            get
            {
                return _viewport.BackgroundColor;
            }
            set
            {
                _viewport.BackgroundColor = value;
            }
        }
//        actualRenderingPath The rendering path that is currently being used (Read Only).The actual rendering path might be different from the user-specified renderingPath if the underlying gpu/platform does not support the requested one, or some other situation caused a fallback (for example, deferred rendering is not supported with orthographic projection cameras). 

        //Matrix that transforms from camera space to world space (Read Only). 
        // Note that camera space matches OpenGL convention: 
        // camera's forward is the negative Z axis. This is different from Unity's convention, where forward is the positive Z axis.

        public Matrix4x4 cameraToWorldMatrix
        {
            get
            {
                //TODO
                return _camera.ViewMatrix.Inverse();
            }
        }

        // Matrix that transforms from world to camera space. 
        public Matrix4x4 worldToCameraMatrix
        {
            get
            {
                return _camera.ViewMatrix;
            }
            set
            {
               _camera.SetCustomViewMatrix(true,value);
            }
        }

        public Matrix4x4 ViewMatrix
        {
            get
            {
                return worldToCameraMatrix; //TODO
            }
        }

        //cameraType Identifies what kind of camera this is. 
        public CameraType cameraType = CameraType.Game;
        //clearFlags How the camera clears the background. 
        public ClearFlags clearFlags = ClearFlags.Depth | ClearFlags.SolidColor;
//clearStencilAfterLightingPass Should the camera clear the stencil buffer after the deferred light pass? 
//commandBufferCount Number of command buffers set up on this camera (Read Only). 
//cullingMask This is used to render parts of the scene selectively. 
//depth Camera's depth in the camera rendering order. 
//depthTextureMode How and if camera generates a depth texture. 
//eventMask Mask to select which layers can trigger events on the camera. 
        //farClipPlane The far clipping plane distance. 
        public float farClipPlane { get { return _camera.Far; } set { _camera.Far = value; } }
        //fieldOfView The field of view of the camera in degrees. 
        public float fieldOfView{ get{return _camera.FieldOfView; } set{ _camera.FieldOfView= value;}}
//hdr High dynamic range rendering. 
//layerCullDistances Per-layer culling distances. 
//layerCullSpherical How to perform per-layer culling for a Camera. 
        //nearClipPlane The near clipping plane distance. 
        public float nearClipPlane { get { return _camera.Near; } set { _camera.Near = value; } }
//opaqueSortMode Opaque object sorting mode. 
//orthographic Is the camera orthographic (true) or perspective (false)? 
//orthographicSize Camera's half-size when in orthographic mode. 

        // Where on the screen is the camera rendered in pixel coordinates. 
        public Rect pixelRect  //TODO
        {
            get { return new Rect(_viewport.ActualLeft,_viewport.ActualTop, _viewport.ActualWidth, _viewport.ActualHeight); }
        }

        public int pixelHeight{ get{return Screen.height;}} //How tall is the camera in pixels (Read Only). 
        
        public int pixelWidth{ get{return Screen.width;}} // How wide is the camera in pixels (Read Only). 

      
        public Matrix4x4 projectionMatrix
        {
            get{
                return _camera.ProjectionMatrix;
            }
            set
            {
                _camera.SetCustomProjectionMatrix(true, value);
            }
        }

        // Where on the screen is the camera rendered in normalized coordinates. 
        // The values in rect range from zero (left/bottom) to one (right/top).
        public Rect rect
        {
            get
            {
                return new Rect(0,0,1,1);
            }
        }

        public RenderingPath renderingPath = RenderingPath.Forward; // The rendering path that should be used, if possible.In some situations, it may not be possible to use the rendering path specified, in which case the renderer will automatically use a different path. For example, if the underlying gpu/platform does not support the requested one, or some other situation caused a fallback (for example, deferred rendering is not supported with orthographic projection cameras).For this reason, we also provide the read-only property actualRenderingPath which allows you to discover which path is actually being used. 

//stereoConvergence Distance to a point where virtual eyes converge. 
//stereoEnabled Stereoscopic rendering. 
//stereoMirrorMode Render only once and use resulting image for both eyes. 
//stereoSeparation Distance between the virtual eyes. 
//targetDisplay Set the target display for this Camera. 
        //targetTexture Destination render texture. 
        public  RenderTexture targetTexture;
//transparencySortMode Transparent object sorting mode. 
//useOcclusionCulling Whether or not the Camera will use occlusion culling during rendering. 
//velocity Get the world-space speed of the camera (Read Only). 

        Shader replacementShader = null;

        //METHODS

        public Camera()
        {
            _camera = UnityContext.Singleton.CurrentScene._sceneManager.CreateCamera("MainCamera");
            _camera.AutoAspectRatio = true;

            _viewport = Screen._renderWindow.AddViewport(_camera, 0, 0, 1.0f, 1.0f, 100);

            _viewport.BackgroundColor = Color.Black; ;

            //TODO ??
            //this.AddComponent(new Transform());
        }

        // Fills an array of Camera with the current cameras in the scene, without allocating a new array. 
        public static void GetAllCameras(Camera[] cameras)
        {
            UnityEngine.Platform.UnityContext.Singleton.CurrentScene.GetAllCameras(cameras);
        }

        //  Add a command buffer to be executed at a specified place. 
        public void AddCommandBuffer(CameraEvent evt,CommandBuffer cmd)
        {
        }

        
        //CalculateObliqueMatrix Calculates and returns oblique near-plane projection matrix. 
        //CopyFrom Makes this camera's settings match other camera. 
        //GetCommandBuffers Get command buffers to be executed at a specified place. 
        //RemoveAllCommandBuffers Remove all command buffers set on this camera. 
        
        // Remove command buffer from execution at a specified place. 

        public void RemoveCommandBuffer(CameraEvent evt, CommandBuffer cmd)
        {
        }

        //RemoveCommandBuffers Remove command buffers from execution at a specified place. 

        //Render Render the camera manually.
        // The camera will send OnPreCull, OnPreRender and OnPostRender to 
        //any scripts attached, and render any eventual image filters.

        public void Render(){
            UnityRenderPipeline.Singleton.Render(this);
        }

//RenderToCubemap Render into a static cubemap from this camera. 

        //RenderWithShader Render the camera with shader replacement. 
        //This will render the camera. It will use the camera's clear flags, target texture and all other settings.
        //The camera will not send OnPreCull, OnPreRender or OnPostRender to attached scripts. Image filters will not be rendered either.
        public void RenderWithShader(Shader shader, string replacementTag)
        {
        }

//ResetAspect Revert the aspect ratio to the screen's aspect ratio. 
//ResetFieldOfView Reset to the default field of view. 
        // Make the projection reflect normal camera's parameters. 
        public void ResetProjectionMatrix()
        {
            _camera.SetCustomProjectionMatrix(false);
        }
        // Remove shader replacement from camera. 
        public void ResetReplacementShader()
        {
            this.replacementShader = null;
        }

//ResetStereoProjectionMatrices Use the default projection matrix for both stereo eye. Only work in 3D flat panel display. 
//ResetStereoViewMatrices Use the default view matrix for both stereo eye. Only work in 3D flat panel display. 
        // Make the rendering position reflect the camera's position in the scene. 
        public void ResetWorldToCameraMatrix()
        {
            _camera.SetCustomViewMatrix(false);
        }

//ScreenPointToRay Returns a ray going from camera through a screen point. 
//ScreenToViewportPoint Transforms position from screen space into viewport space. 
//ScreenToWorldPoint Transforms position from screen space into world space. 
        //Make the camera render with shader replacement. 
        public void SetReplacementShader(Shader replacementShader)
        {
            this.replacementShader = replacementShader;
        }

//SetStereoProjectionMatrices Define the projection matrix for both stereo eye. Only work in 3D flat panel display. 
//SetStereoViewMatrices Define the view matrices for both stereo eye. Only work in 3D flat panel display. 
//SetTargetBuffers Sets the Camera to render to the chosen buffers of one or more RenderTextures. 
//ViewportPointToRay Returns a ray going from camera through a viewport point. 
//ViewportToScreenPoint Transforms position from viewport space into screen space. 

        //ViewportToWorldPoint Transforms position from viewport space into world space. 
        public Vector3 ViewportToWorldPoint(Vector3 position)
        {
            return position;
        }

//WorldToScreenPoint Transforms position from world space into screen space. 
//WorldToViewportPoint Transforms position from world space into viewport space. 

        // MESSAGES

        //OnPostRender OnPostRender is called after a camera has finished rendering the scene. 
        //OnPreCull OnPreCull is called before a camera culls the scene. 
        //OnPreRender OnPreRender is called before a camera starts rendering the scene. 
        //OnRenderImage OnRenderImage is called after all rendering is complete to render image. 
        //OnRenderObject OnRenderObject is called after camera has rendered the scene. 
        //OnWillRenderObject OnWillRenderObject is called once for each camera if the object is visible. 


        // AXIOM


        internal override void _OnAttach()
        {
            transform._node.AttachObject(_camera);

            //fieldOfView = 45;
            //nearClipPlane = 0.1f;
            //farClipPlane = 1000.0f;
            //_camera.AspectRatio = aspect;
        }
    }
}
