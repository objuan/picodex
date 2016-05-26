using System;
using System.Collections.Generic;

using System.Text;
 
using UnityEngine;
//using picodex.worms.math;


namespace Picodex.Vxcm
{
  
    //public class VolumePrimitive
    //{
	   // protected VXCMVolume volume;
    //    protected VXCMVolumeAccessor accessor;
    //    public bool CutMode;
    //    public float BlendFactor;
	   // public bool IsFilled;

	   // public  VolumePrimitive(VXCMVolume volume)
    //    {
    //        this.volume=volume;
		  //  accessor = volume.Accessor;
    //        BlendFactor = 1;
    //        CutMode = false;
    //        IsFilled = true;
	   // }
    //};


public class  VolumePrimitive
{
    protected VXCMVolume volume;
    protected VXCMVolumeAccessor accessor;
    public bool CutMode;
    public float BlendFactor;
    public bool IsFilled;

    public  VolumePrimitive(VXCMVolume volume)
    {
        this.volume = volume;
        accessor = volume.Accessor;
        BlendFactor = 1;
        CutMode = false;
        IsFilled = true;
    }

    // ================================================================
    // VolumePrimitiveSphere
    // ==================================================================

    public void RasterSphere(Vector3 _center, float ray, GeometrySample voxel)
	{
		Vector3 _dims = new Vector3(ray, ray, ray);

		// -------------------------------

		Vector3 center = _center;
		Vector3 dims = _dims;//Vector3_MULT(_dims , volume->getWorldToGridScale());
		//// world to grid
	//	volume->getWorldToLocalTrx().transformPoint(&center);

		Vector3 min = center - dims;
		Vector3 max = center + dims;

            //if (!(dx>0.0f)) OPENVDB_THROW(ValueError, "voxel size must be positive");
            //if (!(w>1)) OPENVDB_THROW(ValueError, "half-width must be larger than one");

       // crop to grid volume
        Bounds crop_bbox = volume.bounds;

    //	AxisAlignedBox crop_bbox = volume->convertGridToLocal(gridRegion);

            float dx=1;
        float w_min = volume.distanceFieldRangeMin;
        float w_max = volume.distanceFieldRangeMax;


		Vector3 r0 = new Vector3 ( ((max.x - min.x) /2) / dx,((max.y - min.y) /2) / dx,((max.z - min.z) /2) / dx);
		Vector3 rmax = r0 + new Vector3(w_max,w_max,w_max);

		// Define center of sphere in voxel units
		Vector3 c = new Vector3(center.x / dx, center.y / dx, center.z / dx);

		float w_int = w_min ;//- 0.0001; // interno non deve essere vuoto
		// Define index coordinates and their respective bounds
	//	int[] ijk = new int[3];
		//int &i = ijk[0], &j = ijk[1], &k = ijk[2];
        int imin = (int)System.Math.Floor(c.x - rmax.x); int imax = (int)System.Math.Ceiling(c.x + rmax.x);
        int jmin = (int)System.Math.Floor(c.y - rmax.y); int jmax = (int)System.Math.Ceiling(c.y + rmax.y);
        int kmin = (int)System.Math.Floor(c.z - rmax.z); int kmax = (int)System.Math.Ceiling(c.z + rmax.z);

		// crop to bouns 
		imin = Math.Max(imin,(int)(crop_bbox.min.x));
		jmin = Math.Max(jmin, (int)(crop_bbox.min.y));
		kmin = Math.Max(kmin, (int)(crop_bbox.min.z));

		imax = Math.Min(imax, (int)(crop_bbox.max.x-1));
		jmax = Math.Min(jmax, (int)(crop_bbox.max.y-1));
		kmax = Math.Min(kmax, (int)(crop_bbox.max.z-1));

		Vector3 d;
		//float wn = 0.1f;
		float dens;	
		
		int i,j,k;

        Vector3i ijk = new Vector3i();
	//	SVOAccessor accessor = tree->getAccessor();
		Vector3i minW = new Vector3i(imin,jmin,kmin);
        Vector3i maxW = new Vector3i(imax,jmax,kmax);
		/*volume->getLocalToWorldTrx().transformPoint(&minW);
		volume->getLocalToWorldTrx().transformPoint(&maxW);*/

		accessor.Begin(new VolumeRegion(minW,maxW));

		accessor.CutMode = CutMode;
		accessor.BlendFactor = BlendFactor;

		//float v;
		for (i = imin; i <= imax; ++i) {
			for (j = jmin; j <= jmax; ++j) {
				for (k = kmin; k <= kmax; k ++) {
                   // ijk[0];
                    ijk.x = i;
                    ijk.y = j;
                    ijk.z = k;

					d = (c - new Vector3(i,j,k) );
					dens = d.magnitude - r0.x;

					//voxel.normal = vec3(i,j,k).normalized();
					if (dens >= w_min && dens <=  w_max)
					{
                        voxel.distanceField = accessor.EncodeDistanceField(dx * dens);
						accessor.ChangeValue(ijk, voxel);
					}
					else if (IsFilled)
					{
						if (!(dens >= w_max))
						{
                            voxel.distanceField = accessor.EncodeDistanceField(dx * w_int);
                            accessor.ChangeValue(ijk, voxel);
						}
					}
				}
			}
		}

        accessor.Flush();
	}

        public void RasterBox(Vector3 min, Vector3 max, GeometrySample voxel)
        {

            //Vector3 min, max,
            Vector3 center;
            // min = region.min;
            // max = region.max;

            center = min + (max - min) / 2;

            //volume->getWorldToLocalTrx().transformPoint(&min);
            //volume->getWorldToLocalTrx().transformPoint(&max);
            //volume->getWorldToLocalTrx().transformPoint(&center);

            // start

            float dx = 1;
            float w_min = volume.distanceFieldRangeMin;
            float w_max = volume.distanceFieldRangeMax;

            Vector3 r0 = new Vector3(((max.x - min.x) / 2) / dx, ((max.y - min.y) / 2) / dx, ((max.z - min.z) / 2) / dx);
            Vector3 rmax = r0 + new Vector3(w_max, w_max, w_max);

            // Define center of sphere in voxel units
            Vector3 c = new Vector3(center.x / dx, center.y / dx, center.z / dx);

            float w_int = w_min;// - 0.0001; // interno non deve essere vuoto
                                // Define index coordinates and their respective bounds

            //	int &i = ijk[0], &j = ijk[1], &k = ijk[2];
            int imin = (int)System.Math.Floor(c.x - rmax.x); int imax = (int)System.Math.Ceiling(c.x + rmax.x);
            int jmin = (int)System.Math.Floor(c.y - rmax.y); int jmax = (int)System.Math.Ceiling(c.y + rmax.y);
            int kmin = (int)System.Math.Floor(c.z - rmax.z); int kmax = (int)System.Math.Ceiling(c.z + rmax.z);

            Vector3i ijk = new Vector3i();

            // crop to bouns 
            Bounds crop_bbox = volume.bounds;

            imin = Math.Max(imin, (int)(crop_bbox.min.x));
            jmin = Math.Max(jmin, (int)(crop_bbox.min.y));
            kmin = Math.Max(kmin, (int)(crop_bbox.min.z));

            imax = Math.Min(imax, (int)(crop_bbox.max.x - 1));
            jmax = Math.Min(jmax, (int)(crop_bbox.max.y - 1));
            kmax = Math.Min(kmax, (int)(crop_bbox.max.z - 1));

            //	SVOAccessor accessor = tree->getAccessor();
            Vector3i minW = new Vector3i(imin, jmin, kmin);
            Vector3i maxW = new Vector3i(imax, jmax, kmax);

            /*volume->getLocalToWorldTrx().transformPoint(&minW);
            volume->getLocalToWorldTrx().transformPoint(&maxW);*/

            accessor.Begin(new VolumeRegion(minW, maxW));

            accessor.CutMode = CutMode;
            accessor.BlendFactor = BlendFactor;

            //TODO, altra parte
            // punto ?? 
            if (Mathf.Abs(max.x - min.x) == 0)
            {
                voxel.distanceField = 0;
                accessor.ChangeValue(new Vector3i((int)max.x, (int)max.y, (int)max.z), voxel);
                return;
            }

            int i, j, k;
            float v;
            for (i = imin; i <= imax; ++i)
            {
                for (j = jmin; j <= jmax; ++j)
                {
                    for (k = kmin; k <= kmax; k++)
                    {

                        ijk.x = i;
                        ijk.y = j;
                        ijk.z = k;

                        Vector3 d = new Vector3(Mathf.Abs(center.x - i) - r0.x, Mathf.Abs(center.y - j) - r0.y, Mathf.Abs(center.z - k) - r0.z);

                        /*if (abs(i) >= abs(j) &&  abs(i) >= abs(k)) voxel.normal = Vector3(d.x>=0 ? 1 : -1,0,0);
                        else if (abs(j) >= abs(k)) voxel.normal = Vector3(0,d.y>=0 ? 1 : -1,0);
                        else voxel.normal = Vector3(0,0,d.z>=0 ? 1 : -1);*/

                        if (d.x <= 0 && d.y <= 0 && d.z <= 0)
                        {
                            // punto interno
                            v = Mathf.Max(d.x, Mathf.Max(d.y, d.z));
                            if (v > w_min)
                            {
                                voxel.distanceField = accessor.EncodeDistanceField(dx * v);
                                accessor.ChangeValue(ijk, voxel);

                            }
                            else // if (isFilled())
                            {
                                voxel.distanceField = accessor.EncodeDistanceField(dx * w_int);
                                accessor.ChangeValue(ijk, voxel);
                            }
                        }
                        else {
                            // punto esterno
                            // controllo se e' valido
                            if (d.x >= w_max || d.y >= w_max || d.z >= w_max) continue;

                            v = 999;
                            if (d.x > 0 && d.x <= w_max) v = d.x;
                            if (d.y > 0 && d.y <= w_max && d.y < v) v = d.y;
                            if (d.z > 0 && d.z <= w_max && d.z < v) v = d.z;

                            if (v != 999)
                            {
                                voxel.distanceField = accessor.EncodeDistanceField(dx * v);
                                //voxel.density = 1;// dx*v;
                                accessor.ChangeValue(ijk, voxel);
                            }
                        }
                    }
                }
            }
            accessor.Flush();
        }
    }
};
