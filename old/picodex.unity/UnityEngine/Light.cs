using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Light : Behaviour
    {
        internal Axiom.Core.Light _light;

        public Light()
        {
            _light = new Axiom.Core.Light();

            UnityEngine.Platform.UnityContext.Singleton.CurrentScene.AddLight(this);
        }

        //        Variables
        //alreadyLightmapped Has the light already been lightmapped. 
        //areaSize The size of the area light. Editor only. 
        //bounceIntensity The multiplier that defines the strength of the bounce lighting. 
        public Color color ; // The color of the light. 
        //commandBufferCount Number of command buffers set up on this light (Read Only). 
        //cookie The cookie texture projected by the light. 
        //cookieSize The size of a directional light's cookie. 
        //cullingMask This is used to light certain objects in the scene selectively. 
        //flare The flare asset to use for this light. 
        //intensity The Intensity of a light is multiplied with the Light color. 
        //range The range of the light. 
        //renderMode How to render the light. 
        //shadowBias Shadow mapping constant bias. 
        //shadowNearPlane Near plane value to use for shadow frustums. 
        //shadowNormalBias Shadow mapping normal-based bias. 
        //shadows How this light casts shadows 
        //shadowStrength Strength of light's shadows. 
        //spotAngle The angle of the light's spotlight cone in degrees. 
        //type The type of the light. 

        //Public Functions
        //AddCommandBuffer Add a command buffer to be executed at a specified place. 
        //GetCommandBuffers Get command buffers to be executed at a specified place. 
        //RemoveAllCommandBuffers Remove all command buffers set on this light. 
        //RemoveCommandBuffer Remove command buffer from execution at a specified place. 
        //RemoveCommandBuffers Remove command buffers from execution at a specified place. 

    }
}
