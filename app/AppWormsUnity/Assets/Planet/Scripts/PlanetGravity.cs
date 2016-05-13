using UnityEngine;
using System.Collections;

[AddComponentMenu("Planet/Gravity")]
public class PlanetGravity : MonoBehaviour
{
    private GameObject planetObject;
    private Planet planet;
    private Rigidbody body;

    public float gravitationalAcceleration = 9.8f;
    public float maxGravDist = 100.0f;
    public float maxGravity = 35.0f;

    // Use this for initialization
    public void Start () {
        planetObject = GameObject.FindGameObjectWithTag("Planet");
        planet = planetObject.GetComponent<Planet>();
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    // Update is called once per frame
    public void FixedUpdate() {

        if (!planet) return;

        var dist = Vector3.Distance(planet.transform.position, transform.position);
        if (dist <= maxGravDist)
        {
            var dir = planet.transform.position - transform.position;

        // body.AddForce(dir.normalized * (1.0f - dist / maxGravDist) * maxGravity);
            body.AddForce(dir.normalized * planet.gravity);

            //   body.velocity += gravitationalAcceleration * Time.fixedTime * (planet.transform.position- transform.position);
        }
    }
}

