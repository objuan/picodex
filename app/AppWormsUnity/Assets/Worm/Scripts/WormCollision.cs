using UnityEngine;
using System.Collections;
namespace Picodex
{
    //[AddComponentMenu("Planet/WormCollision")]
    public class WormCollision : MonoBehaviour {

        DFVolumeCollider planetCollider;
        WormRenderer wormRenderer;
        WormActor actor;

        // Use this for initialization
        void Start() {

            planetCollider = GameObject.FindGameObjectWithTag("Planet").GetComponent< DFVolumeCollider>();

            actor = GetComponent<WormActor>();
            wormRenderer = GetComponent<WormRenderer>();

        }

        // Update is called once per frame
        void Update() {

          //  Vector3 gravity = actor.worldToWormTrx.MultiplyPoint(new Vector3(0, -1, 0));
            // Vector3 gravity = new Vector3(0, -1, 0);

            VolumeRaycastRequest request = new VolumeRaycastRequest();

            //request.AddRaycast();

            float offsetMult = 4;

            foreach (WormSegment segment in actor.segmentList)
            {
                request.AddRaycast(segment.symPosition + segment.up * (segment.ray* offsetMult), -segment.up);
            }

            // pick il volume interessato
            if (Picodex.Volumetric.Raycast(planetCollider, request))
            {
                for(int i=0;i< actor.segmentList.Count;i++)
                {

                    if (!request.entryList[i].hasCollision)
                    {
                        VolumeRaycastRequest req;
                        Picodex.Volumetric.RaycastSpherical(planetCollider, request.entryList[i].origin, out req);
                        if (req != null)
                        {
                            VolumeRaycastHit hit = req.GetMinDistanceHit();
                            request.entryList[i].hit = hit; // assign thew nearest

                            Debug.Log("SCAN HIT !!! " + i);
                        }
                    }

                    if (request.entryList[i].hasCollision)
                    { 
                        //  Debug.Log("HIT " + i+") "+request.entryList[i].origin +" -> "+ request.entryList[i].hit.distance);

                        float dist = request.entryList[i].hit.distance - actor.segmentList[i].ray * (offsetMult);

                       // Quaternion rot = Quaternion.FromToRotation(actor.segmentList[i].up, request.entryList[i].hit.normal);

                        actor.segmentList[i].up = request.entryList[i].hit.normal;

                        //if (i == 0) // solo per la testa
                        //    actor.segmentList[i].forward = rot * actor.segmentList[i].forward;

                         if (Mathf.Abs(dist)> 0.1f)
                             actor.segmentList[i].symPosition =   actor.segmentList[i].symPosition + actor.segmentList[i].up * -dist;
                    }
                    else
                    {
                        Debug.Log("NO COLL !!! " + i);
                    }
                }
                //Debug.Log("Move " + hit.point);
            }

        }

        //bool TotalScan(WormSegment segment,ref VolumeRaycastHit out_hit)
        //{
        //    VolumeRaycastRequest request = new VolumeRaycastRequest();

        //    float offsetMult = 3;

        //    // sparo in tutte le direzioni

        //    request.AddRaycast(segment.position + segment.up * (segment.ray * offsetMult), -segment.up);

        //    // pick il volume interessato
        //    if (Picodex.Volumetric.Raycast(planetCollider, request))
        //    {
        //        out_hit = request.GetMinDistanceHit();

        //        Debug.Log("FIND MIN " + out_hit.point);

        //        return true;
        //    }
        //    else
        //        return false;
        //}
    }
}
