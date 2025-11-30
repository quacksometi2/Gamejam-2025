using UnityEngine;
using UnityEngine.UI; // Bruges til UI.Text

public class DoubleJumpPaetices : MonoBehaviour
{
    [Header("Pickup Settings")]
    public GameObject pickupParticles;
    public AudioClip pickupSound;
    public float soundVolume = 1f;

    [Header("UI Settings")]
    public Text pickupText;            // Drag dit Text-element ind her
    public float displayDuration = 2f; // Hvor længe teksten skal vises

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

            // Aktiver dit eksisterende Text-objekt
            if (pickupText != null)
            {
                pickupText.gameObject.SetActive(true);
                Invoke(nameof(HideText), displayDuration);
            }

            // Fjern cuben
            Destroy(gameObject);
        }
    }

    private void HideText()
    {
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }
    }
}
