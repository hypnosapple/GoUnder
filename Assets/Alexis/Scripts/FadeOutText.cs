using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutText : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float slideInDistance = 1200f; // The distance to slide in from the left
    public float slideInSpeed = 5000f; // The speed at which to slide in
    private Vector3[] originalPositions;
    public float moveSpeed = 5000f;
    private bool keyPressed = false;

    public float fadeDuration = 0.3f; // The duration of the fade-out effect
    public Image imageComponent;    // Reference to the Image component on this GameObject
    public Image DB;
    private bool shouldFadeOut = false; // Whether or not the image should start fading out
    private float fadeTimer = 0.0f;  // Timer for the fade-out effect
    public GameObject midBlack;
    public GameObject aroundBlack;

    void Start()
    {
        originalPositions = new Vector3[gameObjects.Length];
        for(int i = 0; i < gameObjects.Length; i++)
        {
            originalPositions[i] = gameObjects[i].transform.position;
            gameObjects[i].transform.position -= new Vector3(slideInDistance, 0, 0);
        }
    }

    void Update()
    {
        // Check if the user has pressed any key
        if (!keyPressed && Input.anyKeyDown)
        {
            shouldFadeOut = true;
            keyPressed = true;
            midBlack.SetActive(false);
            aroundBlack.SetActive(false);
        }

        // Gradually decrease the alpha value of the image if the user has pressed any key

        if (keyPressed && shouldFadeOut)
        {
            Time.timeScale = 1;
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
            fadeTimer += Time.deltaTime;
            float progress = fadeTimer / fadeDuration;
            Color imageColor = imageComponent.color;
            Color imageColorDB = DB.color;
            imageColor.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageColorDB.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageComponent.color = imageColor;
            DB.color = imageColorDB;
            // Destroy the GameObject when the image is fully transparent
            if (imageColor.a <= 0.0f || imageColorDB.a <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
