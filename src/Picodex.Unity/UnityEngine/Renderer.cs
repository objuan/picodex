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

        /* Returns the first instantiated Material assigned to the renderer. 
         * Modifying material will change the material for this object only.
        If the material is used by any other renderers, this will clone the shared material and start using it from now on.
        Note:
        This function automatically instantiates the materials and makes them unique to this renderer. It is your responsibility to destroy the materials when the game object is being destroyed. Resources.UnloadUnusedAssets also destroys the materials but it is usually only called when loading a new level.
         * */
        public Material material
        {
            get
            {
                if (GetComponent<MeshFilter>() != null) return GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.material;
                return null;
            }
            set
            {
                if (GetComponent<MeshFilter>() != null) GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.material = value;
            }
        }

        /*
         * // Returns all the instantiated materials of this object. 
         * This is an array of all materials used by the renderer. 
         * Unity supports a single object using multiple materials; in this case materials contains all the materials. sharedMaterial and material properties return the first used material if there is more than one.
            Modifying any material in materials will change the appearance of only that object.
            Note that like all arrays returned by Unity, this returns a copy of materials array. If you want to change some materials in it, get the value, change an entry and set materials back.
            Note:
            This function automatically instantiates the materials and makes them unique to this renderer. It is your responsibility to destroy the materials when the game object is being destroyed. 
         * Resources.UnloadUnusedAssets also destroys the materials but it is usually only called when loading a new level.

         * */
        public Material[] materials
        {
            get
            {
                if (GetComponent<MeshFilter>() != null) return GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.materials;
                return null;
            }
        }

      //  probeAnchor If set, Renderer will use this Transform's position to find the light or reflection probe. 
     //   realtimeLightmapIndex The index of the realtime lightmap applied to this renderer. 
     //   realtimeLightmapScaleOffset The UV scale & offset used for a realtime lightmap. 
        public bool receiveShadows;// Does this object receive shadows? 
      //  reflectionProbeUsage Should reflection probes be used for this Renderer? 
        public bool shadowCastingMode;// Does this object cast shadows? 
      
        //sharedMaterial The shared material of this object. 
        //sharedMaterials All the shared materials of this object. 
        public Material sharedMaterial
        {
            get
            {
                if (GetComponent<MeshFilter>() != null) return GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.sharedMaterial;
                return null;
            }
            set
            {
                if (GetComponent<MeshFilter>() != null) GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.sharedMaterial = value;
            }
        }

        public Material[] sharedMaterials// Returns all the instantiated materials of this object. 
        {
            get
            {
                if (GetComponent<MeshFilter>() != null) return GetComponent<MeshFilter>().mesh._renderableObject.MaterialView.sharedMaterials;
                return null;
            }
        }

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
