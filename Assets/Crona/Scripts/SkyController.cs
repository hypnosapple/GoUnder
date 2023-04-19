 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    private Material Skybox;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Skybox = RenderSettings.skybox;
    }

    // Update is called once per frame
    void Update()
    {
        Skybox.SetFloat("_Rotation", Skybox.GetFloat("_Rotation") + Time.deltaTime * speed);
    }
}
