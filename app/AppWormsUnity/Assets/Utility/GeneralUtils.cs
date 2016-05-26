//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using UnityEngine;

namespace Picodex
{
    public class GeneralUtils
    {
     
        //public static UnityEditor.EditorWindow GetMainGameView()
        //{
        //    System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        //    System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        //    System.Object Res = GetMainGameView.Invoke(null, null);
        //    return (UnityEditor.EditorWindow)Res;
        //}

        // Unity requires us to use the 'Destroy' function in play mode but 'DestroyImmediate' in edit mode. Thhi function
        // wraps these and calls the appropriate one depending on the current mode. The documentation also clearly states
        // that Destroy() 'will destroy the GameObject, all its components and all transform children of the GameObject',
        // but does not say whether this is also true for 'DestroyImmediate'. We assume it does until we find otherwise...
        public static void DestroyOrDestroyImmediate(Object objectToDestroy)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(objectToDestroy);
            }
            else
            {
                Object.DestroyImmediate(objectToDestroy);
            }
        }

        public static void DestroyImmediateWithChildren(GameObject gameObject)
        {
            Debug.LogWarning("This function will be removed!");
            // Nothing to do is the object is null
            if (gameObject == null)
                return;

            // Find all the child objects
            List<GameObject> childObjects = new List<GameObject>();
            foreach (Transform childTransform in gameObject.transform)
            {
                childObjects.Add(childTransform.gameObject);
            }

            // Destroy all children
            foreach (GameObject childObject in childObjects)
            {
                DestroyImmediateWithChildren(childObject);
            }

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>() as MeshFilter;
            if (meshFilter != null)
            {
                Object.DestroyImmediate(meshFilter.sharedMesh);
                Object.DestroyImmediate(meshFilter);
            }

            // Destroy all components. Not sure if this is useful, or if it happens anyway?
            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                // We can't destroy the transform of a GameObject.
                if (component is Transform == false)
                {
                    Object.DestroyImmediate(component);
                }
            }

            // Destroy the object itself.
            Object.DestroyImmediate(gameObject);
        }
    }
}
