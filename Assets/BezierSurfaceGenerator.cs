using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierSurface : MonoBehaviour
{
    public float radius = 1f;
    private int numPoints = 32;

    public Vector3[] _inputPoints;
    private Vector3[] _controlPoints;
    private Vector3[] _generatedPoints;
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();

        _controlPoints = new Vector3[numPoints];

        var t = 0f;
        var tStep = 1f / _controlPoints.Length;
        for (var i = 0; i < _controlPoints.Length; i++)
        {
            _controlPoints[i] = CalculateBezierPoint(
                _inputPoints[0],
                _inputPoints[1],
                _inputPoints[2],
                _inputPoints[3],
                t
            );
            t += tStep;
        }

        GeneratePoints();
        CreateMesh();
    }

    private void Update()
    {
        _controlPoints = new Vector3[numPoints];

        var t = 0f;
        var tStep = 1f / _controlPoints.Length;
        for (var i = 0; i < _controlPoints.Length; i++)
        {
            _controlPoints[i] = CalculateBezierPoint(
                _inputPoints[0],
                _inputPoints[1],
                _inputPoints[2],
                _inputPoints[3],
                t
            );
            t += tStep;
        }

        GeneratePoints();
        CreateMesh();

        /*
        GeneratePoints();
        CreateMesh();
    */
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

    // Функция для вычисления точки кривой Безье для заданного значения t
    static Vector3 CalculateBezierPoint(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, double t)
    {
        var u = 1 - t;
        var tt = t * t;
        var uu = u * u;
        var uuu = uu * u;
        var ttt = tt * t;

        // Формула кривой Безье третьего порядка
        var x = (float)(uuu * P0.x + 3 * uu * t * P1.x + 3 * u * tt * P2.x + ttt * P3.x);
        var y = (float)(uuu * P0.y + 3 * uu * t * P1.y + 3 * u * tt * P2.y + ttt * P3.y);
        var z = (float)(uuu * P0.z + 3 * uu * t * P1.z + 3 * u * tt * P2.z + ttt * P3.z);

        return new(x, y, z);
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

        var triangles = new List<int>();
        for (var i = 0; i < numPoints - 1; i++)
        {
            for (var j = 0; j < _controlPoints.Length - 1; j++)
            {
                triangles.Add(i * _controlPoints.Length + j);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);

                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add(i * _controlPoints.Length + j);


                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add((i + 1) * _controlPoints.Length + (j + 1) % _controlPoints.Length);

                triangles.Add((i + 1) * _controlPoints.Length + (j + 1) % _controlPoints.Length);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);

                /*
                triangles.Add(i * _controlPoints.Length + j);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);

                triangles.Add(i * _controlPoints.Length + (j + 1) % _controlPoints.Length);
                triangles.Add((i + 1) * _controlPoints.Length + j);
                triangles.Add((i + 1) * _controlPoints.Length + (j + 1) % _controlPoints.Length);
                */
            }
        }

        mesh.triangles = triangles.ToArray();

        var uv = new Vector2[_generatedPoints.Length];
        for (var i = 0; i < _generatedPoints.Length; i += 4)
        {
            uv[i] = new(0, 0);
            uv[i + 1] = new(0, 1);
            uv[i + 2] = new(1, 1);
            uv[i + 3] = new(1, 0);
        }

        mesh.uv = uv;
        meshFilter.mesh = mesh;
    }
}