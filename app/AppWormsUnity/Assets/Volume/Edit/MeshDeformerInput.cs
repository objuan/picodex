using Picodex.Vxcm;
using UnityEngine;

namespace Picodex
{
    [AddComponentMenu("Vxcm/MeshDeformerInput")]
    public class MeshDeformerInput : MonoBehaviour
    {
        public float force = 10f;
        public float forceOffset = 0.1f;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
            }
        }

      
        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            VolumeRaycastHit hit;

            if (Picodex.Volume.Raycast(inputRay, out hit))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point);
                //MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
                //if (deformer)
                //{
                //    Debug.DrawLine(Camera.main.transform.position, hit.point);

                //    Vector3 point = hit.point;
                //    //point += hit.normal * forceOffset;
                //    //deformer.AddDeformingForce(point, force);
                //}
            }
        }

    }
}