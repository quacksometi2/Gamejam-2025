using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("AudioSource that contains the clip to play when the player hits the lava.")]
    public AudioSource lavaAudio;

    [Header("Disable Components")]
    [Tooltip("Optional: drag any Behaviour components here that should be disabled when the player touches the lava.")]
    public Behaviour[] behavioursToDisable;

    [Header("Restart")]
    [Tooltip("Seconds to wait before restarting the current scene.")]
    public float restartDelay = 2f;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(HandlePlayer(other.gameObject));
    }

    IEnumerator HandlePlayer(GameObject player)
    {
        // Play lava sound (AudioSource should have a clip assigned in inspector)
        if (lavaAudio != null)
        {
            lavaAudio.Play();
        }

        // Disable any behaviours explicitly assigned in the inspector (only if they belong to player or its children)
        if (behavioursToDisable != null)
        {
            foreach (var b in behavioursToDisable)
            {
                if (b == null) continue;
                if (b.gameObject == player || b.gameObject.transform.IsChildOf(player.transform))
                {
                    b.enabled = false;
                }
            }
        }

        // Also try to disable commonly used player scripts if they exist
        var pm = player.GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        var pf = player.GetComponent<PlayerFootsteps>();
        if (pf != null) pf.enabled = false;

        var cam = player.GetComponentInChildren<CameraFollow>();
        if (cam != null) cam.enabled = false;

        // Wait then reload the current scene
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
