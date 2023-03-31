using UnityEngine;
using UnityEngine.UI;

public class ImageSlideAway : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 2f;

    private Vector2[] initialPositions;
    private Vector2[] targetPositions;
    private bool isMoving = false;

    public Button[] buttons;

    void Start()
    {
        initialPositions = new Vector2[buttons.Length];
        targetPositions = new Vector2[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            initialPositions[i] = buttons[i].GetComponent<RectTransform>().anchoredPosition;
            targetPositions[i] = initialPositions[i] + new Vector2(-moveDistance, 0f);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            float t = moveSpeed * Time.deltaTime;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(buttons[i].GetComponent<RectTransform>().anchoredPosition, targetPositions[i], t);
                if (Vector2.Distance(buttons[i].GetComponent<RectTransform>().anchoredPosition, targetPositions[i]) < 0.01f)
                {
                    buttons[i].GetComponent<RectTransform>().anchoredPosition = targetPositions[i];
                }
            }
            if (Vector2.Distance(buttons[0].GetComponent<RectTransform>().anchoredPosition, targetPositions[0]) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void MoveButtons()
    {
        isMoving = true;
        for (int i = 0; i < buttons.Length; i++)
        {
            targetPositions[i] = initialPositions[i] + new Vector2(-moveDistance, 0f);
        }
    }
}
