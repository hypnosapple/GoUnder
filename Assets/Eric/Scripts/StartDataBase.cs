using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDataBase : MonoBehaviour
{
    public Text TextToCheck1, TextToCheck2, TextToCheck3, TextToCheck4;
    public string FirstAnswer1, FirstAnswer2, FirstAnswer3, FirstAnswer4;
    public string SecondAnswer1, SecondAnswer2, SecondAnswer3, SecondAnswer4;
    public GameObject nextPage, wrongPage;
    public GameObject LoadingBar, InfoNotUpdate, NewMail;
    public int AnswerStage;

    public MailSystem InboxMail;
    public void StartButton(GameObject nextPage)
    {
        if (AnswerStage == 0)
        {
            if (TextToCheck1.text == FirstAnswer1 &&
TextToCheck2.text == FirstAnswer2 &&
TextToCheck3.text == FirstAnswer3 &&
TextToCheck4.text == FirstAnswer4)
            {
                AnswerStage++;
                FirstAnswerCorrect();
                //clear out the answers
                TextToCheck1.text = "";
                TextToCheck2.text = "";
                TextToCheck3.text = "";
                TextToCheck4.text = "";

            }
            else
            {
                wrongPage.SetActive(true);
            }
        }
        else if (AnswerStage == 1)
        {
            if (TextToCheck1.text == SecondAnswer1 &&
    TextToCheck2.text == SecondAnswer2 &&
    TextToCheck3.text == SecondAnswer3 &&
    TextToCheck4.text == SecondAnswer4)
            {
                //nextPage.SetActive(true);
                LoadingBar.SetActive(true);
                StartCoroutine(NextpageCo());
                //clear out the answers
                TextToCheck1.text = "";
                TextToCheck2.text = "";
                TextToCheck3.text = "";
                TextToCheck4.text = "";
            }
            else
            {
                wrongPage.SetActive(true);
            }
        }
    }


    public void FirstAnswerCorrect()
    {
        InfoNotUpdate.SetActive(true);
        //set active the ui
        NewMail.SetActive(true);
        //add new mail
        InboxMail.AddNewMail();
    }

    IEnumerator NextpageCo()
    {
        yield return new WaitForSeconds(2) ;
        nextPage.SetActive(true);
    }
}
