using System.Collections.Generic;
using UnityEngine;

namespace Picodex
{
    public class EarthWormBodyModder : WormBodyModder
    {
        public AnimationCurve speedCurve;
        public AnimationCurve heightCurve;

        // public int keyDistance = 4;
        bool turn = false;

        public List<float> distanceList;

        public EarthWormBodyModder(WormBody body):base(body)
        {
        }

        public override void Update(float time)
        {
            base.Update(time);

            //rise
            while (distanceList.Count < segmentList.Count) distanceList.Add(0);

            // calcolo le posizioni 
            int middleSegment = segmentList.Count / 2;
            if (periodChanged)
            {
                turn = !turn;
            }
            if (!turn)
            {
                // muovo la testa fino a middleSegment
                float s = speedCurve.Evaluate(periodTime);
                float h = heightCurve.Evaluate(periodTime);


            }
        }

        public override float GetDistanceFromPrev(int segmentIdx)
        {
            return distanceList[segmentIdx];
            //return segmentList[segmentIdx].ray + segmentList[segmentIdx - 1].ray;
        }
    }

    public class EarthWormActor : WormActor
    {
        public AnimationCurve speedCurve;
        public AnimationCurve heightCurve;

        // Use this for initialization
        protected override void OnStart()
        {
            bodyModder = new EarthWormBodyModder(body);
            ((EarthWormBodyModder)bodyModder).speedCurve = speedCurve;
            ((EarthWormBodyModder)bodyModder).heightCurve = heightCurve;
        }

        // Update is called once per frame
        protected override void OnUpdate()
        {

        }
    }
}
