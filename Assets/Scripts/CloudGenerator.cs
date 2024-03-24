using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject cloudPrefab; // Префаб облака
    public float spawnIntensity = 1f; // Интенсивность спавна
    public float cloudSpeedMin = 1f; // Минимальная скорость облака
    public float cloudSpeedMax = 3f; // Максимальная скорость облака
    public float sizeMin = 1f; // Максимальная скорость облака
    public float sizeMax = 8f; // Максимальная скорость облака
    public Vector3 spawnAreaSize = new(10f, 5f, 15f); // Размер области спавна
    public Vector3 offset = new(25f, 5f, 10f);

    private float nextSpawnTime = 0f;
    private GameObject parent;

    private void Start()
    {
        parent = new("Clouds");
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCloud();
            nextSpawnTime = Time.time + 1f / spawnIntensity;
        }
    }

    private void SpawnCloud()
    {
        var spawnPosition = new Vector3(
            offset.x, // + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            offset.y + Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
            offset.z + Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
        );

        var newCloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);

        var cloudSpeed = Random.Range(cloudSpeedMin, cloudSpeedMax);
        var moveDirection = Vector3.right * cloudSpeed;
        var scale = Random.Range(sizeMin, sizeMax);
        newCloud.transform.localScale = new(scale, scale, scale);
        newCloud.transform.parent = parent.transform;

        Destroy(newCloud, spawnAreaSize.x / cloudSpeed); // Удаляем облако, когда оно выходит за пределы спавна

        StartCoroutine(MoveCloud(newCloud, moveDirection));
    }

    private IEnumerator<GameObject> MoveCloud(GameObject cloud, Vector3 moveDirection)
    {
        var startTime = Time.time;
        while (Time.time - startTime < spawnAreaSize.x / moveDirection.x && cloud != null)
        {
            cloud.transform.Translate(moveDirection * Time.deltaTime);
            yield return null;
        }

        Destroy(cloud);
    }
}