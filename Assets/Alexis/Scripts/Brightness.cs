using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider slider;
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BrightnessPref = "BrightnessPref";
    private int firstPlayInt;
    private float brightnessFloat;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            brightnessFloat = 0.5f;
            PlayerPrefs.SetFloat(BrightnessPref, brightnessFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
            slider.value = brightnessFloat;
            Screen.brightness = slider.value;
        }
        else
        {
            brightnessFloat = PlayerPrefs.GetFloat(BrightnessPref);
            slider.value = brightnessFloat;
            Screen.brightness = slider.value;
        }
    }

    public void SaveBrightnessSettings()
    {
        PlayerPrefs.SetFloat(BrightnessPref, slider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveBrightnessSettings();
        }
    }

    public void UpdateBrightness()
    {
        Screen.brightness = slider.value;
    }
}
