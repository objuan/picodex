using UnityEngine;
using System.Collections;

public class VXCMCamera : MonoBehaviour {

    public RenderTexture renderTexture = null;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CleanUpTextures()
    {
        if (renderTexture)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }
    }

    void OnPreCull()
    {
        if (!enabled)// || !renderer || !renderer.sharedMaterial || !renderer.enabled)
            return;

        CleanUpTextures();

        renderTexture = RenderTexture.GetTemporary((int)Camera.current.pixelRect.width, (int)Camera.current.pixelRect.height, 0, RenderTextureFormat.RFloat);
        renderTexture.name = "VXCMBuffer_"+name;
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.filterMode = FilterMode.Point;

        RenderTexture currentActiveRT = RenderTexture.active; // store current active rt
        Graphics.SetRenderTarget(renderTexture); // set to target rt

        GL.Clear(true, true, Color.clear);
        renderTexture.DiscardContents(true, true);

        RenderTexture.active = currentActiveRT;

    }
}
