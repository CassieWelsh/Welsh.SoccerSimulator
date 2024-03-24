using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRevolutorController : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения камеры
    public float mouseSensitivity = 2f; // Чувствительность мыши

    private float _xRotation = 0f; // Угол вращения по вертикали

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // Перемещение камеры по нажатым клавишам WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * (moveSpeed * Time.deltaTime);
        transform.Translate(movement);

        // Вращение камеры с помощью мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // Ограничиваем угол, чтобы камера не перевернулась

        transform.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(_xRotation, transform.localEulerAngles.y, 0f);
    }
}