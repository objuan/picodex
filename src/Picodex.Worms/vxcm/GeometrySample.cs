using System;
using System.Collections.Generic;

using System.Text;

using MaterialID = System.Int32;
using UnityEngine;

namespace Picodex.Vxcm
{
    public class GeometrySample
    {
        //	Vector3 normal; // not used
        public MaterialID material;
        public Vector3 debugColor; // main material color

        public byte distanceField;

        public Vector3 normal;

	    public GeometrySample()
	    {
	    }

	    //void set(const GeometryMaterial &voxel,float density){
	    //}
	    void set( GeometrySample sample){
		    material = sample.material;
		    debugColor = sample.debugColor;
		    distanceField = sample.distanceField;
		    normal = sample.normal;
	    }
    }
}
