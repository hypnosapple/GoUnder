using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFunction : MonoBehaviour
{

    public GameObject[] ThingsToStayClosed;
    public GameObject ObjectToOpen;
    public void EnableFunctionMulti()
    {
        ObjectToOpen.SetActive(true);
        for (int i = 0; i < ThingsToStayClosed.Length; i++)
        {
            ThingsToStayClosed[i].SetActive(false);
        }
    }
}
