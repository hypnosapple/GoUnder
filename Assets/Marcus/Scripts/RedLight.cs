using UnityEngine;
using UnityEngine.Experimental.Rendering;

using System.Collections;

public class RedLight : MonoBehaviour
{
    public VLB.VolumetricLightBeamHD flickeringLight;
    private float initialIntensity;

    void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            flickeringLight.intensity = Random.Range(0f, .09f);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        }
    }
}

