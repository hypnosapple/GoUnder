using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailSystem : MonoBehaviour
{
    private int EmialTotal;
    public GameObject MidColumnParent, RightColumnParent;
    public GameObject PrefabEmailMiddle, PrefabEmailRight;
    public GameObject ScrollContent;

    public MailObject newMail1, newMail2;

    public MailObject[] InBoxMails;

    private void Awake()
    {
        for (int i = 0; i < InBoxMails.Length; i++)
        {
            if(InBoxMails[i] != null)
            AddAnotherMailAtTheEnd(InBoxMails[i]);
        }
        Invoke("SetupTheFormate", 0.01f);
        
    }
    private void Start()
    {
        //this.transform.parent.gameObject.SetActive(false);
    }


    public void SetupTheFormate()
    {
        EmialTotal = MidColumnParent.transform.childCount;
        //plus length to the scroll
        ScrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(144, EmialTotal * 50 + (EmialTotal - 1) * 10);
        for (int i = 0; i < RightColumnParent.transform.childCount; i++)
        {
            //float tempHeight = RightColumnParent.transform.GetChild(i).
                //transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().rect.height + 95;
            //RightColumnParent.transform.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(355, tempHeight);
            RightColumnParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void AddAnotherMailAtTheEnd(MailObject newMail)
    {
        GameObject tempMail = Instantiate(PrefabEmailMiddle, MidColumnParent.transform);
        //set all the info
        tempMail.transform.GetChild(0).GetComponent<Text>().text = newMail.PersonName;
        tempMail.transform.GetChild(1).GetComponent<Text>().text = newMail.EmailTitle;
        tempMail.transform.GetChild(2).GetComponent<Text>().text = newMail.EmailContent;
        //plus one to the tempindex
        EmialTotal = MidColumnParent.transform.childCount;
        tempMail.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (EmialTotal - 1) * -60);

        //set the right part of the mail
        GameObject tempMailContent = Instantiate(PrefabEmailRight, RightColumnParent.transform);
        //set the info
        tempMailContent.transform.GetChild(2).GetComponent<Text>().text = newMail.EmailTitle; ;
        tempMailContent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>().color = newMail.IconColor;
        tempMailContent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RawImage>().texture = newMail.IconSprite;
        tempMailContent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = newMail.PersonName; ;
        tempMailContent.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = newMail.EmailAddress;
        tempMailContent.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = newMail.EmailContent;
        //plus the length of the scroll
        //float tempHeight = tempMailContent.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().rect.height + 95;
        //tempMailContent.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(355, tempHeight);

        tempMail.GetComponent<EmailOpen>().MyEmailContent = tempMailContent;
        tempMail.GetComponent<EmailOpen>().EmailContentParent = RightColumnParent;
        //tempMailContent.SetActive(false);
        Debug.Log(newMail.PersonName);
    }

    public void SetAllMailToRightSize()
    {
        //Debug.Log(RightColumnParent.transform.GetChild(RightColumnParent.transform.childCount - 1).
        //    transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().rect);

        for (int i = 0; i < RightColumnParent.transform.childCount; i++)
        {
            float tempHeight = RightColumnParent.transform.GetChild(i).
                transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().rect.height + 95;
            RightColumnParent.transform.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(355, tempHeight);
        }
    }

    public void AddNewMail()
    {
        //for (int i = 0; i < MidColumnParent.transform.childCount; i++)
        //{
        //    Destroy(MidColumnParent.transform.GetChild(0).gameObject);
        //}

        InBoxMails[1] = newMail1;
        InBoxMails[2] = newMail2;
        AddAnotherMailAtTheEnd(InBoxMails[1]);
        AddAnotherMailAtTheEnd(InBoxMails[2]);
        Invoke("SetupTheFormate", 0.01f);
    }
}
