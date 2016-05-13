using UnityEngine;
using System.Collections;


public class TestOrtho : MonoBehaviour
{
    public Material mat;

   static Material lineMaterial=null;
    static void CreateLineMaterial()
    {
        if (lineMaterial==null)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
         //   var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material();
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }	// Will be called after all regular rendering is done


    void OnPostRender()
    {
        CreateLineMaterial();
        mat = lineMaterial;

        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Color(Color.red);
        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0.25F, 0.1351F, 0);
        GL.Vertex3(0.25F, 0.3F, 0);
        GL.Vertex3(0.5F, 0.3F, 0);
        GL.End();
        GL.Color(Color.yellow);
        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0.5F, 0.25F, -1);
        GL.Vertex3(0.5F, 0.1351F, -1);
        GL.Vertex3(0.1F, 0.25F, -1);
        GL.End();
        GL.PopMatrix();
    }
}
