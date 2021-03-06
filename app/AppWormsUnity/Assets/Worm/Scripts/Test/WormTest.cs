﻿using UnityEngine;
using System.Collections.Generic;

namespace Picodex
{
    public class WormTest : MonoBehaviour
    {
        WormActor actor;
        public Transform center;
        public float radius = 30;
        public float angle = 0;
        public float period = 6f;
        int dir = -1;
        // Use this for initialization
        void Start()
        {
            actor = GetComponent<WormActor>();

            actor.Ready(transform.position);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (true)
            {
                angle += period * Time.deltaTime;
                float x = Mathf.Cos(angle) * radius + center.transform.position.x; //x=cos(angle)*R+a;
                float y = Mathf.Sin(angle) * radius + center.transform.position.y; //y=sin(angle)*R+b;

                actor.Move(new Vector3(x, 0, y), Vector3.up);
                //   this.gameObject.transform.position = new Vector2(x, y);
            }

            if(false)
            {
                if (Mathf.Abs(actor.symPosition.x) > 100) dir = -dir;

                actor.Move(new Vector3(actor.symPosition.x +  30 * Time.deltaTime*dir, 0, actor.symPosition.z), Vector3.up);
            }
        }

  
    }
}
