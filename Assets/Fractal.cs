using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Renderer rend;
    public float scale = 1f;
    public Vector2 offset = Vector2.zero;
    public float maxIterations = 100f;
    private static readonly int Offset = Shader.PropertyToID("_Offset");
    private static readonly int Scale = Shader.PropertyToID("_Scale");
    private static readonly int MaxIterations = Shader.PropertyToID("_MaxIterations");

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        rend.material.SetFloat(Scale, scale);
        rend.material.SetVector(Offset, offset);
        rend.material.SetFloat(MaxIterations, maxIterations);
    }
}