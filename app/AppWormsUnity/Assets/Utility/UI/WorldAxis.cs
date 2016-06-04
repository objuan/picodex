using UnityEngine;
using System.Collections;

public class WorldAxis : MonoBehaviour
{
    public float size = 1f;

    void OnPostRender()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.right * size, Vector3.zero);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.up * size, Vector3.zero);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.forward * size, Vector3.zero);
        Gizmos.color = Color.white;
    }
}