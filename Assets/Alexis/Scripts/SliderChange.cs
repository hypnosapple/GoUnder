using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour
{
    public Slider slider;
    float fillTime = 0f;
    public CheckInput checkInput;
    public AudioSource loadingSound;

    public void Update()
    {
        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, fillTime);
        fillTime += 0.375f * Time.deltaTime;
        if (slider.value == slider.maxValue)
        {
            loadingSound.enabled = false;
            checkInput.ShowNoResponse();
        }
    }
    
}
