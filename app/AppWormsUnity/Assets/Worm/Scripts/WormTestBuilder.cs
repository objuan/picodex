using UnityEngine;
using System.Collections;

using Picodex;
using Picodex.Vxcm;

[ExecuteInEditMode]
public class WormTestBuilder : MonoBehaviour {

    DFVolume volume;

    public float ray = 10;

    public bool sphereMode = false;

    // Use this for initialization
    void Start () {

        //  Build();
        volume = GetComponent<DFVolumeFilter>().volume;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clear()
    {
      //  volume = GetComponent<DFVolumeFilter>().volume;

        // sfera

        volume.Clear();

        VolumePrimitive raster = new VolumePrimitive(volume);
        raster.CutMode = false;
        // raster.IsFilled = true;

        float ray = volume.resolution.x /2 -4;

        GeometrySample sample = new GeometrySample();
        sample.debugColor = new Vector3(1, 0, 0);

        if (!sphereMode)
            raster.RasterBox(new Vector3(-ray, -4,-ray), new Vector3(ray, 4, ray), sample);
        else
            raster.RasterSphere(new Vector3(0,0,0), ray, sample);
    }

    public void AddObstacle(Vector3 pos)
    {
        volume = GetComponent<DFVolumeFilter>().volume;

        VolumePrimitive raster = new VolumePrimitive(volume);
        raster.CutMode = false;
        raster.IsFilled = false;

        GeometrySample sample = new GeometrySample();
        sample.debugColor = new Vector3(1, 0, 0);

        //   hit.point
        pos = transform.worldToLocalMatrix.MultiplyPoint(pos);

        //if (shape == VolumeEditor_ShapeType.Circle)
        //{
        //    Debug.Log("RasterSphere");
        //    raster.RasterSphere(p, ray, sample);
        //}
        //else
        {
            Bounds b = new Bounds(pos, new Vector3(ray, ray, ray));
            Debug.Log("RasterBox");
            raster.RasterBox(b.min, b.max, sample);
        }
    }
}
