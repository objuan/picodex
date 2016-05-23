using UnityEngine;

namespace Picodex.VolumeData
{
    public interface  IVolumeData<TYPE>
    {
        TYPE this[int x, int y, int z] { get; set; }

        TYPE this[Vector3i v]
        {
            get;

            set;
        }

        int ChunkSize { get; set; }
    }
}