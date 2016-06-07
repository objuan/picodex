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

        //   DFVolume volume;

        Vector3 targetPos;

     //   VolumePath volumePath;

        // Use this for initialization
        void Start()
        {
            actor = GetComponent<WormActor>();
            wormGame = GameObject.FindGameObjectWithTag("WormGame").GetComponent<WormGame>();
            volumeExtra = GameObject.FindGameObjectWithTag("Planet").GetComponent<DFVolumeExtra>();
            // volume = volumeCollider.GetComponent<DFVolumeFilter>().volume;
            navigator = new VolumeNavigator(volumeExtra.GetComponent<DFVolumeCollider>());

           // navigator.SetPos(actor.symPosition);
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
                    MoveTo(hit.point);
                }
            }
            // 
        }
        Vector3 force;

       void FixedUpdate()
        {
            Vector3 targetVector = (targetPos - actor.symPosition);

            // nav update
            navigator.Update(actor.symPosition);

            if (navigator.volumePath.pointList.Count > 0)
            {
                //  }
                //if (targetVector.magnitude > actor.ray)
                //{

                //  Vector3 planeP = MathUtility.ProjectPointOnPlane(actor.up, actor.symPosition, targetPos);
                //   Vector3 heading = (planeP - actor.symPosition);
                Vector3 planeP = navigator.volumePath.pointList[0].worldPosition;

                Vector3 heading = (planeP - actor.symPosition);

                float distance = heading.magnitude;

                float delta = Time.deltaTime * actor.speed * wormGame.symulationTime;

                Vector3 off = (targetPos - actor.symPosition).normalized * 0.1f;

                force = heading.normalized;//+ off;// + actor.up * 0.1f;

                Vector3 newPos = actor.symPosition + force * delta;

                 actor.Move(newPos + off * delta, (newPos - actor.symPosition).normalized);
            }
        }

        public void MoveTo(Vector3 targetPos)
        {
            this.targetPos = targetPos;

          //  navigator.CreatePathW(actor.symPosition, targetPos);

            navigator.MoveTo(targetPos);
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
#if UNITY_EDITOR
            DebugGame.DrawLine(this.targetPos, this.targetPos+ Vector3.up*5,Color.yellow);
            DebugGame.DrawLine(this.targetPos, this.targetPos + force * 5, Color.magenta);

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