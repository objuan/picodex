using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class ScriptableObject : Object
    {
        public ScriptableObject()
        {
        }
        public static ScriptableObject CreateInstance(string className)
        {
            return CreateInstance(Type.GetType(className));
        }

        public static ScriptableObject CreateInstance(Type type)
        {
            return (ScriptableObject)type.GetConstructor(new Type[] { }).Invoke(new Object[] { });
        }

        public static T CreateInstance<T>()
        {
            return (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new Object[] { });
        }


    }
}
