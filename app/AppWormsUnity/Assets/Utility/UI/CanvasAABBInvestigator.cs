using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// This should only be used for debugging the "Invalid AABB aabb" error - JB

public class CanvasAABBInvestigator : MonoBehaviour
{

    public bool delve = false;

    public List<RectTransform> rts;

    // Use this for initialization
    void Start()
    {
        if (!delve) return;

        rts = new List<RectTransform>();

        RectTransform rt = GetComponent<RectTransform>();
        Delve(rt);
    }

    void Delve(RectTransform rt)
    {
        if (rts.IndexOf(rt) != -1) return;

        rts.Add(rt);
        RectTransform[] tempRTs = rt.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rtT in tempRTs)
        {
            Delve(rtT);
        }
    }

    void Update()
    {
        if (!delve) return;

        // Search for any scale that is 0
        Vector3 scale;
        foreach (RectTransform rt in rts)
        {
            scale = rt.localScale;
            if (scale.x == 0 || scale.y == 0 || scale.z == 0)
            {
                //                Utils.Print("CanvasAABBInvestigator", "RectTransform scale of 0", rt.gameObject.name, scale);
                Debug.LogError(System.String.Format("{0}\t{1}\t{2}\t{3}", "CanvasAABBInvestigator", "RectTransform scale of 0", rt.gameObject.name, scale));
            }
        }
    }

}