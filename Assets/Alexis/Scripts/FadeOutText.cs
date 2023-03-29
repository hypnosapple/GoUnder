using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutText : MonoBehaviour
{
    public float fadeDuration = 1.0f; // The duration of the fade-out effect
    public Image imageComponent;    // Reference to the Image component on this GameObject
    public Image DB;
    public Image R;
    public Image R2;
    private bool shouldFadeOut = false; // Whether or not the image should start fading out
    private float fadeTimer = 0.0f;  // Timer for the fade-out effect

    void Start()
    {

    }

    void Update()
    {
        // Check if the user has pressed any key
        if (Input.anyKeyDown)
        {
            shouldFadeOut = true;
        }

        // Gradually decrease the alpha value of the image if the user has pressed any key
        if (shouldFadeOut)
        {
            fadeTimer += Time.deltaTime;
            float progress = fadeTimer / fadeDuration;
            Color imageColor = imageComponent.color;
            Color imageColorDB = DB.color;
            Color imageColorR = R.color;
            Color imageColorR2 = R2.color;
            imageColor.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageColorDB.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageColorR.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageColorR2.a = Mathf.Lerp(1.0f, 0.0f, progress);
            imageComponent.color = imageColor;
            DB.color = imageColorDB;
            R.color = imageColorR;
            R2.color = imageColorR2;

            // Destroy the GameObject when the image is fully transparent
            if (imageColor.a <= 0.0f || imageColorDB.a <= 0.0f || imageColorR.a <= 0.0f || imageColorR2.a <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
