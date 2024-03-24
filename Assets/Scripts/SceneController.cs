using System;
using System.Collections.Generic;
using System.Linq;
using Constructors;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour
{
    public GameObject GatePrefab;
    public GameObject ScoreboardPrefab;
    private List<GameObject> _objects;

    private Transform _cameraTarget;
    public GameObject rain;
    private GameObject _light;

    public float smoothSpeed = 0.125f; // Скорость интерполяции (задает, насколько быстро камера приближается к цели)
    public Vector3 offset; // Смещение камеры относительно цели
    public float rotationSpeed = 5.0f; // Скорость поворота камеры вокруг цели
    private Dictionary<CameraController.CameraState, GameObject> _cameraPositions;

    private void Start()
    {
        Application.targetFrameRate = 75;

        var objectsToDraw = GenerateSceneExtensions.GetObjects(GatePrefab, ScoreboardPrefab);
        _objects = new(objectsToDraw.Length);
        foreach (var obj in objectsToDraw)
            _objects.Add(obj.PrepareObject());

        var player = _objects.First(go => go.name == "Player");
        _cameraTarget = player.transform;

        var top = new GameObject
        {
            transform =
            {
                position = new(0, 37, 0),
                rotation = Quaternion.Euler(90, 0, 180)
            }
        };

        var alongside = new GameObject
        {
            transform =
            {
                position = new(0, 19, 40),
                rotation = Quaternion.Euler(25, 180, 0)
            }
        };

        CameraController.SetPositions(new()
        {
            [CameraController.CameraState.Player] = player,
            [CameraController.CameraState.Top] = top,
            [CameraController.CameraState.Alongside] = alongside
        });

        _light = GameObject.Find("Light");
    }

    private void Update()
    {
        WeatherController.RefreshWeather(rain, _light);
        if (Input.GetKeyDown(KeyCode.J))
            _cameraTarget = CameraController.NextCameraPosition();
    }

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        // Получаем текущее положение и вращение цели
        Vector3 targetPosition = _cameraTarget.position;
        Quaternion targetRotation = _cameraTarget.rotation;

        // Вычисляем смещение от цели с учетом справа-зади
        Vector3 rightOffset = _cameraTarget.right * offset.x;
        Vector3 upOffset = _cameraTarget.up * offset.y;
        Vector3 backOffset = -_cameraTarget.forward * offset.z;

        // Суммируем смещения
        Vector3 desiredPosition = targetPosition + rightOffset + backOffset + upOffset;

        // Интерполируем плавное перемещение к цели
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Поворачиваем камеру к цели
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}