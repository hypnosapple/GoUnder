using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MouseLook : MonoBehaviour
{
    public Slider slider_x;
    public Slider slider_y;
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string xAxis = "xAxis";
    private static readonly string yAxis = "yAxis";
    private int firstPlayInt;
    private float xAxisFloat = 500f;
    private float yAxisFloat = 500f;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            xAxisFloat = 500f;
            yAxisFloat = 500f;
            PlayerPrefs.SetFloat(xAxis, xAxisFloat);
            PlayerPrefs.SetFloat(yAxis, yAxisFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
            slider_x.value = xAxisFloat;
            slider_y.value = yAxisFloat;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = xAxisFloat;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = yAxisFloat;
        }
        else
        {
            xAxisFloat = PlayerPrefs.GetFloat(xAxis);
            slider_x.value = xAxisFloat;
            yAxisFloat = PlayerPrefs.GetFloat(yAxis);
            slider_y.value = yAxisFloat;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = xAxisFloat;
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = yAxisFloat;
        }
        
    }

    public void SaveControlSettings()
    {
        PlayerPrefs.SetFloat(xAxis, slider_x.value);
        PlayerPrefs.SetFloat(yAxis, slider_y.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveControlSettings();
        }
    }

    public void UpdateControl()
    {
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = xAxisFloat;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = yAxisFloat;
    }
}
