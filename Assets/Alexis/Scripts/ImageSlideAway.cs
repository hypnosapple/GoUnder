using UnityEngine;
using UnityEngine.UI;

public class ImageSlideAway : MonoBehaviour
{
    public Button moveButton;
    public GameObject[] objectsToMove;
    public float moveDistance = 50.0f;
    public float moveSpeed = 10.0f;

    private Vector3[] originalPositions;

    private void Start()
    {
        // store the original positions of the objects
        originalPositions = new Vector3[objectsToMove.Length];
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            originalPositions[i] = objectsToMove[i].transform.localPosition;
        }

        // register event listener for button click event
        moveButton.onClick.AddListener(MoveObjectsLeftOnClick);
    }

    private void MoveObjectsLeftOnClick()
    {
        // move each object towards the left by the specified distance
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            // calculate the target position to move the object towards
            Vector3 targetPosition = originalPositions[i] - Vector3.right * moveDistance;

            // move the object towards the target position using a smooth motion
            objectsToMove[i].transform.localPosition = Vector3.MoveTowards(objectsToMove[i].transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
