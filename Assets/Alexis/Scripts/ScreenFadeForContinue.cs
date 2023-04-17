using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFadeForContinue : MonoBehaviour
{
    public Button button; // The button that triggers the screen fade
    public float fadeTime = 1.0f; // The time it takes to fade to black
    public Image fadeImage; // The image used for fading
    private Color targetColor = Color.black; // The color to fade to
    private Color initialColor; // The color to fade from
    private bool fading = false; // Flag to indicate whether the screen is currently fading

    void Start()
    {
        initialColor = fadeImage.color; // Store the initial color of the fade image
        button.onClick.AddListener(OnClick); // Register the OnClick method to be called when the button is clicked
    }

    void Update()
    {
        if (fading)
        {
            float t = Mathf.Clamp01(Time.time / fadeTime); // Calculate the current fade progress as a value between 0 and 1
            fadeImage.color = Color.Lerp(initialColor, targetColor, t); // Set the color of the fade image based on the current fade progress

            // Stop fading once the target color is reached
            if (fadeImage.color == targetColor)
            {
                fading = false;
            }
        }
        if (fadeImage.color == targetColor)
        {
            SceneManager.LoadScene(0); // Load the first scene
        }
    }

    void OnClick()
    {
        if (!fading)
        {
            StartFade(); // Start fading the screen to black
        }

    }

    public void StartFade()
    {
        fading = true; // Start fading the screen to black
    }
}
