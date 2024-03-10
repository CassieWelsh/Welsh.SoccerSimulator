using UnityEngine;

namespace ObjectConstructors
{
    public class FieldConstructor : IConstructible
    {
        private const float Length = 10f;
        private const float Width = 20f;
        private const float Height = 0.5f;

        public GameObject PrepareObject()
        {
            Debug.Log("Created");
            var go = new GameObject("Field");
            var p = go.transform.position;
            p.x = p.y = p.z = 0;
            var mesh = CreateMesh();
            go.AddComponent<MeshFilter>();
            go.GetComponent<MeshFilter>().mesh = mesh;

            go.AddComponent<MeshRenderer>();
            go.GetComponent<MeshRenderer>().material = new(Shader.Find("Custom/FieldShader"));

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return go;
        }

        private Mesh CreateMesh()
        {
            var vertices = new Vector3[]
            {
                new(-Width / 2, 0, -Length / 2),
                new(-Width / 2, 0, Length / 2),
                new(Width / 2, 0, Length / 2),
                new(Width / 2, 0, -Length / 2),

                new(-Width / 2, -Height, -Length / 2),
                new(-Width / 2, -Height, Length / 2),
                new(Width / 2, -Height, Length / 2),
                new(Width / 2, -Height, -Length / 2)
            };

            var mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = new[]
            {
                // Upside
                0, 1, 2,
                2, 3, 0,
                //Side
                0, 4, 5,
                5, 1, 0,
                // Side
                1, 5, 6,
                6, 2, 1,
                //
                7, 3, 2,
                2, 6, 7,
                //
                0, 3, 7,
                7, 4, 0
                // Downside (no need to render)
                //4, 7, 6,
                //6, 5, 4
            };

            return mesh;
        }
    }
}