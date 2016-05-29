using UnityEngine;
using System.Collections;

[AddComponentMenu("Planet/PlanetMover")]
public class PlanetMover : MonoBehaviour {

    public float distance = 100f;
    public float orbitSpeed = 10;
    public float revolutionSpeed = 0;

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (orbitSpeed != 0)
        {
            transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), orbitSpeed * Time.deltaTime);
            transform.LookAt(Vector3.zero);
        }
        if (revolutionSpeed != 0)
        {
            transform.Rotate(0, revolutionSpeed * Time.deltaTime, 0);
        }
    }
}
