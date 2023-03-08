using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    private bool afternoonChanged;
    private bool nightChanged;

    public Material afternoonSkybox;
    public Material nightSkybox;

    public Material afternoonWater;
    public Material nightWater;

    public GameObject sea;
    public GameObject tunnel3to2;
    public GameObject glass;

    void Start()
    {
        afternoonChanged = false;
        nightChanged = false;

    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ToAfternoon")
        {
            
            if (!afternoonChanged)
            {
                tunnel3to2.SetActive(true);
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
