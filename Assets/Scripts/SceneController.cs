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

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float rotationSpeed = 5.0f;
    private Dictionary<CameraController.CameraState, GameObject> _cameraPositions;
    private GameObject _sphere;
    private GameObject _player;

    private void Start()
    {
        Application.targetFrameRate = 75;

        var objectsToDraw = GenerateSceneExtensions.GetObjects(GatePrefab, ScoreboardPrefab);
        _objects = new(objectsToDraw.Length);
        foreach (var obj in objectsToDraw)
            _objects.Add(obj.PrepareObject());

        _player = _objects.First(go => go.name == "Player");
        _cameraTarget = _player.transform;

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
            [CameraController.CameraState.Player] = _player,
            [CameraController.CameraState.Top] = top,
            [CameraController.CameraState.Alongside] = alongside
        });

        _light = GameObject.Find("Light");
        _sphere = _objects.First(go => go.name == "Sphere");
    }

    private void Update()
    {
        WeatherController.RefreshWeather(rain, _light);
        if (Input.GetKeyDown(KeyCode.J))
            _cameraTarget = CameraController.NextCameraPosition();

        if (Input.GetKey(KeyCode.U))
            _light.transform.Rotate(Vector3.up, 1f);

        if (Input.GetKeyDown(KeyCode.L))
        {
            var isPlayer = _player.GetComponent<MovementController>().enabled;
            if (isPlayer)
            {
                _player.GetComponent<MovementController>().enabled = false;
                _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                _sphere.GetComponent<MovementController>().enabled = true;
                _sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation
                                                                | RigidbodyConstraints.FreezeRotationZ
                                                                | RigidbodyConstraints.FreezePositionY;
                var rotation = _sphere.transform.rotation;
                rotation.x = rotation.z = 0f;
                _sphere.transform.rotation = rotation;
                var position = _sphere.transform.position;
                position.y = 1;
                _sphere.transform.position = position;

                _cameraTarget = _sphere.transform;
            }
            else
            {
                _sphere.GetComponent<MovementController>().enabled = false;
                _sphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                _player.GetComponent<MovementController>().enabled = true;
                _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation
                                                                | RigidbodyConstraints.FreezeRotationZ
                                                                | RigidbodyConstraints.FreezePositionY;

                var rotation = _player.transform.rotation;
                rotation.x = rotation.z = 0f;
                _player.transform.rotation = rotation;
                var position = _player.transform.position;
                position.y = 1;
                _player.transform.position = position;

                _cameraTarget = _player.transform;
            }
        }
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