using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class GameObject : Object
    {
        static System.Reflection.BindingFlags flag;

        static GameObject()
        {
            flag = System.Reflection.BindingFlags.Default;
            flag |= System.Reflection.BindingFlags.Instance;
            flag |= System.Reflection.BindingFlags.NonPublic;
            flag |= System.Reflection.BindingFlags.Public;
        }

        private List<Component> componentList = new List<Component>();
        private bool _activeSelf=true;

        public GameObject gameObject { get { return this; } }
       
        //TODO
        // Scene that the GameObject is part of. 
        // solo per la root
        public UnityEngine.SceneManagement.Scene _rootScene = null;

        // ======================================

        //Is the GameObject active in the scene? 
        public bool activeInHierarchy
        {
            get
            {
                return GetComponent<Transform>().root != null;
            }
        }
        //;//The local active state of this GameObject. (Read Only) 
        public bool activeSelf
        {
            get
            {
                return _activeSelf;
            }
        }

        // Scene that the GameObject is part of. 
        public UnityEngine.SceneManagement.Scene scene
        {
            get { return (transform.parent != null) ? transform.parent.gameObject.scene : _rootScene; }
        }

       // isStatic Editor only API that specifies if a game object is static. 
        //  The layer the game object is in. A layer is in the range [0...31]. 
        public int layer;

        public string tag="";// The tag of this game object. 

        public Transform transform
        {
            get
            {
                return GetComponent<Transform>();
            }
        }
        public Renderer renderer
        {
            get
            {
                return GetComponent<Renderer>();
            }
        }

        public GameObject()
        {
            // INIZIO metto le cose di base
          //  this.AddComponent(new MeshFilter());
            this.AddComponent(new Transform());
        }

        public GameObject(String name)
        {
             // INIZIO metto le cose di base
            // this.AddComponent(new MeshFilter());
             this.AddComponent(new Transform());
             this.name = name;
        }

        public static GameObject CreateNull()
        {
            GameObject go = new GameObject();
            go.AddComponent(new Transform());
            return go;
        }

        public static GameObject CreateNull(String name)
        {
            GameObject go = new GameObject(name);
            go.AddComponent(new Transform());
            return go;
        }

        public static GameObject CreatePrimitive(PrimitiveType type)
        {
            GameObject go = new GameObject();
           // go.AddComponent(new Transform());
            go.AddComponent(new MeshRenderer());
            go.AddComponent(new MeshFilter());
            go.GetComponent<MeshFilter>().mesh = PrimitiveBuilder.CreateMesh(type);
            return go;
        }

        // ========================

        // Adds a component class named className to the game object.
        public void AddComponent(Component component)
        {
            ((Component)component)._gameobject = this;
            componentList.Add(component);

            component._OnAttach();
        }

        public Component Find(string name)
        {
            foreach (Component c in componentList)
            {
                if (c.name == name) return c;
            }
            return null;
        }

        // Returns the component of Type type if the game object has one attached, null if it doesn't. 
        public Component GetComponent(Type type)
        {
           foreach(Component c in componentList)
            {
                if (c.GetType() == type) return c;
            }
           return null;
        }

        // Returns the component of Type type in the GameObject or any of its children using depth first search. 
        public Component GetComponentInChildren(Type type)
        {
            foreach (Component c in componentList)
            {
                if (c.GetType() == type) return c;
            }
            foreach (Component c in componentList)
            {
                Component cc = c.GetComponentInChildren(type);
                if (cc != null) return cc;
            }
            return null;
        }
        // Returns the component of Type type in the GameObject or any of its parents. 
        public Component GetComponentInParent(Type type, bool includeInactive)
        {
            foreach (Component c in componentList)
                if (c.GetType() == type) return c;
    
            if (transform.parent != null)
                return transform.parent.gameObject.GetComponentInParent(type, includeInactive);
            else
                return null;
        }

        public ComponentType GetComponentInParent<ComponentType>(bool includeInactive)
        {
            foreach (Component c in componentList)
                if (c.GetType() == typeof(ComponentType)) return (ComponentType)(object)c; 

            if (transform.parent != null)
                return transform.parent.gameObject.GetComponentInParent<ComponentType>(includeInactive);
            else
                return default(ComponentType);
        }

        public Component[] GetComponentsInParent(Type type, bool includeInactive)
        {
            List<Component> list = new List<Component>();
            foreach (Component c in componentList)
                if (c.GetType() == type) list.Add(c);

            if (transform.parent != null)
            {
                Component[] newList = transform.parent.gameObject.GetComponentsInParent(type, includeInactive);
                foreach (Component c in newList) list.Add(c);
            }
            return list.ToArray();

        }

        //  Returns all components of Type type in the GameObject. 
        public Component[] GetComponents(Type type)
        {
            List<Component> list = new List<Component>();
            foreach (Component c in componentList)
            {
                if (c.GetType() == type) list.Add(c);
            }
            return list.ToArray();
        }

        // ====================================================

        public ComponentType AddComponent<ComponentType>() where ComponentType : Component
        {
            ComponentType obj = (ComponentType)(typeof(ComponentType).GetConstructor(new Type[] { }).Invoke(new object[] { }));
            AddComponent(obj);
            return obj;
        }


        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        {
            foreach (Component c in componentList)
            {
                if (c is ComponentType) return (ComponentType)(object)c;
            }
            return default(ComponentType);
        }

        public ComponentType[] GetComponents<ComponentType>() where ComponentType : Component
        {
            List<ComponentType> list = new List<ComponentType>();
            foreach (Component c in componentList)
            {
                if (c is ComponentType) list.Add((ComponentType)c);
            }
            return list.ToArray();
        }
      // 
        //CompareTag Is this game object tagged with tag ? 

        //GetComponent Returns the component of Type type if the game object has one attached, null if it doesn't. 
        //GetComponentInChildren Returns the component of Type type in the GameObject or any of its children using depth first search. 
        //GetComponentInParent Returns the component of Type type in the GameObject or any of its parents. 
        //GetComponents Returns all components of Type type in the GameObject.

        public Component[] GetComponents()
        {
            return componentList.ToArray();
        }

        //GetComponentsInChildren Returns all components of Type type in the GameObject or any of its children. 
        //GetComponentsInParent Returns all components of Type type in the GameObject or any of its parents. 
       
      //  SendMessageUpwards Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour. 

        public void SetActive ()
        {
            _activeSelf = true;
        }

        public void SendMessage(string methodName)
        {
            SendMessage(methodName, null, SendMessageOptions.RequireReceiver);
        }

        public void SendMessage(string methodName, object value)
        {
            SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
        }

        // SendMessage Calls the method named methodName on every MonoBehaviour in this game object. 

        public void SendMessage(string methodName, object value, SendMessageOptions options)
        {
           // base.SendMessage(methodName, value, options);
            foreach (Component c in componentList)
            {
                if (c is MonoBehaviour)
                {
                    try
                    {
                        System.Reflection.MethodInfo method = ((MonoBehaviour)c).GetType().GetMethod(methodName,flag);
                        if (method != null)
                        {
                            if (value == null)
                                method.Invoke(c, new object[] { });
                            else
                                method.Invoke(c, new object[] { value });
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }
            }
        }

        public void BroadcastMessage(string methodName)
        {
            BroadcastMessage(methodName, null, SendMessageOptions.RequireReceiver);
        }
        public void BroadcastMessage(string methodName, object value)
        {
            BroadcastMessage(methodName, value, SendMessageOptions.RequireReceiver);
        }

        //  BroadcastMessage Calls the method named methodName on every MonoBehaviour in this game object or any of its children. 
        public void BroadcastMessage(string methodName, object parameter, SendMessageOptions options)
        {
            foreach(Component c in componentList)
            {
                //c.BroadcastMessage(methodName, parameter, options);

                if (c is MonoBehaviour)
                {
                    try
                    {
                        System.Reflection.MethodInfo method = ((MonoBehaviour)c).GetType().GetMethod(methodName, flag);
                        if (method != null)
                        {
                            if (parameter == null)
                                method.Invoke(c, new object[] { });
                            else
                                method.Invoke(c, new object[] { parameter });
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }
                else if (c is Transform)
                {
                    Transform trx = c as Transform;
                    int cc = trx.childCount;
                    for (int i = 0; i < cc; i++)
                    {
                        if (trx.GetChild(i).gameObject != null)
                            trx.GetChild(i).gameObject.BroadcastMessage(methodName, parameter, options);
                    }
                }
            }
            
        }

        // 
        //  BroadcastMessage Calls the method named methodName on every MonoBehaviour in this game object or any of its children. 
        internal void _BroadcastMessageToComponent(string methodName, Object parameter, SendMessageOptions options)
        {
            foreach (Component c in componentList)
            {
                try
                {
                    if (parameter == null)
                    {
                        System.Reflection.MethodInfo method = ((Component)c).GetType().GetMethod(methodName, flag);
                        if (method != null)
                        {
                            method.Invoke(c, new object[] { });

                        }
                    }
                    else
                    {
                        System.Reflection.MethodInfo method = ((Component)c).GetType().GetMethod(methodName, flag);
                        if (method != null)
                        {
                            method.Invoke(c, new object[] { parameter });
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }
                if (c is Transform)
                {
                    Transform trx = c as Transform;
                    int cc = trx.childCount;
                    for (int i = 0; i < cc; i++)
                    {
                        if (trx.GetChild(i).gameObject != null)
                            trx.GetChild(i).gameObject._BroadcastMessageToComponent(methodName, parameter, options);
                    }
                }
            }

        }
    }
}
