using UnityEngine;
using System.Collections;
using UnityEditor;

using System.IO;

//using Picodex.Unity.Vxcm;
using Picodex.Vxcm;

namespace Picodex.Volume
{
    public class Main : MonoBehaviour
    {
     
        [MenuItem("GameObject/Create Other/DF Volume")]
        static void CreateDFVolume()
        {

            DFVolumeData data = AssetManager.CreateEmptyVolumeData<DFVolumeData>(new Vector3i(64,64,64 ));

            // TEST 
            VolumePrimitiveSphere raster = new VolumePrimitiveSphere(data.volume);

            GeometrySample sample = new GeometrySample();
            sample.debugColor = new Vector3(1, 0, 0);
            //raster.Raster(new Vector3(0, 0, 0), 10, sample);

            raster.Raster(new Vector3(-10,0,0), 10, sample);
            raster.Raster(new Vector3(10,0,0), 10, sample);
            raster.Raster(new Vector3(0, -10, 0), 10, sample);
            raster.Raster(new Vector3(0, 10, 0), 10, sample);
            raster.Raster(new Vector3(0, 0, -10), 10, sample);
            raster.Raster(new Vector3(0,0, 10), 10, sample);


            GameObject go = DFVolume.CreateGameObject(data, true, true);

            // And select it, so the user can get straight on with editing.
            Selection.activeGameObject = go;

            //int floorThickness = 8;
            //QuantizedColor floorColor = new QuantizedColor(192, 192, 192, 255);

            //for (int z = 0; z <= depth - 1; z++)
            //{
            //    for (int y = 0; y < floorThickness; y++)
            //    {
            //        for (int x = 0; x <= width - 1; x++)
            //        {
            //            data.SetVoxel(x, y, z, floorColor);
            //        }
            //    }
            //}
        }

    }
}
