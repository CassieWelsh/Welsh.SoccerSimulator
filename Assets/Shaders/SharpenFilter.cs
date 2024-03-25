using UnityEngine;

public class SharpenFilter : MonoBehaviour
{
    public float sharpness = 1.0f;

    private Material material;
    public Shader shader;

    void Start()
    {
        if (shader == null)
        {
            Debug.LogError("Shader is not assigned in the SharpenFilter script!");
            enabled = false;
            return;
        }

        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Устанавливаем размеры текстуры
        material.SetVector("_TexelSize", new Vector4(1.0f / source.width, 1.0f / source.height, 0, 0));

        material.SetFloat("_Sharpness", sharpness);

        Graphics.Blit(source, destination, material);
    }
}