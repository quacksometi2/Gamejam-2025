using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;               // Den del af spilleren der skal rotere horisontalt
    public InputActionReference lookAction;    // Input Action til musens bevægelse

    [Header("Settings")]
    public float sensitivity = 2f;             // Standard følsomhed (bruges hvis intet er gemt)
    public float minSensitivity = 0.1f;        // Minimum følsomhed (forhindrer 0)

    private float xRotation = 0f;

    void OnEnable() => lookAction.action.Enable();
    void OnDisable() => lookAction.action.Disable();

    void Start()
    {
        // Hent gemt følsomhed fra PlayerPrefs (default = 2f)
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);

        // Clamp så den aldrig bliver 0 eller negativ
        if (sensitivity < minSensitivity)
            sensitivity = minSensitivity;
    }

    void Update()
    {
        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        // Vertikal rotation (kameraet)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horisontal rotation (spillerens krop)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    // Bruges hvis du vil ændre følsomheden live fra options-menuen
    public void ApplySensitivity(float value)
    {
        if (value < minSensitivity)
            value = minSensitivity;

        sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }
}
