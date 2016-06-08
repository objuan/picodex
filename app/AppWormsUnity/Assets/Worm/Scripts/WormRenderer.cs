using UnityEngine;
using System.Collections;
 
namespace Picodex
{
   // [AddComponentMenu("Planet/Worm")]
    public class WormRenderer : MonoBehaviour
    {
        WormActor actor;

        float scale = 0.5f;

        // Use this for initialization
        void Start()
        {

            actor = GetComponent<WormActor>();
       
        }

        // Update is called once per frame
        void Update()
        {

         
        }

        public void CreateMesh(WormSegment segment)
        {
            // segment.obj = PrimitiveHelper.CreatePrimitive(PrimitiveType.Sphere, segment.ray);
            segment.obj = PrimitiveHelper.CreatePrimitive(PrimitiveType.Cone, segment.ray*2 * scale);
            segment.obj.transform.parent = this.transform;

            Matrix4x4 trx = Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(90,0,0),Vector3.one);
            PrimitiveHelper.TrasformMesh(segment.obj.GetComponent<MeshFilter>().sharedMesh, trx);

           // segment.obj.transform.rotation.SetEulerAngles(45, 0, 0);

            segment.obj.GetComponent<Renderer>().material = new Material(Shader.Find("Custom/SampleDiffuse"));

//#if UNITY_EDITOR
//            segment.obj.AddComponent<WorldAxis>().size = segment.ray * 2;
//#endif
        }


    }
}