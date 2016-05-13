using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Renderer : Component
    {
        private bool _isVisible=true;

       //  Makes the rendered 3D object visible if enabled. 
        public bool enabled;

        // The bounding volume of the renderer (Read Only). 
        Bounds bounds
        {
            get {
                if (GetComponent<MeshFilter>() != null)
                    return GetComponent<MeshFilter>().mesh.bounds;
                return null;
            }
        }

        bool isPartOfStaticBatch;// Has this renderer been statically batched with any other renderers? 

        bool isVisible{get{return _isVisible;}} //Is this renderer visible in any camera? (Read Only) 

        //lightmapIndex The index of the baked lightmap applied to this renderer. 
//lightmapScaleOffset The UV scale & offset used for a lightmap. 
        // Matrix that transforms a point from local space into world space (Read Only). 
        public Matrix4x4 localToWorldMatrix{
            get{return gameObject.transform.localToWorldMatrix;}
        }
        public Matrix4x4 worldToLocalMatrix{
            get { return gameObject.transform.worldToLocalMatrix; }
        }
           

        public Material material;// Returns the first instantiated Material assigned to the renderer. 

        public Material[] materials;// Returns all the instantiated materials of this object. 

      //  probeAnchor If set, Renderer will use this Transform's position to find the light or reflection probe. 
     //   realtimeLightmapIndex The index of the realtime lightmap applied to this renderer. 
     //   realtimeLightmapScaleOffset The UV scale & offset used for a realtime lightmap. 
        public bool receiveShadows;// Does this object receive shadows? 
      //  reflectionProbeUsage Should reflection probes be used for this Renderer? 
        public bool shadowCastingMode;// Does this object cast shadows? 
        //sharedMaterial The shared material of this object. 
        //sharedMaterials All the shared materials of this object. 

        //sortingLayerID Unique ID of the Renderer's sorting layer. 
        //sortingLayerName Name of the Renderer's sorting layer. 
        //sortingOrder Renderer's order within a sorting layer. 
      //  useLightProbes Should light probes be used for this Renderer? 

        public Renderer()
        {
            UnityEngine.Platform.UnityContext.Singleton.CurrentScene.AddRenderer(this);

        }
     


    }
}
