using UnityEngine;
using System.Collections;
 
namespace Picodex
{
   // [AddComponentMenu("Planet/Worm")]
    public class WormRenderer : MonoBehaviour
    {

        struct SnakePart
        {
            public Vector3 position;
            public GameObject obj;
        }

        // PARAMS
        public float segLength = 4.5f;  // distance between objects
                                        // prefab obj
        public GameObject segmentObj;


        // how many objects
        public int objects = 20;
        private SnakePart[] bodyList;
        //  private MCBlob blob = null;

        // Use this for initialization
        void Start()
        {

            //  blob = GetComponent<MCBlob>();

            // init array sizes
            bodyList = new SnakePart[objects];

            // instantiate objects
            for (var i = 0; i < objects; i++)
            {
                bodyList[i].position = new Vector3();
                //     bodyList[i].obj = new GameObject[objects];
                bodyList[i].obj = (GameObject)Instantiate(segmentObj, new Vector3(i, i, 0), Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                //RaycastHit hit;
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //if (Volume.Raycast(ray, out hit))
                //// if (Physics.Raycast(ray, out hit))
                //{
                //    // this.transform.position = hit.point;

                //    MoveTo( hit.point);
                //}
            }
        }

        void MoveTo(Vector3 posW)
        {
            setSegment(0, posW);

            for (var i = 0; i < objects - 1; i++)
            {
                setSegment(i + 1, bodyList[i].position);
            }
        }

        void setSegment(int i, Vector3 prevPos)
        {
            Vector3 d = prevPos - bodyList[i].position;
            float angle1 = Mathf.Atan2(d.z, d.x);

            bodyList[i].position.x = prevPos.x - Mathf.Cos(angle1) * segLength;
            bodyList[i].position.z = prevPos.z - Mathf.Sin(angle1) * segLength;

            // simple gravity effect
            // if (gravity == 1) y[i] += 0.1;

            // set object pos
            bodyList[i].obj.transform.position = bodyList[i].position;


            //blob.blobs[i][0] = bodyList[i].position.x;
            //blob.blobs[i][1] = bodyList[i].position.y;
            //blob.blobs[i][2] = bodyList[i].position.z;

        }

    }
}