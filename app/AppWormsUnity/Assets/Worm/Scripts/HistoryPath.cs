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

        //public Vector3 position
        //{
        //    get { return path[turn]; }
        //}
        //public Vector3 forward
        //{
        //    get { return path_fw[turn]; }
        //}
        //public Vector3 up
        //{
        //    get { return path_up[turn]; }
        //}

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
            Vector3 last = (turn > 0) ? path[turn - 1].position : pos;

            turn++; if (turn == size) turn = 0;

            path[turn].position = pos;
            path[turn].forward = (pos - last);
            path[turn].distance = path[turn].forward.magnitude;
            path[turn].forward.Normalize();
            path[turn].up = Vector3.up;
        }

        public void GetInfo(float headDistance,ref HistoryPathPoint point)
        {

        }
    }
}
