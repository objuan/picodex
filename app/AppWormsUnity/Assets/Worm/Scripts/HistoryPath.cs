using UnityEngine;

namespace Picodex
{
    public struct HistoryPathPoint
    {
        public Vector3 position;
        public Vector3 forward;
        public Vector3 up;
        public float distance;
    }

    public class HistoryPath
    {
        public HistoryPathPoint[] path;
        private int turn = -1;
        private int size;

        public HistoryPathPoint headPoint
        {
            get
            {
                return path[turn];
            }
        }

        public HistoryPath(int size)
        {
            this.size = size;
            path = new HistoryPathPoint[size];
        }

        public void Add(Vector3 pos, Vector3 up)
        {
            Vector3 last = (turn >= 0) ? path[turn ].position : pos;

            turn++; if (turn == size) turn = 0;

            path[turn].position = pos;
            path[turn].forward = (pos - last);
            path[turn].distance = path[turn].forward.magnitude;
            path[turn].forward.Normalize();
            path[turn].up = Vector3.up;
        }

        public void GetInfo(float distance,ref HistoryPathPoint point)
        {
            float d = path[turn].distance; // PRIMO

            int i_turn = turn -1; if (i_turn < 0) i_turn = path.Length - 1; // ciclica

            while (d < distance && i_turn != turn)
            {
                d += path[i_turn].distance;

                i_turn--;if (i_turn < 0) i_turn = path.Length-1; // ciclica
            }
            if (i_turn != turn)
                point = path[i_turn];
            else
                point = path[turn];
        }
    }
}
