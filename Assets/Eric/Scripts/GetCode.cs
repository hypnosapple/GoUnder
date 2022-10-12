using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GetCode : MonoBehaviour
{
    public Text TextToCheck1, TextToCheck2;
    public string TextAnswer1, TextAnswer2;
    public GameObject GetCodeButton;

    public void GetCodeValueChange()
    {
        //Debug.Log(TextToCheck1.text + " = " + TextAnswer1);
        //Debug.Log(TextToCheck2.text + " = " + TextAnswer2);
        if (TextToCheck1.text == TextAnswer1 && TextToCheck2.text == TextAnswer2)
        {
            //Debug.Log("button ready");
            this.GetComponent<Button>().interactable = true;
            //this.GetComponent<Button>().colors = Color.white;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }
}
