using UnityEngine;
using System.Collections.Generic;
 
namespace Picodex
{
    public enum WormState
    {
        Dead,
        GetLive,
        Iddle,
        Moving
    }

    // [AddComponentMenu("Planet/Worm")]
    public abstract class WormActor : MonoBehaviour
    {
        new WormRenderer renderer;
        WormGame wormGame;

        public float speed = 10;

        public WormState state = WormState.Dead;

        // PARAMS
        // public float segLength = 10f;  // distance between objects
        // prefab obj
        // how many objects
        public int objects = 5;

       // [System.NonSerialized]
        public Vector3 symPosition;
        //[System.NonSerialized]
        public Vector3 symNormal;

        // path
        HistoryPath path;
        [System.NonSerialized]
        public WormBodyModder bodyModder;

        public WormBody body;
        //[System.NonSerialized]
        //public List<WormSegment> segmentList = new List<WormSegment>();
        //  private MCBlob blob = null;

        public float ray
        {
            get { return body.ray; }
        }

        //public Vector3 symPosition
        //{
        //    get { return segmentList[0].symPosition; }
        //}
        public Vector3 up
        {
            get { return path.headPoint.up; }
        }
        public Vector3 forward
        {
            get { return path.headPoint.forward; }
        }

        protected abstract void OnStart();

        protected abstract void OnUpdate();


        // Use this for initialization
        void Start()
        {
            renderer = GetComponent<WormRenderer>();
            wormGame = GameObject.FindGameObjectWithTag("WormGame").GetComponent<WormGame>();
            //  blob = GetComponent<MCBlob>();

            body = new WormBody(this);

            for (var i = 0; i < objects; i++)
                AppendSegment(5 );
            //AppendSegment(5 - ((float)i * 0.5f));

            path = new HistoryPath(200);
            path.Add(symPosition, symNormal); // first

            bodyModder = new WormBodyModder(body);

            OnStart();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (body.Count == 0) return;

            //Matrix4x4 _worldToWormTrx = worldToWormTrx;
            //Matrix4x4 _worldToWormInvTrx = worldToWormTrx.inverse;

            float stepDist = (symPosition - path.headPoint.position).magnitude;
         //   float stepDist = path.headPoint.distance;
            // update positions
            // setSegment(0, segmentList[0].position);
            if (state == WormState.Iddle && stepDist > 0.1)
            {
                state = WormState.Moving;
            }
            if (state == WormState.Moving && stepDist < 0.1)
            {
                state = WormState.Iddle;
            }

            if (state == WormState.Moving)
            {
                path.Add(symPosition,symNormal);
              //  Debug.Log("--");
                float dist = 0;

                bodyModder.Update(Time.realtimeSinceStartup);

                HistoryPathPoint point = new HistoryPathPoint();
                for (var i = 0; i < objects; i++)
                {
                    path.GetInfo(dist,ref point);

                //    Debug.Log(dist);

                    body.UpdateSegment(i, point.position, point.forward, point.up);

                    if (i < objects - 1)
                        dist += bodyModder.GetDistanceFromPrev(i + 1);

                    //    dist += body.GetDistanceFromPrev(i + 1, bodyModder, Time.realtimeSinceStartup);
                }
                //segmentList[0].symPosition = path.position;
                //segmentList[0].forward = path.forward;
                //segmentList[0].up = symNormal;// path.up;

                    //segmentList[0].SetTrx();

                    //for (var i = 0; i < objects - 1; i++)
                    //{
                    //    setSegment(i + 1, segmentList[i].symPosition);
                    //}
            }
        }

        public void OnRenderObject()
        {
#if UNITY_EDITOR
            body.OnRenderObject();

            for(int i = 0; i < path.path.Length; i++)
            {
                DebugGame.DrawLine(path.path[i].position, path.path[i].position+ path.path[i].up , Color.red);
            }
#endif
        }

        public void Born()
        {
            state = WormState.GetLive;
        }

        public void Ready(Vector3 pos)
        {
            symPosition = pos;
            state = WormState.Iddle;
        }

        //public void Go()
        //{
        //    state = WormState.Moving;
        //}

        //public void Stop()
        //{
        //    state = WormState.Iddle;
        //}

        public WormSegment AppendSegment(float ray)
        {
            WormSegment segment = body.AppendSegment(ray);
            //WormSegment segment = new WormSegment();
            //segmentList.Add(segment);
            //segment.symPosition = new Vector3(0,0,0);
            //segment.ray = ray;
            renderer.CreateMesh(segment);
            return segment;
        }

        public void Move(Vector3 pos,Vector3 normal) // , Vector3 forward
        {
            symPosition = pos;
            symNormal = normal;
        }

        //void setSegment(int i, Vector3 prevPos)
        //{
        //    //TODO
        //    Quaternion rot = Quaternion.FromToRotation(Vector3.forward, segmentList[i].forward);
        //    Quaternion rotInv = Quaternion.Inverse(rot);

        //    Vector3 prevPosLoc = rot * prevPos;
        //    Vector3 posLoc = rot * segmentList[i].symPosition;

        //    Vector3 d = prevPosLoc - posLoc;
        //    float angle1 = Mathf.Atan2(d.z, d.x); // plane XZ

        //    Vector3 newLocPos = new Vector3();
        //    newLocPos.x = prevPos.x - Mathf.Cos(angle1) * segmentList[i].ray*2; //TODO
        //    newLocPos.z = prevPos.z - Mathf.Sin(angle1) * segmentList[i].ray*2;
        //    newLocPos.y = posLoc.y;

        //    segmentList[i].symPosition = rotInv * newLocPos;
        //    segmentList[i].forward = (prevPos-segmentList[i].symPosition).normalized;
        //    // set object pos
        //    segmentList[i].SetTrx();

        //    //blob.blobs[i][0] = bodyList[i].position.x;
        //    //blob.blobs[i][1] = bodyList[i].position.y;
        //    //blob.blobs[i][2] = bodyList[i].position.z;

        //}

    }
}