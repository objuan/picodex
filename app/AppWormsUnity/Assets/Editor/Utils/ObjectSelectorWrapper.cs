using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

namespace Picodex
{
    public static class ObjectSelectorWrapper
    {
        private static System.Type TT;
        private static bool oldState = false;

        static ObjectSelectorWrapper()
        {
            TT = System.Type.GetType("UnityEditor.ObjectSelector,UnityEditor");
        }

        private static EditorWindow Get()
        {
            PropertyInfo P = TT.GetProperty("get", BindingFlags.Public | BindingFlags.Static);
            return P.GetValue(null, null) as EditorWindow;
        }
        public static void ShowSelector(System.Type aRequiredType)
        {
            MethodInfo ShowMethod = TT.GetMethod("Show", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            ShowMethod.Invoke(Get(), new object[] { null, aRequiredType, null, true });
        }
        public static T GetSelectedObject<T>() where T : UnityEngine.Object
        {
            MethodInfo GetCurrentObjectMethod = TT.GetMethod("GetCurrentObject", BindingFlags.Static | BindingFlags.Public);
            return GetCurrentObjectMethod.Invoke(null, null) as T;
        }
        public static bool isVisible
        {
            get
            {
                PropertyInfo P = TT.GetProperty("isVisible", BindingFlags.Public | BindingFlags.Static);
                return (bool)P.GetValue(null, null);
            }
        }
        public static bool HasJustBeenClosed()
        {
            bool visible = isVisible;
            if (visible != oldState && visible == false)
            {
                oldState = false;
                return true;
            }
            oldState = visible;
            return false;
        }
    }
}