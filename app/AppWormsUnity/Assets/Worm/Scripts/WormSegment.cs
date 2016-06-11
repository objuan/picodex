using UnityEngine;


namespace Picodex
{
    public class WormSegment
    {
        public GameObject obj;
        public float ray;

     
        public Vector3 _gfxPosition; // in world coordinate

        // runtime
        public Vector3 symPosition; // in world coordinate
        public Vector3 forward = Vector3.forward;// in world coordinate
        public Vector3 up = Vector3.up;// in world coordinate

        public void SetTrx()
        {
            _gfxPosition = symPosition + up * ray;
            obj.transform.position = _gfxPosition;
            obj.transform.LookAt(_gfxPosition + forward, up);
        }
    }
}
