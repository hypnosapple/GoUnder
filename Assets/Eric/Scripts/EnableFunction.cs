using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableFunction : MonoBehaviour
{

    public GameObject[] ThingsToStayClosed;
    public GameObject ObjectToOpen;
    public RawImage Sent, Inbox, Draft;
    public Color Red;
    public GameObject TheOtherIcon;
    public void EnableFunctionMulti()
    {
        ObjectToOpen.SetActive(true);
        for (int i = 0; i < ThingsToStayClosed.Length; i++)
        {
            ThingsToStayClosed[i].SetActive(false);
        }
    }

    public void ColorChange()
    {
        Sent.color = Color.white;
        Draft.color = Color.white;
        Inbox.color = Color.white;
        this.GetComponent<RawImage>().color = Red;
    }

    public void TurnMeRed()
    {
        this.GetComponent<RawImage>().color = Red;
        TheOtherIcon.GetComponent<RawImage>().color = Color.white;
    }

}
