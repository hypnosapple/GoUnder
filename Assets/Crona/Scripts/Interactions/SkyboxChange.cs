using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    public static SkyboxChange Instance;

    private bool afternoonChanged;
    private bool nightChanged;

    [Header("Materials")]
    public Material afternoonSkybox;
    public Material nightSkybox;

    public Material afternoonWater;
    public Material nightWater;

    [Header("Object References")]
    public GameObject sea;
    public GameObject tunnel3to2;
    public GameObject glass;

    public AudioSource tunnelTone;

    void Start()
    {
        Instance = this;

        afternoonChanged = false;
        nightChanged = false;

    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ToAfternoon")
        {
            
            if (!afternoonChanged)
            {
                tunnel3to2.SetActive(true);
                tunnelTone.Play();
                glass.SetActive(false);
                RenderSettings.skybox = afternoonSkybox;
                sea.GetComponent<MeshRenderer>().material = afternoonWater;
                afternoonChanged = true;
            }
        }

        else if (other.gameObject.tag == "ToNight")
        {

            if (!nightChanged)
            {
                
                RenderSettings.skybox = nightSkybox;
                sea.GetComponent<MeshRenderer>().material = nightWater;
                nightChanged = true;
            }
        }
    }

    
}
