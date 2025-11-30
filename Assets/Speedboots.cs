using UnityEngine;

public class Speedboots : MonoBehaviour
{
    [Tooltip("Multiplier applied to player's walk and sprint speeds.")]
    public float speedMultiplier = 2f;

    [Tooltip("Duration of the speed boost in seconds.")]
    public float duration = 10f;

    [Tooltip("If true, the Speedboots object will be disabled after one use.")]
    public bool singleUse = true;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        var pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ApplySpeedBoost(speedMultiplier, duration);
            if (singleUse)
            {
                used = true;
                // Disable visuals and collider so it cannot be reused
                var col = GetComponent<Collider>();
                if (col != null) col.enabled = false;

                foreach (var r in GetComponentsInChildren<Renderer>())
                    r.enabled = false;
            }
        }
    }
}
