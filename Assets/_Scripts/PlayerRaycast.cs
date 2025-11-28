using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform rayOrigin;        // Sæt til Camera eller Player
    public float rayLength = 3f;
    public LayerMask hitLayers;        // Vælg hvilke layers raycast må ramme
    public string targetTag = "Interactable";

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || rayOrigin == null) return;

        if (keyboard.eKey.wasPressedThisFrame)
        {
            Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, hitLayers))
            {
                if (hit.collider.CompareTag(targetTag))
                {
                    hit.collider.GetComponent<ButtonInfo>().ChooseButton();
                }
            }
        }
    }

    // Optional editor gizmo
    void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + rayOrigin.forward * rayLength);
    }
}
