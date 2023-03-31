using UnityEngine;

public class SlideInImages : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float slideInDistance = 5000f; // The distance to slide in from the left
    public float slideInSpeed = 100f; // The speed at which to slide in
    private Vector3[] originalPositions;
    public float moveSpeed = 10f;
    private bool keyPressed = false;

    void Start()
    {
        originalPositions = new Vector3[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            originalPositions[i] = gameObjects[i].transform.position;
            gameObjects[i].transform.position -= new Vector3(slideInDistance, 0, 0); // Start the objects offscreen to the left
        }
    }

    void Update()
    {
        if (!keyPressed && Input.anyKeyDown)
        {
            keyPressed = true;
        }

        if (keyPressed)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Vector3 targetPosition = originalPositions[i];
                if (gameObjects[i].transform.position.x < originalPositions[i].x) // If the object is not yet at its original position
                {
                    gameObjects[i].transform.position += new Vector3(slideInSpeed * Time.deltaTime, 0, 0); // Slide in from the left
                }
                else
                {
                    gameObjects[i].transform.position = Vector3.MoveTowards(gameObjects[i].transform.position, targetPosition, moveSpeed * Time.deltaTime); // Move back to the original position on the x-axis
                }
            }
        }
    }
}
