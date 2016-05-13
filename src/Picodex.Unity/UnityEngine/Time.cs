using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Time
    {
       // public static float deltaTime;

        public static int captureFramerate ;//Slows game playback time to allow screenshots to be saved between frames. 
        public static float deltaTime = 0;//The time in seconds it took to complete the last frame (Read Only). 
        public static float fixedDeltaTime = 0;//The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate) are performed. 
        public static float fixedTime = 0;// The time the latest FixedUpdate has started (Read Only). This is the time in seconds since the start of the game. 
        public static int frameCount = 0;// The total number of frames that have passed (Read Only). 
        public static float maximumDeltaTime ;//The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's FixedUpdate). 
        public static float realtimeSinceStartup ;//The real time in seconds since the game started (Read Only). 
        public static float smoothDeltaTime;// A smoothed out Time.deltaTime (Read Only). 
        public static double time=0;// The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game. 
        public static float timeScale=0;// The scale at which the time is passing. This can be used for slow motion effects. 
        public static float timeSinceLevelLoad = 0;//The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded. 
        public static float unscaledDeltaTime = 0;//The timeScale-independent time in seconds it took to complete the last frame (Read Only). 
        public static float unscaledTime = 0;//

    }
}
