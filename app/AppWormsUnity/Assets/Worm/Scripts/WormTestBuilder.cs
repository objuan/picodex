using UnityEngine;
using System.Collections;

using Picodex;
using Picodex.Vxcm;

public class WormTestBuilder : MonoBehaviour {

    DFVolume volume;

    public float ray = 50;

	// Use this for initialization
	void Start () {
        volume = GetComponent<DFVolumeFilter>().volume;

        Build();

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

        raster.RasterBox(new Vector3(-60,-4,-60), new Vector3(60, 4, 60), sample);
    }
}
