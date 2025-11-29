using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    [Header("Rotation Settings")]
    public Axis rotateAround = Axis.Y;
    public float degreesPerSecond = 90f;
    public AnimationCurve speedOverTime = AnimationCurve.Linear(0, 1, 1, 1);

    private float curveDuration;
    private float elapsed;

    void Start()
    {
        // Cache curve duration for looping; default to 1 second if no keys are present.
        curveDuration = speedOverTime.length > 0 ? speedOverTime.keys[speedOverTime.length - 1].time : 1f;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float sampleTime = curveDuration > 0 ? elapsed % curveDuration : elapsed;
        float speedFactor = speedOverTime.Evaluate(sampleTime);

        Vector3 axis = rotateAround switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.up
        };

        transform.Rotate(axis, degreesPerSecond * speedFactor * Time.deltaTime, Space.Self);
    }
}
