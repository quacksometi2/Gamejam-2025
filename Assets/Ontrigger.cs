using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;     // Drag din kube prefab ind i Inspector
    public Transform spawnPoint;      // Hvor kuben skal dukke op

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Sørg for at din spiller har tag "Player"
        {
            Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
