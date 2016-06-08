using UnityEngine;
using System.Collections;
 
namespace Picodex
{
  
    // [AddComponentMenu("Planet/Worm")]
    public class WormIA : MonoBehaviour
    {
        WormActor actor;
        WormGame wormGame;
        DFVolumeExtra volumeExtra;
        VolumeNavigator navigator;
        DFVolumeCollider planetCollider;

        //   DFVolume volume;

      //  Vector3 targetPos;

        Vector3 borningPos;

        //   VolumePath volumePath;

        // Use this for initialization
        void Start()
        {
            actor = GetComponent<WormActor>();
            wormGame = GameObject.FindGameObjectWithTag("WormGame").GetComponent<WormGame>();
            volumeExtra = GameObject.FindGameObjectWithTag("Planet").GetComponent<DFVolumeExtra>();
            // volume = volumeCollider.GetComponent<DFVolumeFilter>().volume;
            planetCollider = volumeExtra.GetComponent<DFVolumeCollider>();
            navigator = new VolumeNavigator(planetCollider);

            // navigator.SetPos(actor.symPosition);

            // DEBUG

            Born(new Vector3(0, 0, 0));
        }

        // Update is called once per frame
        void Update()
        {
            // DEBUG
            if (Input.GetMouseButton(0))
            {
                VolumeRaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Volumetric.Raycast(ray, out hit))
                {
                    // Debug.Log("Move "+ hit.point);

                    //  Vector3 delta = hit.point - actor.position;
                    //  Plane plane = new Plane(actor.position, actor.up);
                    // delta.y = 0;
                    MoveTo(hit.point + hit.normal * 0.5f);
                }
            }
            // 
        }
        Vector3 force;

       void FixedUpdate()
        {
            // return;
            // STATE MACHINE
            if (actor.state == WormState.GetLive)
            {
                Vector3 a = borningPos;
                if (CheckPos(ref a))
                {
                    actor.Ready(a);
                    navigator.MoveTo(a);
                    navigator.Update(a);
                    // targetPos = a; //reset
                }
                return;
            }
            else if (actor.state == WormState.Dead)
            {
                return;
            }
            //else if(actor.state == WormState.Iddle)
            //{
            //    navigator.Update(actor.symPosition);

            //    if (navigator.Count > 0)
            //        actor.Go();
            //    return;
            //}

          //  Vector3 targetVector = (targetPos - actor.symPosition);

            // nav update
            navigator.Update(actor.symPosition);

            if (navigator.Count > 0)
            {
                //  }
                //if (targetVector.magnitude > actor.ray)
                //{

                //  Vector3 planeP = MathUtility.ProjectPointOnPlane(actor.up, actor.symPosition, targetPos);
                //   Vector3 heading = (planeP - actor.symPosition);
                VolumePathPoint nextPoint = navigator.nextPoint;

                Vector3 heading = (nextPoint.worldPosition - actor.symPosition);
                //Vector3 targetVector = (targetPos - actor.symPosition);

                float distance = heading.magnitude;

                float delta = Time.deltaTime * actor.speed * wormGame.symulationTime;

              //  Vector3 off = (targetPos - actor.symPosition).normalized * 0.1f;

                force = heading.normalized;//+ off;// + actor.up * 0.1f;

                Vector3 newPos = actor.symPosition + force * delta;

                // collision

                CheckPos(ref newPos);

                //

                actor.Move(newPos);// + off * delta);
            }
            //else
            //    actor.Stop();
        }

        public void MoveTo(Vector3 targetPos)
        {
       //     this.targetPos = targetPos;

           // CheckPos(ref this.targetPos);
            //  navigator.CreatePathW(actor.symPosition, targetPos);

            navigator.MoveTo(targetPos);

        }

        public void Born(Vector3 pos)
        {
            borningPos = pos;
            actor.Born();
        }

        private bool CheckPos(ref Vector3 pos)
        {
            VolumeRaycastHit hit = new VolumeRaycastHit();
            float dist;

            VolumeRaycastRequest req;
            int hitCount = Picodex.Volumetric.RaycastSpherical(planetCollider, pos, out req);
            if (hitCount == 0)
            {
                // no collision
                req = new VolumeRaycastRequest();
                float offsetMult = 2;
                req.AddRaycast(pos + actor.up * (actor.ray * offsetMult), -actor.up);
                if (Picodex.Volumetric.Raycast(planetCollider, req))
                {
                    hit = req.GetMinDistanceHit();
                }
                else
                    return false;
                dist = actor.ray * (offsetMult) - hit.distance;
                Debug.Log("search");
            }
            else
            {
                hit = req.GetMinDistanceHit();
                dist = actor.ray - hit.distance;
            }

            pos = hit.point + (hit.normal * 0.5f);

        //    Debug.Log(pos + " N:" + hit.normal + " d:" + dist);
            //pos += hit.normal * dist;
            return true;
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
#if UNITY_EDITOR
            DebugGame.DrawLine(navigator.targetPos.worldPosition, navigator.targetPos.worldPosition+ Vector3.up*5,Color.yellow);
            DebugGame.DrawLine(navigator.targetPos.worldPosition, navigator.targetPos.worldPosition + force * 5, Color.magenta);

            VolumePath volumePath  = navigator.volumePath;
            // DEBUG PATH
            if (volumePath != null)
            {
                foreach (VolumePathPoint point in volumePath.pointList)
                {
                    Vector3 p = point.worldPosition;

                    DebugGame.DrawLine(p, p + Vector3.up, Color.blue);
                }
            }
#endif
        }
    }
}