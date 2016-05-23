using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Picodex.VolumeData
{
    public class WorldChunk<TYPE> where TYPE:struct
    {
        TYPE [] data;
        Vector3i pos;
        int size;

        public WorldChunk(int size,Vector3i position)
        {
            this.size = size;
            data = new TYPE[size * size * size];
            pos = position;
        }

        public TYPE this[int x,int y,int z]
        {
            get
            {
                return data[x + y * size + z * size * size];
            }

            set
            {
                data[x + y * size + z * size * size] = value;
            }
        }

        public TYPE this[Vector3i v]
        {
            get
            {
                return this[v.x, v.y, v.z];
            }

            set
            {
                this[v.x, v.y, v.z] = value;
            }
        }

        public Vector3i GetPosition()
        {
            return pos;
        }

        public int Size()
        {
            return size;
        }
    }
}
