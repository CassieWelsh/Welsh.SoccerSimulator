using UnityEditor;
using UnityEngine;

namespace Constructors
{
    public class FieldConstructor : IConstructible
    {
        private readonly GameObject _gatePrefab;
        private readonly GameObject _scoreboardPrefab;

        public FieldConstructor(GameObject gatePrefab, GameObject scoreboardPrefab)
        {
            _gatePrefab = gatePrefab;
            _scoreboardPrefab = scoreboardPrefab;
        }

        private const float Length = 40f;
        private const float Width = 65f;
        private const float Height = 0.5f;

        public GameObject PrepareObject()
        {
            var go = new GameObject("Field");
            var p = go.transform.position;
            p.x = p.y = p.z = 0;
            CreateMesh(go);
            CreateRigidbody(go);
            CreateGates(go);
            CreateScoreboard(go);

            return go;
        }

        private void CreateScoreboard(GameObject go)
        {
            var gateFirst = Object.Instantiate(_scoreboardPrefab);
            var p0 = gateFirst.transform.position;
            p0.x = 0;
            p0.y = -1;
            p0.z = -20f;
            gateFirst.transform.position = p0;
        }

        private void CreateGates(GameObject go)
        {
            var gateFirst = Object.Instantiate(_gatePrefab);
            var p0 = gateFirst.transform.position;
            p0.x = -24;
            p0.y = 1.6f;
            p0.z = -.5f;
            gateFirst.transform.position = p0;

            var gateSecond = Object.Instantiate(_gatePrefab);
            var p1 = gateSecond.transform.position;
            p1.x = 24;
            p1.y = 1.6f;
            p1.z = .5f;

            var r1 = gateSecond.transform.rotation;
            r1.y = 180;

            gateSecond.transform.rotation = r1;
            gateSecond.transform.position = p1;
        }

        private static void CreateRigidbody(GameObject go)
        {
            go.AddComponent<BoxCollider>();
            go.AddComponent<Rigidbody>();
            var rb = go.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        private static void CreateMesh(GameObject go)
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

            var uvScale = 40000f; // Adjust this scale factor as needed to fit the texture properly

            var uv = new Vector2[]
            {
                // UVs for the top face
                new(0, 0),
                new(0, 1),
                new(1, 1),
                new(1, 0),

                // UVs for the side faces
                new(0, 0),
                new(0, uvScale),
                new(uvScale, uvScale),
                new(uvScale, 0)
            };

            var mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.uv = uv; // Assign UV coordinates to the mesh
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

            go.AddComponent<MeshFilter>().mesh = mesh;

            go.AddComponent<MeshRenderer>();
            go.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/grass2");
            //new(Shader.Find("Custom/FieldShader"));

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}