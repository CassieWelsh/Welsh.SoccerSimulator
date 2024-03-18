using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 8f; // Speed of movement
    public float rotationSpeed = 4f; // Speed of rotation

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation
                         | RigidbodyConstraints.FreezeRotationZ
                         | RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true; // Freezing rotation to prevent unwanted physics interactions
    }

    void Update()
    {
        // Movement
        float moveInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * (moveInput * moveSpeed);
        rb.velocity = new(moveDirection.x, rb.velocity.y, moveDirection.z);

        // Rotation
        float rotateInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * (rotateInput * rotationSpeed * 100 * Time.deltaTime));
    }
}