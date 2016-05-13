using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Picodex.Vxcm;

namespace Picodex
{
    public class DFUtil
    {
        public static Texture3D getTexture(int w,int h,int d)
        {
            VXCMVolume volume = getVolume(w, h, d);

         //   Color32[] cc = getBlock3D();

            Texture3D buffer = new Texture3D(w,h,d, TextureFormat.RGBA32, false);
            buffer.SetPixels32(volume.DF);
            buffer.Apply();

            return buffer;
        }

        public static VXCMVolume getVolume(int w, int h, int d)
        {
            VXCMVolume volume = new VXCMVolume(new Vector3i(w, h,d), 1, -2, 2);

            //   VXCMVolume volume = new VXCMVolume(new VolumeRegion(-w/2,-h/2,-d / 2, w / 2, h/2,d/2), 1, -2, 2);

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
}
