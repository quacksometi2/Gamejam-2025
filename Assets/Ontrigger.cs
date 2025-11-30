using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject boss;
    public Transform spawnPoint;
    public bool spawnOnce = true;
    private bool hasSpawned = false;

    // Tilføj en AudioSource reference
    public AudioSource audioSource;
    public AudioClip bossMusic;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player") && !hasSpawned)
        {
            boss.SetActive(true);

            // Spil musik
            if (audioSource != null && bossMusic != null)
            {
                audioSource.clip = bossMusic;
                audioSource.Play();
            }

            if (spawnOnce)
            {
                hasSpawned = true;
            }
        }
    }
}
