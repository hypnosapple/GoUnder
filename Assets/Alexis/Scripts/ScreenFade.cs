using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public Button button; // The button that triggers the screen fade
    public float fadeTime = 1.0f; // The time it takes to fade to black
    public Image fadeImage; // The image used for fading
    private Color targetColor = Color.black; // The color to fade to
    private Color initialColor; // The color to fade from
    private bool fading = false; // Flag to indicate whether the screen is currently fading

    public AudioSource[] audioSources;
    public AudioSource buttonAudioSource;

    private bool loaded = false;

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

            foreach(AudioSource audioSource in audioSources)
            {
                audioSource.volume = Mathf.Lerp(1, 0, t);
            }
            // Stop fading once the target color is reached
            if (fadeImage.color == targetColor)
            {
                fading = false;
            }

        }
        if (fadeImage.color == targetColor && !loaded)
        {
            PlayerPrefs.DeleteAll();
            loaded = true;
            StartCoroutine(LoadSceneF3()); // Load the first scene
        }
    }

    IEnumerator LoadSceneF3()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainSceneF3");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }


    }

    void OnClick()
    {
        if (!fading)
        {
            PlayButtonAudio();
            StartFade(); // Start fading the screen to black
        }

    }

    public void PlayButtonAudio()
    {
        buttonAudioSource.Play();
    }

    public void StartFade()
    {
        fading = true; // Start fading the screen to black
    }
}
