using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
 
    public enum KeyCode
    {
        LeftAlt,
        LeftControl
    }

    public struct MouseState
    {
        public int X;
        public int Y;
        public int Z;
        public bool[] buttons;

        public void init()
        {
            X = Y = Z = 0;
            buttons = new bool[2] { false, false };
        }
    }

    public static class Input
    {
        private static MouseState lastState;
        private static MouseState currentState;
        private static MouseState deltaState;

        static Input()
        {
            lastState.init();
            currentState.init();
            deltaState.init();
        }

        internal static void changeState(MouseState mouseState)
        {
            currentState = mouseState;
            deltaState.X = currentState.X - lastState.X;
            deltaState.Y = currentState.Y - lastState.Y;
            deltaState.Z = currentState.Z - lastState.Z;
    
            deltaState.buttons[0] = currentState.buttons[0] != lastState.buttons[0];
            deltaState.buttons[1] = currentState.buttons[1] != lastState.buttons[1];
            lastState = mouseState;
        }

        //  Returns the value of the virtual axis identified by axisName. 
        public static float GetAxis(String axisName)
        {
            if (axisName == "Mouse X") return deltaState.X * 0.01f;
            if (axisName == "Mouse Y") return deltaState.Y * 0.01f;
            if (axisName == "Mouse ScrollWheel") return deltaState.Z * 0.01f;
            return 0;
        }

        //public static GetAccelerationEvent()// Returns specific acceleration measurement which occurred during last frame. (Does not allocate temporary variables). 
        //{
        //}
      //  public static float GetAxis Returns the value of the virtual axis identified by axisName. 
      //  public static float GetAxisRaw Returns the value of the virtual axis identified by axisName with no smoothing filtering applied. 

        // Returns true while the virtual button identified by buttonName is held down. 
        public static bool GetButton(String buttonName)
        {
            return currentState.buttons[0];
        }
        // Returns true during the frame the user pressed down the virtual button identified by buttonName. 
        public static bool GetButtonDown (String buttonName)
        {
            return deltaState.buttons[0] && currentState.buttons[0];
        }
        //  Returns true the first frame the user releases the virtual button identified by buttonName. 
        public static bool GetButtonUp(String buttonName)
        {
            return deltaState.buttons[0] && !currentState.buttons[0];
        }

        //   Returns whether the given mouse button is held down. 
        public static bool GetMouseButton(int idx)
        {
            return currentState.buttons[idx];
        }
        //  Returns true during the frame the user pressed the given mouse button. 
        public static bool GetMouseButtonDown(int idx)
        {
            return deltaState.buttons[idx] && currentState.buttons[idx];
        }
        // Returns true during the frame the user releases the given mouse button. 
        public static bool GetMouseButtonUp(int idx)
        {
            return deltaState.buttons[idx] && !currentState.buttons[idx];
        }

        public static bool GetKey(KeyCode code)
        {
            return false;
        }

        //public static float GetJoystickNames Returns an array of strings describing the connected joysticks. 
        //public static float GetKey Returns true while the user holds down the key identified by name. Think auto fire. 
        //public static float GetKeyDown Returns true during the frame the user starts pressing down the key identified by name. 
        //public static float GetKeyUp Returns true during the frame the user releases the key identified by name. 
         //public static float GetTouch Returns object representing status of a specific touch. (Does not allocate temporary variables). 
        //public static float IsJoystickPreconfigured Determine whether a particular joystick model has been preconfigured by Unity. (Linux-only). 
        //public static float ResetInputAxes Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame. 

    }
}
