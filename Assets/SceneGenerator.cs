using System.Collections.Generic;
using ObjectConstructors;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    private List<GameObject> _objects;

    private void Start()
    {
        var _objectsToDraw = GenerateSceneExtensions.GetObjects();
        _objects = new(_objectsToDraw.Length);
        foreach (var obj in _objectsToDraw)
            _objects.Add(obj.PrepareObject());
    }
    /*
    private GameObject _go;

    // Start is called before the first frame update
    private void Start()
    {
        print("Created");
        _go = new GameObject();
        var p = _go.transform.position;
        p.x = p.y = p.z = 0;

        var vertices = new Vector3[]
        {
            new(-1, 0, 0),
            new(0, 0, 1),
            new(1, 0, 0)
        };
        var mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = new[] { 0, 1, 2 };

        _go.AddComponent<MeshFilter>();
        _go.AddComponent<MeshRenderer>();
        _go.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
*/
}