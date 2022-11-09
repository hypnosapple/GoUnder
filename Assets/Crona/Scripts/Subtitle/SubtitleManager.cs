using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleManager : MonoBehaviour
{
    public List<string> currentContent;
    public List<float> visibleTimeList;
    public int amount;

    public Text subtitleText;

    public void ShowSubtitle(SubtitleData_SO subtitleData)
    {
        currentContent = subtitleData.content;
        visibleTimeList = subtitleData.visibleTime;
        amount = currentContent.Count;

        StartCoroutine(Display(0));

        

    }

    IEnumerator Display(int i)
    {
        if (i < amount)
        {
            subtitleText.text = currentContent[i];
            Debug.Log(currentContent[i]);
            yield return new WaitForSeconds(visibleTimeList[i]);
            StartCoroutine(Display(i + 1));
        }
        else
        {
            subtitleText.text = "";
            currentContent = null;
            visibleTimeList = null;
            amount = 0;
            yield break;
        }
        
    }

}
