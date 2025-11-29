using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.hasDoubleJump = true;
            Destroy(gameObject); // eller gameObject.SetActive(false);
        }
    }
}
