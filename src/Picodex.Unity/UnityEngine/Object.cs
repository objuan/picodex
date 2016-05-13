using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public enum HideFlags
    {
        None = 0,// A normal, visible object. This is the default. 
        HideInHierarchy = 1, //The object will not appear in the hierarchy. 
        DontSave = 2,
        HideAndDontSave = 4
    };

    public class Object
    {
        //The name of the object. 
        public string name = "";
        public int id;
        private static int _id = 0;

        // Should the object be hidden, saved with the scene or modifiable by the user? 
        public HideFlags hideFlags;

        public Object()
        {
            id = Object._id++;
        }

        public int GetInstanceID()
        {
            return id;
        }

        public static Object FindObjectOfType(Type type)
        {
            return null;
        }
        public static Object FindObjectOfType<T>()
        {
            return null;
        }

        public static Object[] FindObjectsOfType(Type type)
        {
            return new Object[] { };
        }

        public static Object[] FindObjectsOfType<T>()
        {
            return new Object[] { };
        }

        public static void DontDestroyOnLoad(Object obj)
        {
        }

        public static void Destroy(Object obj)
       {
            Destroy(obj,0.0f);
       }

       public static void Destroy(Object obj, float t )
       {
           if (obj is Camera)
           {
               UnityEngine.Platform.UnityContext.Singleton.CurrentScene.RemoveCamera((Camera)obj);
           }
           if (obj is Light)
           {
               UnityEngine.Platform.UnityContext.Singleton.CurrentScene.RemoveLight((Light)obj);
           }
           if (obj is Renderer)
           {
               UnityEngine.Platform.UnityContext.Singleton.CurrentScene.RemoveRenderer((Renderer)obj);
           }
       }

       public static  void DestroyImmediate(Object obj){
       }

       public static Object Instantiate(Object original)
       {
           return (Object)original.GetType().GetConstructor(new Type[] { }).Invoke(new object[] { });
       }

       public static Object Instantiate(Object original, Vector3 position, Quaternion rotation) {
           return (Object)original.GetType().GetConstructor(new Type[] { }).Invoke(new object[] { });
       } 


    }
}
