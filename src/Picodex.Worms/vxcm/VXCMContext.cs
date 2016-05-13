//using Assets.VXCM.Scripts.core;
//using Assets.VXCM.Scripts.native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Picodex.Vxcm
{
    public class VXCMContext  
    {
        private static VXCMContext instance=null;
        private int usageCount=0;

        public static VXCMContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VXCMContext();
                 }
                return instance;
            }
        }


        public void useContext()
        {
            if (usageCount == 0)
            {
#if VXCM_DEBUG
                NativeAPI.LoadLibrary();
#endif
            }
            usageCount++;
        }

        public void freeContext()
        {
            usageCount--;
            if (usageCount <= 0)
            {
#if VXCM_DEBUG
                NativeAPI.FreeLibrary();
#endif
            }
        }

        public bool IsActive
        {
            get{
                return usageCount > 0;
            }
        }

        //public VXCMVolume CreateVolume()
        //{
        //    if (!VXCMContext.Instance.IsActive) throw new VXCMException("Stat the VXCMContext before");
        //    return new VXCMVolume();
        //}
        //public VXCMVolume CreateVolumeFromAsset(String assetPath)
        //{
        //    if (!VXCMContext.Instance.IsActive) throw new VXCMException("Stat the VXCMContext before");
        //    VXCMVolume vol = new VXCMVolume(assetPath);
        //    return vol;
        //}
        //public VXCMVolume CreateVolumeFromTexture(Texture3D txt)
        //{
        //    if (!VXCMContext.Instance.IsActive) throw new VXCMException("Stat the VXCMContext before");
        //    VXCMVolume vol =  new VXCMVolume();
        //    vol.SetToTexture(txt);
        //    return vol;
        //}
    }
}
