using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // Hastighed for bevægelse
    public float lookSpeed = 2f;      // Hastighed for rotation

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // Bevægelse med WASD eller piletaster
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);

        // Rotation med mus
        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Begræns lodret rotation

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
    }
}
