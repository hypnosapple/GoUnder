using UnityEngine;

public class ImageSlideAway : MonoBehaviour
{
    public float speed = 100f;              // The speed of the image movement
    public float distance = 1000f;          // The distance the image should move
    public float delay = 1f;                // The delay before the images start moving

    public GameObject[] objectsToMove;      // The objects to move

    private Vector3[] originalPositions;    // The original positions of the objects
    private bool isMoving;                  // Whether the objects are currently moving

    public float cameraSpeed = 1.0f;

    void Start()
    {
        // Record the original positions of the objects
        originalPositions = new Vector3[objectsToMove.Length];
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            originalPositions[i] = objectsToMove[i].transform.localPosition;
        }

        isMoving = false;
    }

    void Update()
    {
        // Move each object away from the screen
        if (isMoving)
        {
            for (int i = 0; i < objectsToMove.Length; i++)
            {
                Vector3 targetPosition = originalPositions[i] - Vector3.right * distance;
                objectsToMove[i].transform.localPosition = Vector3.MoveTowards(objectsToMove[i].transform.localPosition, targetPosition, Time.deltaTime * speed);
            }
        }
    }

    public void StartMoving()
    {
        // Wait for the delay before starting the movement
        if (!isMoving)
        {
            isMoving = true;
            Invoke("StopMoving", delay);
        }
    }

    void StopMoving()
    {
        isMoving = false;
    }

}