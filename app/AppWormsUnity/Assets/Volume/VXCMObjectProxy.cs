using UnityEngine;
using System.Collections.Generic;

namespace Picodex
{
    [ExecuteInEditMode]
    public class VXCMObjectProxy : MonoBehaviour
    {
        bool lastIsBack = true;

        Mesh originalMesh;

        MeshFilter meshFilter;

      //  DFVolumeRenderer renderer_df;

        public static GameObject CreateGameObject(string name)
        {
            // default
            GameObject proxyGameObject = new GameObject(name);
            MeshFilter meshFilter = proxyGameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh.name = name;
            PrimitiveHelper.CreateCube(meshFilter.sharedMesh);

            proxyGameObject.AddComponent<MeshRenderer>();
            proxyGameObject.AddComponent<VXCMObjectProxy>();

            return proxyGameObject;
        }

        public void Start()
        {

            meshFilter = GetComponent<MeshFilter>();
            
            originalMesh = meshFilter.sharedMesh;

            Rebuild();

        }

        public void Rebuild()
        {
            if (meshFilter == null) return;

            DFVolumeRenderer renderer_df = transform.parent.gameObject.GetComponent<DFVolumeRenderer>();

            DFVolume volume = GetComponent<VXCMObject>().volume as DFVolume;

            if (renderer_df.proxyType == DFVolumeRendererProxyType.Box)
                PrimitiveHelper.CreateCube(originalMesh, volume.resolution.x, volume.resolution.y, volume.resolution.z);
            else if (renderer_df.proxyType == DFVolumeRendererProxyType.Sphere)
                PrimitiveHelper.CreateSphere(originalMesh, volume.resolution.x * 0.5f);
        }

        public void OnWillRenderObject()
        {
            if (meshFilter == null) return;

            //  Debug.Log("OnWillRenderObject");
            if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
                return;

            Camera camera = Camera.current;
            if (!camera) return;
        //}

        //public void UpdateFromCamera(Transform objTrx, Camera camera)
        //{
            
            // if (camera != Camera.main) return; // solo main camera

            Bounds bounds = originalMesh.bounds;

            bool isBack = false;
            // bound to EYE space
            Vector3[] extentPoints = bounds.GetExtends();
            for (int i = 0; i < 8; i++)
            {
                extentPoints[i] = camera.WorldToViewportPoint(transform.TransformPoint(extentPoints[i]));
                if (extentPoints[i].z < 0) // is back
                {
                    isBack = true;
                    break;
                }
            }

            if (isBack && camera == Camera.main) // solo per la camera principale
            {

               // Debug.Log("HIT");

                // Matrix4x4 worldToLocal = objTrx.worldToLocalMatrix;

                //Vector3[] planeVertices = new Vector3[] {
                //    camera.ViewportToWorldPoint(worldToLocal.MultiplyPoint(new Vector3(0, 0, 10))),
                //    camera.ViewportToWorldPoint(worldToLocal.MultiplyPoint(new Vector3(0, 1,  10))),
                //    camera.ViewportToWorldPoint(worldToLocal.MultiplyPoint(new Vector3(1, 1,  10))),
                //    camera.ViewportToWorldPoint(worldToLocal.MultiplyPoint(new Vector3(1, 0,  10)))
                //};

                // CUT PLANE

                //   Matrix4x4 cutTrx = Matrix4x4.TRS(camera.transform.position + camera.transform.forward * camera.nearClipPlane, camera.transform.rotation * Quaternion.Euler(90, 0, 0), Vector3.one);

                Vector3 splitPos = camera.transform.position + camera.transform.forward * (camera.nearClipPlane +2); // TODO ??
                Quaternion splitRot = camera.transform.rotation * Quaternion.Euler(90, 0, 0);

                Split(splitPos, splitRot, transform);

#if UNITY_EDITOR
                //  Split(cutTrx, objTrx);
                if (GameObject.Find("CutPlane") != null)
                {
                    Transform cutTrx = GameObject.Find("CutPlane").transform;

                    cutTrx.position = splitPos;
                    cutTrx.rotation = splitRot;
                }
                // TODO, ottimizzare ??
#endif 
            }

            else if (lastIsBack != isBack)
            {
                //CombineInstance[] combine = new CombineInstance[1];
                //combine[0].mesh = originalMesh;
                //combine[0].transform = Matrix4x4.identity;
                //mesh.CombineMeshes(combine);
                //mesh.RecalculateBounds();
                meshFilter.sharedMesh = originalMesh;
            }
            lastIsBack = isBack;

        }

        // ==============================================================

        public void Split(Vector3 splitPos,Quaternion splitRot, Transform objTrx)
        {
            Plane _splitPlane = new Plane(splitPos, splitRot * Vector3.up);
           //   Plane _splitPlane = new Plane(splitTransform);

           MeshContainer meshContainer = new MeshContainer(originalMesh, objTrx);
            //IMeshSplitter meshSplitter = (IMeshSplitter)new MeshSplitterConvex(meshContainer, _splitPlane, splitTransform.rotation);
            IMeshSplitter meshSplitter = (IMeshSplitter)new MeshSplitterConvex(meshContainer, _splitPlane, splitRot);

            //   if (UseCapUV) meshContainer.SetCapUV(UseCapUV, CustomUV, CapUVMin, CapUVMax);

#if UNITY_EDITOR
            meshSplitter.DebugDraw(false);
#endif

            // ---------------
            bool anySplit = false;

            meshContainer.MeshInitialize();
            meshContainer.CalculateWorldSpace();

            // split mesh
            meshSplitter.MeshSplit();

            if (meshContainer.IsMeshSplit())
            {
                anySplit = true;
                meshSplitter.MeshCreateCaps();               
            }
            if (anySplit)
            {
                //if (meshContainer.HasMeshUpper() & meshContainer.HasMeshLower())
                //{
                //    meshFilter.mesh = meshContainer.CreateMeshLower();
                //}
                if (meshContainer.HasMeshUpper() & meshContainer.HasMeshLower())
                {
                    meshFilter.sharedMesh = meshContainer.CreateMeshUpper();
                    meshFilter.sharedMesh.name = "VXCMObjCutted";
                }
            }
       }
   
    }
}
