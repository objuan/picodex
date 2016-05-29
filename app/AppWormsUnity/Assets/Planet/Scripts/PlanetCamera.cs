using UnityEngine;
using System.Collections;

[AddComponentMenu("Planet/PlanetCamera")]
[ExecuteInEditMode]
public class PlanetCamera : MonoBehaviour {


    void Awake()
    {
        RenderSettings.skybox = (Material)Resources.Load("Skybox3");

    }

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
    
    }
}
