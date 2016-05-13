using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;
//using picodex.worms.math;

namespace Picodex.Vxcm
{
    public class VXCMVolumeAccessor
    {
        VXCMVolume volume;
        Color32[] DF;

        float distanceFieldRangeMin;
        float distanceFieldRangeMax;
        float distanceFieldRangeW;
        int size;
        int size2;
     

        // from 0 (trasparent) to 1 (opaque)
        public float BlendFactor = 1;
        public bool CutMode = false;
        GeometrySample tmpGeometrySample = new GeometrySample();

        VolumeAddress localToVolumeTrx;
        VolumeRegion volumeLocalRegion;

        //bool isEditing;
        //List<VolumeRegion> invalidateRegions;

        public VXCMVolumeAccessor(VXCMVolume volume)
        {
            this.volume = volume;
            DF = volume.DF;
            size = volume.region.size.x;
            size2 = volume.region.size.x * volume.region.size.y;
            distanceFieldRangeMin = volume.DistanceFieldRangeMin;
            distanceFieldRangeMax = volume.DistanceFieldRangeMax;
            distanceFieldRangeW = distanceFieldRangeMax - distanceFieldRangeMin;

            localToVolumeTrx = volume.localToVolumeTrx;
            volumeLocalRegion = volume.region;

        }

        public void Begin(VolumeRegion volumeRegion)
        {
        }

        public void Flush()
        {
        }

        // MAX-> 0
        // MIN -> 1
        public byte EncodeDistanceField(float density){
		    // contrario // 0 -> out 1 -> in
            float w = 1.0f - (density - distanceFieldRangeMin) / (distanceFieldRangeW);
		    //return VoxelDensity(65535 * w);
		    return (byte)(w * 255 );
	    }
        public byte decodeDistanceField(byte density)
        {
            
            float w = 1.0f - (density - distanceFieldRangeMin) / (distanceFieldRangeW);
            return (byte)(w * 255);
        }
     
        // direct edit
        public bool ProbeValue(VolumeAddress address, ref GeometrySample voxel)
        {
            byte df = DF[address.x + address.y * size + address.z * size2].r;
            voxel.distanceField = decodeDistanceField(df);
            return df != 0;
        }

        public void SetValue(VolumeAddress address, GeometrySample voxel)
        {
            int idx = address.x + address.y * size + address.z * size2;
            DF[idx].r = voxel.distanceField;
            DF[idx].g = 0;
            DF[idx].b = 0;
            DF[idx].a = 255;

        }

        public void SetValueOff(VolumeAddress address)
        {
            int idx = address.x + address.y * size + address.z * size2;
            DF[idx].r = 0;
            DF[idx].g = 0;
            DF[idx].b = 0;
            DF[idx].a = 255;
        }

        // density read

        public byte GetDistanceField(VolumeAddress address)
        {
            return DF[address.x + address.y * size + address.z * size2].r;
        }

        public void SetDistanceField(VolumeAddress address, byte voxelDistanceField)
        {
            //assert(isEditing);
            int idx = address.x + address.y * size + address.z * size2;
            DF[idx].r = voxelDistanceField;
            DF[idx].g = 0;
            DF[idx].b = 0;
            DF[idx].a = 255;

        }

        // csg operation
        public void csgUnion(VolumeAddress localAddress, GeometrySample voxel)
        {
            VolumeAddress address = localAddress + localToVolumeTrx;

            Debug.Assert(address.x >= 0); Debug.Assert(address.y >= 0); Debug.Assert(address.z >= 0);
            Debug.Assert(address.x < volumeLocalRegion.max.x); Debug.Assert(address.y < volumeLocalRegion.max.y); Debug.Assert(address.z < volumeLocalRegion.max.z);

            byte old_density = GetDistanceField(address);
            float new_density = voxel.distanceField;

            if (BlendFactor == 1)
            {
                // a = max(a, b)
                if (old_density == distanceFieldRangeMax) // era vuoto
                    SetValue(address, voxel);
                else
                {
                    if (new_density > old_density) // solo se la densita' e' minore
                    {
                        SetValue(address, voxel);
                    }
                }
            }
            else
            {
                //	GeometrySampleColor old_color = tmpGeometrySample.color;

                //		tmpGeometrySample.material = GeometrySampleMaterial::blend(voxel.material,tmpGeometrySample.material,(1-blendFactor) );
                //	tmpGeometrySample.color = voxel.color * (1-blendFactor) + old_color * blendFactor;
                tmpGeometrySample.distanceField = (byte)(old_density * (1 - BlendFactor) + new_density * BlendFactor);

                if (old_density == distanceFieldRangeMax) // era vuoto
                {
                    //	setValue(address, tmpGeometrySample);
                }
                else
                {
                    if (new_density < old_density) // solo se la densita' e' minore
                    {
                        SetValue(address, tmpGeometrySample);
                    }
                }
            }
        }

        public void csgIntersection(VolumeAddress address, GeometrySample voxel)
        {
        }

        public void csgDifference(VolumeAddress address, GeometrySample voxel)
        {
            //bool active_dst = ProbeValue(address, tmpGeometrySample);

            //float dens_src = -voxel.distanceField;

            //// a = max(a, -b)

            //if (active_dst)
            //{
            //    //if (dens_src < 0)
            //    //{
            //    //	accessor_density.setValue(ijk, dens_src);
            //    //	/*accessor_density.setValueOff(ijk);
            //    //	accessor_mat.setValueOff(ijk);*/
            //    //}
            //    //else
            //    {
            //        if (dens_src > tmpGeometrySample.distanceField)
            //        {
            //            tmpGeometrySample = voxel;
            //            tmpGeometrySample.distanceField = dens_src;
            //            SetValue(address, tmpGeometrySample);

            //            // set the invalid tree as clear need
            //            //tree->getInvalidTree().invalidClear();
            //        }
            //    }
            //}
        }

        // 

        public void ChangeValue(VolumeAddress address, GeometrySample voxel)
        {
            if (!CutMode) csgUnion(address, voxel);
            else csgDifference(address, voxel);
        }

        // 
        public  void SetValueW(Vector3 address, GeometrySample voxel)
        {
        }
    }

}
