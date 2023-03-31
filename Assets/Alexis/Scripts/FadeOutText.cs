using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutText : MonoBehaviour
{
    public float fadeDuration = 1.0f; // The duration of the fade-out effect
    public Image imageComponent;    // Reference to the Image component on this GameObject
    public Image DB;
    private bool shouldFadeOut = false; // Whether or not the image should start fading out
    private float fadeTimer = 0.0f;  // Timer for the fade-out effect
    public GameObject midBlack;
    public GameObject aroundBlack;

    void Start()
    {

    }

    void Update()
    {
        // Check if the user has pressed any key
        if (Input.anyKeyDown)
        {
            shouldFadeOut = true;
            midBlack.SetActive(false);
            aroundBlack.SetActive(false);
        }

        // Gradually decrease the alpha value of the image if the user has pressed any key
        if (shouldFadeOut)
        {
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
