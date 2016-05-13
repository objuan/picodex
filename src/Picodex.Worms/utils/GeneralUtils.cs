using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace picodex.utils
{
    public class GeneralUtils
    {
        public static UnityEditor.EditorWindow GetMainGameView()
        {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetMainGameView.Invoke(null, null);
            return (UnityEditor.EditorWindow)Res;
        }
    }
}
