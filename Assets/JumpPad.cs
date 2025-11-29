using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Pad Settings")]
    [SerializeField] private float upForce = 12f;
    [SerializeField] private float forwardForce = 8f;

    [Header("Player Reference")]
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement == null) return;

        // nulstil Y-hastighed og tilføj boost
        Vector3 forward = player.transform.forward * forwardForce;
        Vector3 boost = new Vector3(forward.x, upForce, forward.z);

        movement.ApplyJumpPadBoost(boost);
    }
}
