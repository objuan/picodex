using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{

    public enum SendMessageOptions
    {
        RequireReceiver = 0, //  A receiver is required for SendMessage. 
        DontRequireReceiver 
    }

    public class Component : Object
    {
        static System.Reflection.BindingFlags flag;

        internal GameObject _gameobject = null;// The game object this component is attached to. A component is always attached to a game object. 

        public GameObject gameObject { get { return _gameobject; } }

        // conversion

        //public static implicit operator GameObject(Component c)
        //{
        //    return c.gameObject;
        //}

        public string tag="";// The tag of this game object. 
        
        //The Transform attached to this GameObject (null if there is none attached). 
        public Transform transform
        {
            get
            {
                return GetComponent<Transform>();
            }
        }

        //  Calls the method named methodName on every MonoBehaviour in this game object or any of its children. 
        public void BroadcastMessage(){
        }

        //CompareTag Is this game object tagged with tag ? 

        //Finds a child by name and returns it. 
        public Component Find(string name)
        {
            if (_gameobject != null) return _gameobject.Find(name);
            else return null;
            // Transform trx = base.GetComponent<Transform>
        }

        // Returns the component of Type type if the game object has one attached, null if it doesn't. 

        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        {
            if (_gameobject != null) return _gameobject.GetComponent<ComponentType>();
            else return default(ComponentType);
        }

        public Component GetComponent(Type type)
        {
            if (_gameobject != null) return _gameobject.GetComponent(type);
            else return null;
        }
            
        // Returns the component of Type type in the GameObject or any of its children using depth first search. 
        public Component GetComponentInChildren(Type type)
        {
            if (_gameobject != null) return _gameobject.GetComponentInChildren(type);
            else return null;
        }
        // Returns the component of Type type in the GameObject or any of its parents. 
        public Component GetComponentInParent(Type type,bool includeInactive)
        {
            if (_gameobject != null) return _gameobject.GetComponentInChildren(type);
            else return null;
        }
        public Component[] GetComponentsInParent(Type type, bool includeInactive)
        {
            if (_gameobject != null) return _gameobject.GetComponentsInParent(type, includeInactive);
            else return new Component[]{};
        }

        //  Returns all components of Type type in the GameObject. 
        public Component[] GetComponents(Type type)
        {
            if (_gameobject != null) return _gameobject.GetComponents(type);
            else return new Component[] { };
        }

        public ComponentType[] GetComponents<ComponentType>() where ComponentType : Component
        {
            if (_gameobject != null) return _gameobject.GetComponents<ComponentType>();
            else return new ComponentType[] { };
        }

        //GetComponentsInChildren Returns all components of Type type in the GameObject or any of its children. 
        //GetComponentsInParent Returns all components of Type type in the GameObject or any of its parents. 
        //SendMessageUpwards Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. 

        public void SendMessage(string methodName)
        {
            SendMessage(methodName, null, SendMessageOptions.RequireReceiver);
        }
        public void SendMessage(string methodName, object value)
        {
            SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
        }
        // Calls the method named methodName on every MonoBehaviour in this game object. 
        public virtual void SendMessage(string methodName, object value, SendMessageOptions options)
        {
            if (_gameobject != null) _gameobject.SendMessage(methodName, value, options);
        }
     
        //   Calls the method named methodName on every MonoBehaviour in this game object or any of its children.

        public void BroadcastMessage(string methodName)
        {
            BroadcastMessage(methodName, null, SendMessageOptions.RequireReceiver);
        }
        public void BroadcastMessage(string methodName, object value)
        {
            BroadcastMessage(methodName, value, SendMessageOptions.RequireReceiver);
        }

        public void BroadcastMessage(string methodName, object parameter, SendMessageOptions options)
        {
            if (_gameobject != null) _gameobject.BroadcastMessage(methodName, parameter, options);
        }

        // AXIOM 

        internal virtual void _OnAttach()
        {
        }

        // received where a oarent change the parent
        internal virtual void _OnLinkTo(Object trasformParent)
        {
        }
    }
}
