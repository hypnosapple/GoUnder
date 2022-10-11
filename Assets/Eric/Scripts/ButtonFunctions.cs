using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public void ExitButton(GameObject ObjectToClose)
    {
        ObjectToClose.SetActive(false);
    }

    public void EnableFunction(GameObject ObjectToClose)
    {
        ObjectToClose.SetActive(true);
    }

    public void EnableFunctionMulti(GameObject ObjectToOpen, GameObject ObjectToClose, GameObject ObjectToClose2)
    {
        ObjectToOpen.SetActive(true);
        ObjectToClose.SetActive(false);
        ObjectToClose2.SetActive(false);
    }
}
