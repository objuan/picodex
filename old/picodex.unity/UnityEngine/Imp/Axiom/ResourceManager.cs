using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Axiom.Graphics;
using ResourceHandle = System.UInt64;

namespace Axiom.Core
{
  
   
    public class Resource
    {
        protected virtual void load()
        {
        }
        protected virtual void unload()
        {
        }

        public virtual void Touch()
        {
        }
        protected virtual void dispose(bool disposeManagedResources)
        {
        }
        protected virtual int calculateSize()
        {
            return 0;
        }
    }


    public class ResourceManager
    {
        protected virtual void dispose(bool disposeManagedResources)
        {
        }
        public virtual void ParseScript(System.IO.Stream data, string groupName, string fileName)
        {
        }
        protected virtual Resource _create(string name, ResourceHandle handle, string group, bool isManual, IManualResourceLoader loader, Axiom.Collections.NameValuePairList createParams)
        {
            return null;
        }
        public virtual void RemoveAll()
        {
        }
    }

    public class TextureManager : ResourceManager
    {
    }

 
}
