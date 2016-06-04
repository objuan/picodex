using UnityEngine;

namespace Picodex
{
    public class DebugGame
    {
        public float radius = 3.0f;

        static Material lineMaterial;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                var shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        public static void DrawLine(Vector3 from, Vector3 to,Color color)
        {
            CreateLineMaterial();
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
           // GL.MultMatrix(transform.localToWorldMatrix);


            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex3(from.x, from.y, from.z);
            GL.Vertex3(to.x, to.y, to.z);
            GL.End();

            GL.PopMatrix();
        }
    }
}
