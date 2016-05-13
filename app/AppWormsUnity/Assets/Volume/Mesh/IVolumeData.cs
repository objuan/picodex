//using Transvoxel.Math;

using UnityEngine;

namespace Picodex
{
    public abstract class IVolumeData<TYPE>
    {
        public abstract TYPE this[int x, int y, int z] { get; set; }
        public TYPE this[Vector3i v] 
        { 
            get
            {
                return this[v.x,v.y,v.z];
            } 
            
            set
            {
                this[v.x, v.y, v.z] = value;
            }
        }

        public abstract int ChunkSize { get; set; }
    }
}