using UnityEngine;
using System.Collections;

using Picodex;
using Picodex.Vxcm;

[AddComponentMenu("Planet/Builder")]
public class PlanetBuilder : MonoBehaviour {


    DFVolume volume;

    public float ray = 50;

	// Use this for initialization
	void Start () {
        volume = GetComponent<DFVolumeFilter>().volume;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Build()
    {
        // sfera

        volume.Clear();

        VolumePrimitive raster = new VolumePrimitive(volume);
        raster.CutMode = false;
        // raster.IsFilled = true;

        GeometrySample sample = new GeometrySample();
        sample.debugColor = new Vector3(1, 0, 0);

        raster.RasterSphere(new Vector3(0,0,0), ray, sample);
    }
}
