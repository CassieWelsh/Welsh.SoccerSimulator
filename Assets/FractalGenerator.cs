using System;
using UnityEngine;

public class FractalGenerator : MonoBehaviour
{
    public int textureWidth = 512;
    public int textureHeight = 512;
    public int iterations = 4;
    public Material material;

    private Texture2D kochTexture;

    void Start()
    {
        kochTexture = new Texture2D(textureWidth, textureHeight);
        material.mainTexture = kochTexture;
        GenerateKochFractal();
    }

    void GenerateKochFractal()
    {
        Color[] colors = new Color[textureWidth * textureHeight];

        // Generate Koch Fractal
        Vector2 startPoint = new Vector2(0, textureHeight / 2);
        Vector2 endPoint = new Vector2(textureWidth, textureHeight / 2);
        Vector2 p1 = startPoint;
        Vector2 p2 = endPoint;

        DrawKochLine(colors, p1, p2, iterations);

        // Draw Triangle
        Vector2 p3 = new Vector2(textureWidth / 2, textureHeight);
        DrawKochLine(colors, p2, p3, iterations);
        DrawKochLine(colors, p3, p1, iterations);

        kochTexture.SetPixels(colors);
        kochTexture.Apply();
    }

    void DrawKochLine(Color[] colors, Vector2 start, Vector2 end, int iter)
    {
        if (iter == 0)
        {
            DrawLine(colors, start, end, Color.white);
        }
        else
        {
            Vector2 dir = (end - start) / 3f;
            Vector2 p1 = start + dir;
            Vector2 p2 = end - dir;
            Vector2 p3 = RotatePoint(p1, end, 60);

            DrawKochLine(colors, start, p1, iter - 1);
            DrawKochLine(colors, p1, p3, iter - 1);
            DrawKochLine(colors, p3, p2, iter - 1);
            DrawKochLine(colors, p2, end, iter - 1);
        }
    }

    Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians) * (point.x - pivot.x) - Mathf.Sin(radians) * (point.y - pivot.y) + pivot.x;
        float y = Mathf.Sin(radians) * (point.x - pivot.x) + Mathf.Cos(radians) * (point.y - pivot.y) + pivot.y;
        return new Vector2(x, y);
    }

    void DrawLine(Color[] colors, Vector2 start, Vector2 end, Color color)
    {
        int startX = Mathf.RoundToInt(start.x);
        int startY = Mathf.RoundToInt(start.y);
        int endX = Mathf.RoundToInt(end.x);
        int endY = Mathf.RoundToInt(end.y);

        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);
        int sx = startX < endX ? 1 : -1;
        int sy = startY < endY ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (startX >= 0 && startX < textureWidth && startY >= 0 && startY < textureHeight)
            {
                colors[startY * textureWidth + startX] = color;
            }

            if (startX == endX && startY == endY) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                startX += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                startY += sy;
            }
        }
    }
}