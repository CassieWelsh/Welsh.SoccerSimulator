using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierSurface : MonoBehaviour
{
    public float radius = 1f;
    public int numPoints = 20;

    public Vector3[] _controlPoints;
    private Vector3[] _generatedPoints;

    private void Start()
    {
        GeneratePoints();
        CreateMesh();

        var r = RotateCurve(_controlPoints, 400f);
    }

    // Метод для вращения криволинейной прямой вокруг оси Y
    private Vector3[] RotateCurve(IList<Vector3> curvePoints, float angle)
    {
        var rotatedCurve = new Vector3[curvePoints.Count];

        // Угол в радианах
        var angleInRadians = angle * Mathf.Deg2Rad;

        for (var i = 0; i < curvePoints.Count; i++)
        {
            // Для каждой точки на кривой вычисляем новую позицию после вращения
            var newX = curvePoints[i].x * Mathf.Cos(angleInRadians) + curvePoints[i].z * Mathf.Sin(angleInRadians);
            var newZ = -curvePoints[i].x * Mathf.Sin(angleInRadians) + curvePoints[i].z * Mathf.Cos(angleInRadians);

            rotatedCurve[i] = new Vector3(newX, curvePoints[i].y, newZ);
        }

        return rotatedCurve;
    }

    private void GeneratePoints()
    {
        _generatedPoints = new Vector3[numPoints * _controlPoints.Length];
        var step = _controlPoints.Length;
        Array.Copy(_controlPoints, _generatedPoints, _controlPoints.Length);
        for (var i = step; i < _generatedPoints.Length; i += step)
        {
            var newPoints = RotateCurve(_generatedPoints[(i - step)..i].ToList(), radius);
            Array.Copy(newPoints, 0, _generatedPoints, i, step);
        }
    }

    private void CreateMesh()
    {
        var mesh = new Mesh();
        mesh.vertices = _generatedPoints;

        var triangles = new int[(numPoints - 1) * _controlPoints.Length * 6];
        var index = 0;

        for (var i = 0; i < numPoints - 1; i++)
        {
            for (var j = 0; j < _controlPoints.Length; j++)
            {
                triangles[index++] = i * _controlPoints.Length + j;
                triangles[index++] = (i + 1) * _controlPoints.Length + j;
                triangles[index++] = i * _controlPoints.Length + (j + 1) % _controlPoints.Length;

                triangles[index++] = i * _controlPoints.Length + (j + 1) % _controlPoints.Length;
                triangles[index++] = (i + 1) * _controlPoints.Length + j;
                triangles[index++] = (i + 1) * _controlPoints.Length + (j + 1) % _controlPoints.Length;
                /*
                triangles[index++] = i * _controlPoints.Length + j;
                triangles[index++] = (i + 1) * _controlPoints.Length + j;
                triangles[index++] = i * _controlPoints.Length + (j + 1) % _controlPoints.Length;

                triangles[index++] = i * _controlPoints.Length + (j + 1) % _controlPoints.Length;
                triangles[index++] = (i + 1) * _controlPoints.Length + j;
                triangles[index++] = (i + 1) * _controlPoints.Length + (j + 1) % _controlPoints.Length;
                */
            }
        }

        mesh.triangles = triangles;

        var uv = new Vector2[_generatedPoints.Length];
        for (var i = 0; i < _generatedPoints.Length; i += 4)
        {
            uv[i] = new(0, 0);
            uv[i + 1] = new(0, 1);
            uv[i + 2] = new(1, 1);
            uv[i + 3] = new(1, 0);
        }

        mesh.uv = uv; // Assign UV coordinates to the mesh


        gameObject.AddComponent<MeshFilter>().mesh = mesh;
    }
}