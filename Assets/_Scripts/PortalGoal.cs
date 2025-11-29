using UnityEngine;

public class PortalGoal : MonoBehaviour
{
    private GameManager gameManager;
    private bool hasTriggered = false;

    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        if (gameManager != null)
        {
            gameManager.FinishedLevel();
        }
        else
        {
            Debug.LogWarning("GameManager not found when entering portal goal.");
        }
    }
}
