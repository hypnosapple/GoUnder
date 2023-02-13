using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    private bool afternoonChanged;
    private bool nightChanged;

    public Material afternoonSkybox;
    public Material nightSkybox;

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
            Debug.Log("layer");
            if (!afternoonChanged)
            {
                Debug.Log("afternoon");
                RenderSettings.skybox = afternoonSkybox;
                afternoonChanged = true;
            }
        }
    }

    
}
