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
                
                RenderSettings.skybox = afternoonSkybox;
                sea.GetComponent<MeshRenderer>().material = afternoonWater;
                afternoonChanged = true;
            }
        }
    }

    
}
