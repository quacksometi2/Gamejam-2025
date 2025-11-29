using UnityEngine;

public class DoubleJumpPaetices : MonoBehaviour
{
    [Header("Pickup Settings")]
    public GameObject pickupParticles;     // Træk dit particle prefab ind her
    public AudioClip pickupSound;          // Træk dit lydklip ind her
    public float soundVolume = 1f;         // Justér lydstyrke

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Aktiver double jump
            player.hasDoubleJump = true;

            // Spawn partikler
            if (pickupParticles != null)
            {
                Instantiate(pickupParticles, transform.position, Quaternion.Euler(-90, 0, 0));
            }

            // Spil lyd
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }

            // Fjern cuben
            Destroy(gameObject);
        }
    }
}

