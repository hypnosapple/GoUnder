using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CheckInput : MonoBehaviour
{
    public GameObject Open;
    public GameObject Error;
    public GameObject SignalSending;
    public GameObject NoResponse;
    public TMP_InputField IslandCode;
    public TMP_InputField CommunicationCode;
    public string correctIsland;
    public string correctCommunication;
    public bool allCorrect;

    public void CheckPassword()
    {
        string ReceivedString1 = IslandCode.text;
        string ReceivedString2 = CommunicationCode.text;
        if(ReceivedString1 == correctIsland && ReceivedString2 == correctCommunication)
        {
            Debug.Log("correct");
            IslandCode.text = string.Empty;
            CommunicationCode.text = string.Empty;
            Open.SetActive(false);
            SignalSending.SetActive(true);
            allCorrect = true;
        }
        else
        {
            Debug.Log("incorrect");
            Open.SetActive(false);
            Error.SetActive(true);
        }
    }

    public void ShowNoResponse()
    {
        SignalSending.SetActive(false);
        IslandCode.text = string.Empty;
        CommunicationCode.text = string.Empty;
        NoResponse.SetActive(true);
    }

    public void ShowOpen()
    {
        NoResponse.SetActive(false);
        Error.SetActive(false);
        IslandCode.text = string.Empty;
        CommunicationCode.text = string.Empty;
        Open.SetActive(true);
    }
}
