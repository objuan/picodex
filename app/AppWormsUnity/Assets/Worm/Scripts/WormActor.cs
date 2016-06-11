using UnityEngine;
using System.Collections.Generic;
 
namespace Picodex
{
    public class WormSegment
    {
        
        public Vector3 symPosition; // in world coordinate
        public GameObject obj;
        public float ray;

        public Vector3 _gfxPosition; // in world coordinate

        // runtime

        public Vector3 forward = Vector3.forward;// in world coordinate
        public Vector3 up = Vector3.up;// in world coordinate


        public void SetTrx()
        {
            _gfxPosition = symPosition + up * ray;
            obj.transform.position = _gfxPosition;
            obj.transform.LookAt(_gfxPosition + forward, up);

        }
    }
    public enum WormState
    {
        Dead,
        GetLive,
        Iddle,
        Moving
    }

    // [AddComponentMenu("Planet/Worm")]
    public class WormActor : MonoBehaviour
    {
        class MovePath
        {
            public Vector3[] path_pos;
            public Vector3[] path_fw;
            public Vector3[] path_up;
            public int turn = -1;
            int size;
            public Vector3 position
            {
                get { return path_pos[turn]; }
            }
            public Vector3 forward
            {
                get { return path_fw[turn]; }
            }
            public Vector3 up
            {
                get { return path_up[turn]; }
            }
            public MovePath(int size)
            {
                this.size = size;
                path_pos = new Vector3[size];
                path_fw = new Vector3[size];
                path_up = new Vector3[size];
            }
            public void Add(Vector3 p)
            {
                Vector3 last = (turn > 0) ? path_pos[turn - 1] : p;

                turn++; if (turn == size) turn = 0;
                path_pos[turn] = p;
                path_fw[turn] = (p - last).normalized;
                path_up[turn] = Vector3.up;
            }
        }

        new WormRenderer renderer;
        WormGame wormGame;

        public float speed = 1;

        public WormState state = WormState.Dead;

        // PARAMS
        // public float segLength = 10f;  // distance between objects
        // prefab obj
        // how many objects
        public int objects = 5;

        public Vector3 symPosition;
        public Vector3 symNormal;

        // path
        MovePath path;

        [System.NonSerialized]
        public List<WormSegment> segmentList = new List<WormSegment>();
        //  private MCBlob blob = null;

        public float ray
        {
            get { return segmentList[0].ray; }
        }
        //public Vector3 symPosition
        //{
        //    get { return segmentList[0].symPosition; }
        //}
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

            path = new MovePath(10);
            path.Add(symPosition); // first
        }

        // Update is called once per frame
        void Update()
        {
            if (segmentList.Count == 0) return;

            //Matrix4x4 _worldToWormTrx = worldToWormTrx;
            //Matrix4x4 _worldToWormInvTrx = worldToWormTrx.inverse;

            float stepDist = (symPosition - path.position).magnitude;
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
                path.Add(symPosition);

                segmentList[0].symPosition = path.position;
                segmentList[0].forward = path.forward;
                segmentList[0].up = symNormal;// path.up;

                segmentList[0].SetTrx();

                for (var i = 0; i < objects - 1; i++)
                {
                    setSegment(i + 1, segmentList[i].symPosition);
                }
            }
        }

        public void OnRenderObject()
        {
#if UNITY_EDITOR
            DebugGame.DrawLine(segmentList[0].symPosition, segmentList[0].symPosition + segmentList[0].up * 5, Color.green);
            DebugGame.DrawLine(segmentList[0].symPosition, segmentList[0].symPosition + segmentList[0].forward * 5, Color.blue);

            for(int i = 0; i < path.path_pos.Length; i++)
            {
                DebugGame.DrawLine(path.path_pos[i], path.path_pos[i]+ path.path_fw[i] * 0.2f, Color.red);
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
            WormSegment segment = new WormSegment();
            segmentList.Add(segment);
            segment.symPosition = new Vector3(0,0,0);
            segment.ray = ray;
            renderer.CreateMesh(segment);
            return segment;
        }

        public void Move(Vector3 pos,Vector3 normal) // , Vector3 forward
        {
            symPosition = pos;
            symNormal = normal;
            // new pos

            //segmentList[0].symPosition = pos;
            //segmentList[0].forward = forward;
        }

        void setSegment(int i, Vector3 prevPos)
        {
            //TODO
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, segmentList[i].forward);
            Quaternion rotInv = Quaternion.Inverse(rot);

            Vector3 prevPosLoc = rot * prevPos;
            Vector3 posLoc = rot * segmentList[i].symPosition;

            Vector3 d = prevPosLoc - posLoc;
            float angle1 = Mathf.Atan2(d.z, d.x); // plane XZ

            Vector3 newLocPos = new Vector3();
            newLocPos.x = prevPos.x - Mathf.Cos(angle1) * segmentList[i].ray*2; //TODO
            newLocPos.z = prevPos.z - Mathf.Sin(angle1) * segmentList[i].ray*2;
            newLocPos.y = posLoc.y;

            segmentList[i].symPosition = rotInv * newLocPos;
            segmentList[i].forward = (prevPos-segmentList[i].symPosition).normalized;
            // set object pos
            segmentList[i].SetTrx();

            //blob.blobs[i][0] = bodyList[i].position.x;
            //blob.blobs[i][1] = bodyList[i].position.y;
            //blob.blobs[i][2] = bodyList[i].position.z;

        }

    }
}