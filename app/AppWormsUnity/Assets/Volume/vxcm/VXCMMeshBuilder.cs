using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace Picodex.Vxcm
{
    public class VXCMMeshBuilder 
    {

        public static GameObject CreateMesh(VXCMVolume volume,Vector3i chunkSize)
        {
            MarchingCubes.SetTarget(0);
            Mesh m = MarchingCubes.CreateMesh(volume.DF, volume.resolution);

            GameObject gameObject = new GameObject("Mesh");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = m;
            gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/SampleDiffuse"));

            return gameObject;
        }

        public static GameObject CreateMeshTransvoxel(VXCMVolume volume, Vector3i chunkSize,int lod)
        {
            Mesh mesh = new TransvoxelExtractor(new VXCMVolumeData(volume)).GenLodCell(new Vector3i(0,0,0), volume.resolution, lod);
          //  new TransvoxelManager().ExtractMesh()

            GameObject gameObject = new GameObject("Mesh");
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/SampleDiffuse"));

            return gameObject;
        }

    }


}
