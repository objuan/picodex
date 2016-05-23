﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Picodex.VolumeData
{
    public class HashedVolume<TYPE> : IVolumeData<TYPE> where TYPE:struct
    {
        int chunksize = 64;
        public Dictionary<Vector3i, WorldChunk<TYPE>> data = new Dictionary<Vector3i, WorldChunk<TYPE>>();

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


        public TYPE this[int x,int y,int z]
        {
            get
            {
                if (x < 0 || y < 0 || z < 0)
                    return default(TYPE);

                Vector3i v = new Vector3i(x / chunksize, y / chunksize, z / chunksize) * chunksize;

                if (data.ContainsKey(v))
                {
                    WorldChunk<TYPE> a = data[v];
                    return a[x % chunksize, y % chunksize, z % chunksize];
                }
                else
                { 
                    return default(TYPE);
                }
            }

            set 
            {
                Vector3i v = new Vector3i(x / chunksize, y / chunksize, z / chunksize) * chunksize;
                WorldChunk<TYPE> a;

                if (!data.ContainsKey(v))
                {
                    a = new WorldChunk<TYPE>(chunksize, v);
                    data[v] = a;
                }
                else
                { 
                    a = data[v];
                }

                a[x % chunksize, y % chunksize, z % chunksize] = value;
            }
        }



        public int ChunkSize
        {
            get
            {
                return chunksize;
            }

            set
            {
                chunksize = value;
            }
        }
    }
}
