using UnityEngine;
using System.Collections;
 
namespace Picodex
{
    // [AddComponentMenu("Planet/Worm")]
    public class WormIA : MonoBehaviour
    {
        WormActor actor;
        WormGame wormGame;

        Vector3 targetPos;
       
        // Use this for initialization
        void Start()
        {
            actor = GetComponent<WormActor>();
            wormGame = GameObject.FindGameObjectWithTag("WormGame").GetComponent<WormGame>();
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
            Vector3 targetVector = (targetPos - actor.position);

            if (targetVector.magnitude > actor.ray)
            {
                Vector3 planeP = MathUtility.ProjectPointOnPlane(actor.up, actor.position, targetPos);
                Vector3 heading = (planeP - actor.position);

                float distance = heading.magnitude;

                float delta = Time.deltaTime * actor.speed * wormGame.symulationTime;

                Vector3 off = (targetPos - actor.position).normalized * 0.1f;

                force = heading.normalized;//+ off;// + actor.up * 0.1f;

                Vector3 newPos = actor.position + force * delta;

                actor.Move(newPos + off * delta, (newPos - actor.position).normalized);
            }
        }

        public void MoveTo(Vector3 targetPos)
        {
            this.targetPos = targetPos;
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
#if UNITY_EDITOR
            DebugGame.DrawLine(this.targetPos, this.targetPos+ Vector3.up*5,Color.yellow);

            DebugGame.DrawLine(this.targetPos, this.targetPos + force * 5, Color.magenta);
#endif
        }
    }
}