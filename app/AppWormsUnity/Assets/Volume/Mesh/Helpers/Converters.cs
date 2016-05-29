using UnityEngine;

namespace Picodex
{
    public static class Converters
    {
        //public static Vector3 ToVector3(this Vector3 v)
        //{
        //    return new Vector3(v.x, v.y, v.z);
        //}
        //public static Vector3 ToVector3(this Vector3 v)
        //{
        //    return new Vector3(v.x, v.y, v.z);
        //}
        public static Vector3 ToVector3(this Vector3i v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
        public static Vector3i ToVector3I(this Vector3 v)
        {
            return new Vector3i((int)v.x, (int)v.y, (int)v.z);
        }

        //public static VertexPositionTextureNormalColor[] ConvertMeshToXna(Mesh m, Color color)
        //{
        //    var i = m.GetIndices(0);
        //    VertexPositionTextureNormalColor[] vertices = new VertexPositionTextureNormalColor[m.vertices.Length];

        //    // minimize garbage by accessing list
        //    for (int vert = 0; vert < m.vertices.Length; vert++)
        //    {
        //        // project texture coord using x/y
        //        var uv = new Vector2(m.vertices[vert].x, m.vertices[vert].y);

        //        vertices[vert] = new VertexPositionTextureNormalColor(m.vertices[vert].ToVector3(), uv, color.ToVector4()) { Normal = m.Normals[vert].ToVector3() };
        //    }
        //    BuildTangent(ref vertices, ref i);
        //    //BuildTangentSpaceDataForTriangleList(ref vertices, ref i);

        //    return vertices;
        //}

        public static void BuildTangentSpaceDataForTriangleList(ref VertexPositionTextureNormalColor[] vertices, ref ushort[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = new Vector3(0, 0, 0);
                vertices[i].Tangent = new Vector3(0, 0, 0);
                vertices[i].Binormal = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index_vert0 = indices[i];
                int index_vert1 = indices[i + 1];
                int index_vert2 = indices[i + 2];

                Vector3 firstvec = vertices[index_vert1].Position - vertices[index_vert0].Position;
                Vector3 secondvec = vertices[index_vert0].Position - vertices[index_vert2].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                vertices[index_vert0].Normal += normal;
                vertices[index_vert1].Normal += normal;
                vertices[index_vert2].Normal += normal;


                Vector2 uv0 = vertices[index_vert0].TextureCoordinate1;
                Vector2 uv1 = vertices[index_vert1].TextureCoordinate1;
                Vector2 uv2 = vertices[index_vert2].TextureCoordinate1;

                float s1 = uv1.x - uv0.x;
                float s2 = uv2.x - uv0.x;
                float t1 = uv1.y - uv0.y;
                float t2 = uv2.y - uv0.y;

                float r = s1 * t2 - s2 * t1;
                if (r != 0.0f)
                {
                    r = 1.0f / r;

                    Vector3 position0 = vertices[index_vert0].Position;
                    Vector3 position1 = vertices[index_vert1].Position;
                    Vector3 position2 = vertices[index_vert2].Position;

                    float x1 = position1.x - position0.x;
                    float x2 = position2.x - position0.x;
                    float y1 = position1.y - position0.y;
                    float y2 = position2.y - position0.y;
                    float z1 = position1.z - position0.z;
                    float z2 = position2.z - position0.z;

                    Vector3 tangent = new Vector3(
                        (t2 * x1 - t1 * x2) * r,
                        (t2 * y1 - t1 * y2) * r,
                        (t2 * z1 - t1 * z2) * r);

                    Vector3 binormal = new Vector3(
                        (s1 * x2 - s2 * x1) * r,
                        (s1 * y2 - s2 * y1) * r,
                        (s1 * z2 - s2 * z1) * r);

                    vertices[index_vert0].Tangent += tangent;
                    vertices[index_vert1].Tangent += tangent;
                    vertices[index_vert2].Tangent += tangent;

                    vertices[index_vert0].Binormal += binormal;
                    vertices[index_vert1].Binormal += binormal;
                    vertices[index_vert2].Binormal += binormal;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
                vertices[i].Tangent.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }

        public static void BuildTangent(ref VertexPositionTextureNormalColor[] vertices, ref ushort[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                //vertices[i].Normal = new Vector3(0, 0, 0);
                vertices[i].Tangent = new Vector3(0, 0, 0);
                vertices[i].Binormal = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index_vert0 = indices[i];
                int index_vert1 = indices[i + 1];
                int index_vert2 = indices[i + 2];

                //Vector3 firstvec = vertices[index_vert1].Position - vertices[index_vert0].Position;
                //Vector3 secondvec = vertices[index_vert0].Position - vertices[index_vert2].Position;
                //Vector3 normal = Vector3.Cross(firstvec, secondvec);
                //vertices[index_vert0].Normal += normal;
                //vertices[index_vert1].Normal += normal;
                //vertices[index_vert2].Normal += normal;


                Vector2 uv0 = vertices[index_vert0].TextureCoordinate1;
                Vector2 uv1 = vertices[index_vert1].TextureCoordinate1;
                Vector2 uv2 = vertices[index_vert2].TextureCoordinate1;

                float s1 = uv1.x - uv0.x;
                float s2 = uv2.x - uv0.x;
                float t1 = uv1.y - uv0.y;
                float t2 = uv2.y - uv0.y;

                float r = s1 * t2 - s2 * t1;
                if (r != 0.0f)
                {
                    r = 1.0f / r;

                    Vector3 position0 = vertices[index_vert0].Position;
                    Vector3 position1 = vertices[index_vert1].Position;
                    Vector3 position2 = vertices[index_vert2].Position;

                    float x1 = position1.x - position0.x;
                    float x2 = position2.x - position0.x;
                    float y1 = position1.y - position0.y;
                    float y2 = position2.y - position0.y;
                    float z1 = position1.z - position0.z;
                    float z2 = position2.z - position0.z;

                    Vector3 tangent = new Vector3(
                        (t2 * x1 - t1 * x2) * r,
                        (t2 * y1 - t1 * y2) * r,
                        (t2 * z1 - t1 * z2) * r);

                    Vector3 binormal = new Vector3(
                        (s1 * x2 - s2 * x1) * r,
                        (s1 * y2 - s2 * y1) * r,
                        (s1 * z2 - s2 * z1) * r);

                    vertices[index_vert0].Tangent += tangent;
                    vertices[index_vert1].Tangent += tangent;
                    vertices[index_vert2].Tangent += tangent;

                    vertices[index_vert0].Binormal += binormal;
                    vertices[index_vert1].Binormal += binormal;
                    vertices[index_vert2].Binormal += binormal;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
                vertices[i].Tangent.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }

        private static void CalculateNormals(ref VertexPositionTextureNormalColor[] vertices, ref ushort[] indices)
        {
            //for (int i = 0; i < vertices.Length; i++)
            //    vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                Vector3 firstvec = vertices[indices[i * 3 + 1]].Position - vertices[indices[i * 3]].Position;
                Vector3 secondvec = vertices[indices[i * 3]].Position - vertices[indices[i * 3 + 2]].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                vertices[indices[i * 3]].Normal += normal;
                vertices[indices[i * 3 + 1]].Normal += normal;
                vertices[indices[i * 3 + 2]].Normal += normal;

                // binormal/tangent
                Vector3 v1 = vertices[indices[i * 3]].Position;
                Vector3 v2 = vertices[indices[i * 3 + 1]].Position;
                Vector3 v3 = vertices[indices[i * 3 + 2]].Position;

                Vector2 w1 = vertices[indices[i * 3]].TextureCoordinate1;
                Vector2 w2 = vertices[indices[i * 3 + 1]].TextureCoordinate1;
                Vector2 w3 = vertices[indices[i * 3 + 2]].TextureCoordinate1;

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;

                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                // Gram-Schmidt orthogonalize  
                Vector3 tangent = sdir - normal * Vector3.Dot(normal, sdir);
                tangent.Normalize();
                vertices[indices[i * 3]].Tangent += tangent;
                vertices[indices[i * 3 + 1]].Tangent += tangent;
                vertices[indices[i * 3 + 2]].Tangent += tangent;

                // Calculate handedness (here maybe you need to switch >= with <= depend on the geometry winding order)  
                float tangentdir = (Vector3.Dot(Vector3.Cross(normal, sdir), tdir) <= 0.0f) ? 1.0f : -1.0f;
                Vector3 binormal = Vector3.Cross(normal, tangent) * tangentdir;
                vertices[indices[i * 3]].Binormal += binormal;
                vertices[indices[i * 3 + 1]].Binormal += binormal;
                vertices[indices[i * 3 + 2]].Binormal += binormal;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Tangent.Normalize();
                vertices[i].Normal.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }

        public static void CalculateNormals(ref Mesh mesh)
        {
            //for (int i = 0; i < vertices.Length; i++)
            //    vertices[i].Normal = new Vector3(0, 0, 0);

            int[] indices =  mesh.GetIndices(0);
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = new Vector3[vertices.Length] ;

            for (int i = 0; i < indices.Length / 3; i++)
            {
                Vector3 firstvec = vertices[indices[i * 3 + 1]] - vertices[indices[i * 3]];
                Vector3 secondvec = vertices[indices[i * 3]] - vertices[indices[i * 3 + 2]];
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();

                normals[indices[i * 3]] += normal;
                normals[indices[i * 3 + 1]] += normal;
                normals[indices[i * 3 + 2]] += normal;

                // binormal/tangent
                //Vector3 v1 = vertices[indices[i * 3]];
                //Vector3 v2 = vertices[indices[i * 3 + 1]];
                //Vector3 v3 = vertices[indices[i * 3 + 2]];

                //Vector2 w1 = vertices[indices[i * 3]].TextureCoordinate1;
                //Vector2 w2 = vertices[indices[i * 3 + 1]].TextureCoordinate1;
                //Vector2 w3 = vertices[indices[i * 3 + 2]].TextureCoordinate1;

                //float x1 = v2.x - v1.x;
                //float x2 = v3.x - v1.x;
                //float y1 = v2.y - v1.y;
                //float y2 = v3.y - v1.y;
                //float z1 = v2.z - v1.z;
                //float z2 = v3.z - v1.z;

                //float s1 = w2.x - w1.x;
                //float s2 = w3.x - w1.x;
                //float t1 = w2.y - w1.y;
                //float t2 = w3.y - w1.y;

                //float r = 1.0f / (s1 * t2 - s2 * t1);
                //Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                //Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                // Gram-Schmidt orthogonalize  
                //Vector3 tangent = sdir - normal * Vector3.Dot(normal, sdir);
                //tangent.Normalize();
                //vertices[indices[i * 3]].Tangent += tangent;
                //vertices[indices[i * 3 + 1]].Tangent += tangent;
                //vertices[indices[i * 3 + 2]].Tangent += tangent;

                //// Calculate handedness (here maybe you need to switch >= with <= depend on the geometry winding order)  
                //float tangentdir = (Vector3.Dot(Vector3.Cross(normal, sdir), tdir) <= 0.0f) ? 1.0f : -1.0f;
                //Vector3 binormal = Vector3.Cross(normal, tangent) * tangentdir;
                //vertices[indices[i * 3]].Binormal += binormal;
                //vertices[indices[i * 3 + 1]].Binormal += binormal;
                //vertices[indices[i * 3 + 2]].Binormal += binormal;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                //    vertices[i].Tangent.Normalize();
                normals[i].Normalize();
            //    vertices[i].Binormal.Normalize();
            }

            mesh.normals = normals;
        }
    }


}