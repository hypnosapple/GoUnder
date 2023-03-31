using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Button button; // The button that triggers the camera movement
    public float moveSpeed = 1.0f; // The speed at which to move the camera forward
    public float moveDistance = 10.0f; // The distance to move the camera forward
    private bool moving = false; // Flag to indicate whether the camera is currently moving
    private Vector3 startPosition; // The camera's starting position

    void Start()
    {
        startPosition = Camera.main.transform.position; // Store the camera's starting position
        button.onClick.AddListener(OnClick); // Register the OnClick method to be called when the button is clicked
    }

    void Update()
    {
        if (moving)
        {
            Camera.main.transform.position += Camera.main.transform.forward * moveSpeed * Time.deltaTime; // Move the camera forward

            // Stop moving once the camera has moved the desired distance
            if (Camera.main.transform.position.z - startPosition.z >= moveDistance)
            {
                moving = false;
            }
        }
    }

    void OnClick()
    {
        if (!moving)
        {
            Invoke("StartMoving", 2.0f); // Call the StartMoving method after a delay of 2 seconds
        }
    }

    void StartMoving()
    {
        moving = true; // Start moving the camera forward
        startPosition = Camera.main.transform.position; // Store the camera's new starting position
    }
}
