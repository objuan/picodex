using UnityEngine;
using System.Collections.Generic;
 
namespace Picodex
{
    public class WormSegment
    {
        public Vector3 position; // in world coordinate
        public GameObject obj;
        public float ray;

        // runtime

        public Vector3 forward = Vector3.forward;// in world coordinate
        public Vector3 up = Vector3.up;// in world coordinate

        public void SetTrx()
        {
            obj.transform.position = position;
            obj.transform.LookAt(position + forward, up);

        }
    }

    // [AddComponentMenu("Planet/Worm")]
    public class WormActor : MonoBehaviour
    {
        new WormRenderer renderer;
        WormGame wormGame;

        public float speed = 1;

        // PARAMS
       // public float segLength = 10f;  // distance between objects
                                        // prefab obj
      // how many objects
        public int objects = 5;

        [System.NonSerialized]
        public List<WormSegment> segmentList = new List<WormSegment>();
        //  private MCBlob blob = null;

        public float ray
        {
            get { return segmentList[0].ray; }
        }
        public Vector3 position
        {
            get { return segmentList[0].position; }
        }
        public Vector3 up
        {
            get { return segmentList[0].up; }
        }
        public Vector3 forward
        {
            get { return segmentList[0].forward; }
        }
        //TODO, parametrico
        //public Matrix4x4 worldToWormTrx
        //{
        //    get
        //    {
        //        return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, 0), Vector3.one);
        //    }
        //}

        // Use this for initialization
        void Start()
        {
            renderer = GetComponent<WormRenderer>();
            wormGame = GameObject.FindGameObjectWithTag("WormGame").GetComponent<WormGame>();
            //  blob = GetComponent<MCBlob>();

            for (var i = 0; i < objects; i++)
                AppendSegment(5 - ((float)i * 0.5f));

        }

        // Update is called once per frame
        void Update()
        {
            if (segmentList.Count==0) return;

            //Matrix4x4 _worldToWormTrx = worldToWormTrx;
            //Matrix4x4 _worldToWormInvTrx = worldToWormTrx.inverse;

            // update positions
            // setSegment(0, segmentList[0].position);
            segmentList[0].SetTrx();
         
            for (var i = 0; i < objects - 1; i++)
            {
                setSegment(i + 1, segmentList[i].position);
            }

#if UNITY_EDITOR
            Vector3 gravity = segmentList[0].up;
            Debug.DrawLine(segmentList[0].position, segmentList[0].position + gravity * 20, Color.yellow);
#endif
        }

        public WormSegment AppendSegment(float ray)
        {
            WormSegment segment = new WormSegment();
            segmentList.Add(segment);
            segment.position = new Vector3(0,0,0);
            segment.ray = ray;
            renderer.CreateMesh(segment);
            return segment;
        }

        public void Move(Vector3 pos, Vector3 forward )
        {
            // new pos

        
            segmentList[0].position = pos;
            segmentList[0].forward = forward;
        }

        void setSegment(int i, Vector3 prevPos)
        {
            //TODO
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, segmentList[i].forward);
            Quaternion rotInv = Quaternion.Inverse(rot);

            Vector3 prevPosLoc = rot * prevPos;
            Vector3 posLoc = rot * segmentList[i].position;

            Vector3 d = prevPosLoc - posLoc;
            float angle1 = Mathf.Atan2(d.z, d.x); // plane XZ

            Vector3 newLocPos = new Vector3();
            newLocPos.x = prevPos.x - Mathf.Cos(angle1) * segmentList[i].ray*2; //TODO
            newLocPos.z = prevPos.z - Mathf.Sin(angle1) * segmentList[i].ray*2;
            newLocPos.y = posLoc.y;

            segmentList[i].position = rotInv * newLocPos;
            segmentList[i].forward = (prevPos-segmentList[i].position).normalized;
            // set object pos
            segmentList[i].SetTrx();

            //blob.blobs[i][0] = bodyList[i].position.x;
            //blob.blobs[i][1] = bodyList[i].position.y;
            //blob.blobs[i][2] = bodyList[i].position.z;

        }

    }
}