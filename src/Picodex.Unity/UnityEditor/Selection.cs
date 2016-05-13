using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace UnityEditor
{
    public class Selection
    {
        static GameObject _gameObjects;

        static GameObject _activeGameObject;

        public static GameObject activeGameObject
        {
            get
            {
                return _activeGameObject;
            }
            set
            {
                _activeGameObject = value;
            }
        }

        public static GameObject gameObjects
        {
            get
            {
                return gameObjects;
            }
            set
            {
                gameObjects = value;
            }
        }

        //activeGameObject Returns the active game object. (The one shown in the inspector). 
        //activeInstanceID Returns the instanceID of the actual object selection. Includes prefabs, non-modifyable objects. 
        //activeObject Returns the actual object selection. Includes prefabs, non-modifyable objects. 
        //activeTransform Returns the active transform. (The one shown in the inspector). 
        //assetGUIDs Returns the guids of the selected assets. 
        //gameObjects Returns the actual game object selection. Includes prefabs, non-modifyable objects. 
        //instanceIDs The actual unfiltered selection from the Scene returned as instance ids instead of objects. 
        //objects The actual unfiltered selection from the Scene. 
        //selectionChanged Delegate callback triggered when currently active/selected item has changed. 
        //transforms Returns the top level selection, excluding prefabs. 

        //Static Functions
        //Contains Returns whether an object is contained in the current selection. 
        //GetFiltered Returns the current selection filtered by type and mode. 
        //GetTransforms Allows for fine grained control of the selection type using the SelectionMode bitmask. 


    }
}
