using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDataBase : MonoBehaviour
{
    public Text TextToCheck1, TextToCheck2, TextToCheck3, TextToCheck4;
    public string TextAnswer1, TextAnswer2, TextAnswer3, TextAnswer4;
    public GameObject nextPage, wrongPage;

    public void StartButton(GameObject nextPage)
    {
        if (TextToCheck1.text == TextAnswer1 &&
            TextToCheck2.text == TextAnswer2 &&
            TextToCheck3.text == TextAnswer3 &&
            TextToCheck4.text == TextAnswer4)
        {
            nextPage.SetActive(true);
        }
        else
        {
            wrongPage.SetActive(true);
        }

    }
}
