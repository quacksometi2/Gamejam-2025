using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;
    public InputActionReference lookAction;

    [Header("Settings")]
    public float sensitivity = 2f;
    public float minSensitivity = 0.1f;
    public float maxTilt = 12f;        // kan justeres fra menu
    public float tiltSpeed = 6f;

    private float xRotation = 0f;
    private float targetTilt = 0f;
    private float tiltVelocity;

    void OnEnable() { if (lookAction != null) lookAction.action.Enable(); }
    void OnDisable() { if (lookAction != null) lookAction.action.Disable(); }

    void Start()
    {
        // Hent gemte værdier
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
        maxTilt = PlayerPrefs.GetFloat("TiltAmount", maxTilt);

        if (sensitivity < minSensitivity) sensitivity = minSensitivity;
    }

    void Update()
    {
        Vector2 mouseDelta = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;
        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        // Vertical (camera pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal (player yaw)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Smooth tilt (roll)
        float currentZ = transform.localEulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;

        float newZ = Mathf.SmoothDampAngle(
            currentZ,
            targetTilt,
            ref tiltVelocity,
            1f / Mathf.Max(0.0001f, tiltSpeed)
        );

        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            newZ
        );
    }

    // 👉 Kaldt fra PlayerMovement
    public void SetTilt(bool wallOnLeft, bool wallOnRight)
    {
        if (wallOnLeft) targetTilt = maxTilt;        // tilt modsat venstre væg
        else if (wallOnRight) targetTilt = -maxTilt; // tilt modsat højre væg
        else targetTilt = 0f;
    }

    // 👉 Kald fra menu-slider
    public void ApplyTiltAmount(float value)
    {
        maxTilt = value;
        PlayerPrefs.SetFloat("TiltAmount", value);
        PlayerPrefs.Save();
    }

    public void ApplySensitivity(float value)
    {
        if (value < minSensitivity) value = minSensitivity;
        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }
}
