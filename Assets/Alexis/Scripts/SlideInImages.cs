using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInImages : MonoBehaviour
{
    public float slideDuration = 1.0f;      // The duration of the slide-in effect
    public float slideDelay = 0.5f;        // The delay between each image's slide-in effect
    public float slideDistance = 500.0f;   // The distance that each image should slide in from the left
    public Image[] images;                 // The images to slide in

    private bool isSliding = false;        // Whether or not the images are currently sliding in

    private Vector2[] originalPositions;   // The original positions of each image

    void Start()
    {
        // Save the original positions of each image
        originalPositions = new Vector2[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            originalPositions[i] = images[i].GetComponent<RectTransform>().anchoredPosition;
        }

        // Move the images offscreen to the left
        foreach (Image image in images)
        {
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-slideDistance, 0.0f);
        }

        // Wait for the user to press any key
        StartCoroutine(WaitForKeyPressCoroutine());
    }

    IEnumerator WaitForKeyPressCoroutine()
    {
        // Wait for the user to press any key
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        // Start the slide-in effect after a delay
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(SlideInImagesCoroutine());
    }

    IEnumerator SlideInImagesCoroutine()
    {
        // Slide in all images simultaneously
        float timer = 0.0f;
        while (timer < slideDuration)
        {
            for (int i = 0; i < images.Length; i++)
            {
                // Calculate the target position of the image
                RectTransform rectTransform = images[i].GetComponent<RectTransform>();
                Vector2 startPosition = new Vector2(-slideDistance, 0.0f);
                Vector2 targetPosition = originalPositions[i];

                // Slide the image in over a specified duration
                float progress = timer / slideDuration;
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, progress);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Set the image positions to their final destinations
        for (int i = 0; i < images.Length; i++)
        {
            images[i].GetComponent<RectTransform>().anchoredPosition = originalPositions[i];
        }

        // Mark the slide-in effect as complete
        isSliding = true;
    }
}
