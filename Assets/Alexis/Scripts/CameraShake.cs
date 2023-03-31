using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float amplitude = 0.1f;      // The amount of camera shake
    public float frequency = 1f;        // The frequency of camera shake
    public float smoothSpeed = 1f;      // The smoothness of camera movement
    public Vector3 startPosition;      // The starting position of the camera

    private float offset;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the camera's vertical offset based on time
        offset = Mathf.Sin(Time.time * frequency) * amplitude;

        // Move the camera up and down smoothly using Lerp
        Vector3 targetPosition = startPosition + Vector3.up * offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
