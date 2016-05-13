using UnityEngine;
using System.Collections;

[AddComponentMenu("Planet/Destroyer")]
public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.K))
        {
            Destroy(gameObject);
        }
    }
}
