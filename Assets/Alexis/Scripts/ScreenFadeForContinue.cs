using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenFadeForContinue : MonoBehaviour
{
    public Button button; // The button that triggers the screen fade
    public float fadeTime = 1.0f; // The time it takes to fade to black
    public Image fadeImage; // The image used for fading
    private Color targetColor = Color.black; // The color to fade to
    private Color initialColor; // The color to fade from
    private bool fading = false; // Flag to indicate whether the screen is currently fading

    public AudioSource[] audioSources;
    public AudioSource buttonAudioSource;

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
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        string sceneName;

        if (PlayerPrefs.HasKey("isInFloor2"))
        {
            sceneName = SceneManager.GetSceneByBuildIndex(2).name;
        }
        else
        {
            sceneName = SceneManager.GetSceneByBuildIndex(0).name;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            float t = Mathf.Clamp01(Time.time / fadeTime);
            fadeImage.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        // After the main scene has been loaded, you can unload the menu scene if you want
        //SceneManager.UnloadSceneAsync("MenuScene"); // Replace "MenuScene" with the name of your menu scene
    }
}

