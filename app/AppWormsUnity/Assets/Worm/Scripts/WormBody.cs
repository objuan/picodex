using System.Collections.Generic;
using UnityEngine;

namespace Picodex
{
    public class WormSegment
    {
        public GameObject obj;
        public float ray;

        public Vector3 _gfxPosition; // in world coordinate

        // runtime
        public Vector3 position; // in world coordinate
        public Vector3 forward = Vector3.forward;// in world coordinate
        public Vector3 up = Vector3.up;// in world coordinate

        public WormSegment()
        {

        }

        public void SetTrx()
        {
            _gfxPosition = position + up * ray;
            obj.transform.position = _gfxPosition;
            obj.transform.LookAt(_gfxPosition + forward, up);
        }

        public float DistanceFrom(WormSegment prevSegment)
        {
            return 0;
        } 
    }

    // ========================================================================
    public class WormBodyModder
    {
        WormBody body;
        public List<WormSegment> segmentList;
        protected float periodTime;
        protected bool periodChanged;
        float speedMult = 0.1f;
        public WormBodyModder(WormBody body)
        {
            this.body = body;
            segmentList = body.segmentList;
        }
        public virtual void Update(float time)
        {
            this.periodTime = time * speedMult;
            periodChanged = false;
            while (periodTime >= 1)
            {
                periodTime -= 1;
                periodChanged = true;
            }
        }
        public virtual float GetDistanceFromPrev(int segmentIdx)
        {
            return segmentList[segmentIdx].ray + segmentList[segmentIdx - 1].ray;
        }
    }
    // ========================================================================

    public class WormBody
    {
        [System.NonSerialized]
        public List<WormSegment> segmentList = new List<WormSegment>();

        public int Count
        {
            get { return segmentList.Count; }
        }
        public float ray
        {
            get { return segmentList[0].ray; }
        }

        // =================

        public WormBody(WormActor actor)
        {

        }

        public float GetDistanceFromPrev(int segmentIdx,float time)
        {
            return segmentList[segmentIdx].ray + segmentList[segmentIdx-1].ray ;
        }

        public WormSegment AppendSegment(float ray)
        {
            WormSegment segment = new WormSegment();
            segmentList.Add(segment);
            segment.position = new Vector3(0, 0, 0);
            segment.ray = ray;
          //  renderer.CreateMesh(segment);
            return segment;
        }

        public void UpdateSegment(int segmentIdx,Vector3 position, Vector3 forward, Vector3 up)
        {
            segmentList[segmentIdx].position = position;
            segmentList[segmentIdx].forward = forward;
            segmentList[segmentIdx].up = up;

            segmentList[segmentIdx].SetTrx();
        }


        public void OnRenderObject()
        {
#if UNITY_EDITOR
            DebugGame.DrawLine(segmentList[0].position, segmentList[0].position + segmentList[0].up * 5, Color.green);
            DebugGame.DrawLine(segmentList[0].position, segmentList[0].position + segmentList[0].forward * 5, Color.blue);
#endif
        }

    }
}
