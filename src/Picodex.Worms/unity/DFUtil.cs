using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Picodex.Vxcm;

//namespace Picodex.Unity
//{
    public class DFUtil
    {
        public static Texture3D getTexture()
        {
            VXCMVolume volume = getVolume();

            Color32[] cc = getBlock3D();

            Texture3D buffer = new Texture3D(64, 64, 64, TextureFormat.RGBA32, false);
            buffer.SetPixels32(volume.DF);
            buffer.Apply();

            return buffer;
        }

        public static VXCMVolume getVolume()
        {
            VXCMVolume volume = new VXCMVolume(new VolumeRegion(-32,-32,-32,32,32,32), 1, -2, 2);

            VolumePrimitiveSphere raster = new VolumePrimitiveSphere(volume);

            GeometrySample sample = new GeometrySample();
            sample.debugColor = new Vector3(1, 0, 0);
            //raster.Raster(new Vector3(0, 0, 0), 10, sample);

            raster.Raster(new Vector3(-5, 0, 0), 10, sample);
            raster.Raster(new Vector3(5, 0, 0), 10, sample);

            return volume;
        }

        public static Color32[]  getBlock3D()
        {
            Color32[] cc = new Color32[64 * 64 * 64];
            for (byte x = 0; x < 64; x++)
            {
                for (byte y = 0; y < 64; y++)
                {
                    for (byte z = 0; z < 64; z++)
                    {
                        cc[x + y * 64 + z * 64 * 64] = new Color32((byte)(x * 4), (byte)(y * 4), (byte)(z * 4), 255);
                    }
                }
            }
            return cc;
        }
    }
//}
