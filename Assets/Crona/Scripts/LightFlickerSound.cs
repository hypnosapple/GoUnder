using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerSound : MonoBehaviour
{
    public AudioClip buzzAudio;
    public float playAudioBelowIntensity;
    AudioSource usedAudioSource;
    public Light myLight;

    private bool performPauses;
    private bool audioOn;

    

    private void OnEnable()
    {
        myLight = GetComponent<Light>();
        usedAudioSource = GetComponent<AudioSource>();
        usedAudioSource.loop = true;
        usedAudioSource.clip = buzzAudio;
        StartCoroutine(CheckIntensity());
    }

    IEnumerator CheckIntensity()
    {
        while (!performPauses)
        {
            
            if (myLight.intensity < playAudioBelowIntensity && !audioOn)
            {
                usedAudioSource.Play();
                audioOn = true;
            }
            else if (myLight.intensity >= playAudioBelowIntensity && audioOn)
            {
                usedAudioSource.Stop();
                audioOn = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
