using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public struct Resolution
    {
        public int width;
        public int height;
        public int refreshRate; // the Resolution's vertical refresh rate in Hz. 
    }

    public class Screen
    {
        static Resolution resolution;

        internal static Axiom.Graphics.RenderWindow _renderWindow=null;

        // The current width of the screen window in pixels (Read Only). 
        public static int width { get { return resolution.width; } }
        //  The current height of the screen window in pixels (Read Only). 
        public static int height { get { return resolution.height; } }

        //autorotateToLandscapeLeft Allow auto-rotation to landscape left? 
        //autorotateToLandscapeRight Allow auto-rotation to landscape right? 
        //autorotateToPortrait Allow auto-rotation to portrait? 
        //autorotateToPortraitUpsideDown Allow auto-rotation to portrait, upside down? 
        //currentResolution The current screen resolution (Read Only). 
        public static Resolution currentResolution { get { return resolution;  } }

        //dpi The current DPI of the screen / device (Read Only). 
        //fullScreen Is the game running fullscreen? 
        //orientation Specifies logical orientation of the screen. 
        //resolutions All fullscreen resolutions supported by the monitor (Read Only). 
        //sleepTimeout A power saving setting, allowing the screen to dim some time after the last active user interaction. 
      

        public static void SetResolution(Resolution res) 
        {
            resolution = res;
        }
        public static void SetResolution(int w,int h)
        {
            resolution.width = w;
            resolution.height = h;
        }
    }
}
