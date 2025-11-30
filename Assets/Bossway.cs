using UnityEngine;

public class CubePathFollower : MonoBehaviour
{
    public Transform[] pathPoints;   // Drag dine waypoints ind i Inspector
    public float speed = 5f;
    private int currentPoint = 0;

    void Update()
    {
        if (pathPoints.Length == 0) return;

        // Bevæg kuben mod det aktuelle punkt
        transform.position = Vector3.MoveTowards(
            transform.position,
            pathPoints[currentPoint].position,
            speed * Time.deltaTime
        );

        // Når vi er tæt nok på punktet, gå videre til det næste
        if (Vector3.Distance(transform.position, pathPoints[currentPoint].position) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= pathPoints.Length)
            {
                currentPoint = 0; // Loop tilbage til start
            }
        }
    }
}
