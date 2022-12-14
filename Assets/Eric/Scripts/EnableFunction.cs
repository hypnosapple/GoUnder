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

    public GameObject Sent2, Inbox2, Draft2;

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

    public void TurnMeRed(bool temp)
    {
        if (temp)
        {
            this.GetComponent<RawImage>().color = Red;
            TheOtherIcon.GetComponent<RawImage>().color = Color.white;
        }
        else
        {
            this.GetComponent<RawImage>().color = Color.white;
        }
    }

    public void MailAndInfoButton()
    {
        bool temp = ObjectToOpen.activeSelf;
        ObjectToOpen.SetActive(!temp);
        TurnMeRed(!temp);
        if (TheOtherIcon.GetComponent<EnableFunction>().ObjectToOpen.activeSelf)
        {
            TheOtherIcon.GetComponent<EnableFunction>().TurnMeOff();
        }
    }

    public void TurnMeWhite()
    {
            this.GetComponent<RawImage>().color = Color.white;
    }

    public void TurnMeOff()
    {
        bool temp = ObjectToOpen.activeSelf;
        ObjectToOpen.SetActive(!temp);
        TurnMeRed(!temp);
    }

    public void DraftEnable()
    {
        Sent2.gameObject.SetActive(false);
        Inbox2.gameObject.SetActive(false);
        Draft2.gameObject.SetActive(true);
    }

    public void SentEnable()
    {
        Sent2.gameObject.SetActive(true);
        Inbox2.gameObject.SetActive(false);
        Draft2.gameObject.SetActive(false);
    }

    public void Empty()
    {
        Draft2.gameObject.SetActive(true);
    }

}
