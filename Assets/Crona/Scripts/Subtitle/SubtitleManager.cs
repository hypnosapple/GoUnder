using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubtitleManager : MonoBehaviour
{
    public List<string> currentContent;
    public List<float> visibleTimeList;
    public int amount;

    public Text subtitleText;
    public AudioSource playerAudio;
    public string clipName = "";


    public void ShowSubtitle(SubtitleData_SO subtitleData)
    {
        currentContent = subtitleData.Contents;
        visibleTimeList = subtitleData.VisibleTime;
        clipName = subtitleData.ClipName;
        amount = currentContent.Count;

        if (subtitleData.AudioFile != null)
        {
            playerAudio.clip = subtitleData.AudioFile;
            playerAudio.Play();
        }

        StartCoroutine(Display(0));

        

    }

    IEnumerator Display(int i)
    {
        if (i < amount)
        {
            subtitleText.text = "";
            subtitleText.DOText(currentContent[i], 2f);
            //Debug.Log(currentContent[i]);
            yield return new WaitForSeconds(visibleTimeList[i]);
            StartCoroutine(Display(i + 1));
        }
        else
        {
            subtitleText.text = "";
            currentContent = null;
            visibleTimeList = null;
            amount = 0;

            if (clipName == "Beginning")
            {
                gameObject.GetComponent<GameManager>().EnableMove();
            }

            clipName = "";

            yield break;
        }
        
    }

}
