using UnityEngine;
using System.Collections;
namespace Picodex
{
    //[AddComponentMenu("Planet/WormCollision")]
    public class WormCollision : MonoBehaviour {

        DFVolumeCollider planetCollider;

        // Use this for initialization
        void Start() {

            planetCollider = GameObject.FindGameObjectWithTag("Planet").GetComponent< DFVolumeCollider>();
        }

        // Update is called once per frame
        void Update() {



        }


    }
}
