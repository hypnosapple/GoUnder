using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSound : MonoBehaviour
{
    public Slider slider_music;
    public Slider slider_sound;
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    private float backgroundFloat;
    private float soundEffectsFloat;
    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            backgroundFloat = 1f;
            soundEffectsFloat = 1f;
            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
            slider_music.value = backgroundFloat;
            slider_sound.value = soundEffectsFloat;
            backgroundAudio.volume = slider_music.value;
            for (int i = 0; i < soundEffectsAudio.Length; i++)
            {
                soundEffectsAudio[i].volume = slider_sound.value;
            }
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            slider_music.value = backgroundFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            slider_sound.value = soundEffectsFloat;
            backgroundAudio.volume = slider_music.value;
            for (int i = 0; i < soundEffectsAudio.Length; i++)
            {
                soundEffectsAudio[i].volume = slider_sound.value;
            }
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, slider_music.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, slider_sound.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        backgroundAudio.volume = slider_music.value;
        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = slider_sound.value;
        }
    }
}
