using System;
using UnityEngine;

public class FractalGenerator : MonoBehaviour
{
    public int maxIterations = 100;
    public float zoom = 1f;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public float threshold = 2f;
    public float stepSize = 0.1f;
    public float scale = 1f;

    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateFractal();
    }

    private void GenerateFractal()
    {
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[0];
        int[] triangles = new int[0];

        for (float x = -scale; x < scale; x += stepSize)
        {
            for (float y = -scale; y < scale; y += stepSize)
            {
                Vector3 c = new Vector3(x, y, 0) * zoom + offset;
                Vector3 z = Vector3.zero;
                int iteration = 0;

                while (iteration < maxIterations && z.sqrMagnitude < threshold * threshold)
                {
                    z = new Vector3(
                        z.x * z.x - z.y * z.y + c.x,
                        2 * z.x * z.y + c.y,
                        0
                    );
                    iteration++;
                }

                if (iteration == maxIterations)
                {
                    int vertexIndex = vertices.Length;
                    Array.Resize(ref vertices, vertexIndex + 1);
                    vertices[vertexIndex] = new Vector3(x, y, 0);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}