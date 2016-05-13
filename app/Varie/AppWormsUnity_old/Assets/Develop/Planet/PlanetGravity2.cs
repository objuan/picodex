using UnityEngine;
using System.Collections;

[AddComponentMenu("Planet/Gravity2")]
public class PlanetGravity2 : MonoBehaviour
{

    //Declare Variables:

    //Strength of attraction from your sphere (obviously, it can be any type of game-object)
    public float StrengthOfAttraction;

    //Obviously, you won't be using planets, so change this variable to whatever you want
    GameObject planet;

    //Initialise code:
    public void Start()
    {
        //Again, you can change the tag to whatever you want.
        planet = GameObject.FindGameObjectWithTag("Planet");
    }

    //Use FixedUpdate because we are controlling the orbit with physics
    public void FixedUpdate()
    {
        //Declare Variables:

        //magsqr will be the offset squared between the object and the planet
        float magsqr;

        //offset is the distance to the planet
        Vector3 offset;

        //get offset between each planet and the player
        offset = planet.transform.position - transform.position;

        //Offset Squared:
        magsqr = offset.sqrMagnitude;

        //Check distance is more than 0 to prevent division by 0
        if (magsqr > 0.0001f)
        {
            //Create the gravity- make it realistic through division by the "magsqr" variable

            GetComponent<Rigidbody>().AddForce((StrengthOfAttraction * offset.normalized / magsqr) * GetComponent<Rigidbody>().mass);
        }
    }
}
