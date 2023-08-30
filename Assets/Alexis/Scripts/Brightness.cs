using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Brightness : MonoBehaviour
{
    public Slider slider; // Reference to the UI slider
    public Volume globalVolume; // Reference to the Volume component

    private const string FirstPlay = "FirstPlay";
    private const string ExposurePref = "ExposurePref";
    private int firstPlayInt;
    private ColorAdjustments colorAdjustments;
    private float exposureFloat;

    void Start()
    {
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
            if (firstPlayInt == 0) // If the game is played for the first time
            {
                exposureFloat = 0.15f; // Default exposure
                PlayerPrefs.SetFloat(ExposurePref, exposureFloat);
                PlayerPrefs.SetInt(FirstPlay, -1); // Mark that the game isn't played for the first time anymore
                slider.value = exposureFloat;
                UpdateExposure(); // Ensure the exposure gets set
            }
            else
            {
                exposureFloat = PlayerPrefs.GetFloat(ExposurePref);
                slider.value = exposureFloat;
                UpdateExposure(); // Ensure the exposure gets set
            }
        }
    }

    public void SaveExposureSettings()
    {
        // Save the exposure setting
        PlayerPrefs.SetFloat(ExposurePref, slider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        // When the application loses focus, save the exposure setting
        if (!inFocus)
        {
            SaveExposureSettings();
        }
    }

    public void UpdateExposure()
    {
        if (colorAdjustments)
        {
            colorAdjustments.postExposure.value = slider.value;
        }
    }
}
