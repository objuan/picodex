using UnityEngine;
using System.Collections;

using picodex.worms;
using Picodex.Vxcm;

[AddComponentMenu("Vxcm/DFVolume")]
public class DFVolumeOld : MonoBehaviour {

    private VXCMVolume volume;

    public Texture3D buffer;

    // Use this for initialization
    void Start () {

        volume = DFUtil.getVolume();

        buffer = new Texture3D(64, 64, 64, TextureFormat.RGBA32, false);
        buffer.SetPixels32(volume.DF);
        buffer.Apply();
    }

    // Update is called once per frame
    void Update () {

      

    }

   
}
