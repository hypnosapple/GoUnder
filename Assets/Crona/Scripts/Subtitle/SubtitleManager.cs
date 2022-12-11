using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public List<string> currentContent;
    public List<float> visibleTimeList;
    public int amount;

    public Text subtitleText;
    public AudioSource playerAudio;
    public string clipName = "";

    public GameObject callReminder;
    public Image callerImage;
    public TMP_Text callerName;

    public GameObject inCallPanel;
    public Image inCallImage;
    public TMP_Text inCallName;

    public GameObject blackPanel;
    public GameObject subtitlePanel;

    public bool isPhone = false;
    private bool toBePicked = false;
    private bool isSlidingIn = false;
    private bool isSlidingOut = false;

    private float TIn;
    private float TOut;

    public SubtitleData_SO GLEnd2;


    void Update()
    {
        if (toBePicked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                callReminder.SetActive(false);
                toBePicked = false;
                playerAudio.Play();

                if (blackPanel.GetComponent<Image>().color.a == 1)
                {
                    GetComponent<GameManager>().StartFadeIn();
                }

                isSlidingIn = true;
                TIn = 0f;
                subtitlePanel.SetActive(true);
                StartCoroutine(Display(0));
            }
        }
        //Debug.Log(inCallPanel.GetComponent<RectTransform>().position);
        CallSlideIn();
        CallSlideOut();
    }


    public void ShowSubtitle(SubtitleData_SO subtitleData)
    {
        currentContent = subtitleData.Contents;
        visibleTimeList = subtitleData.VisibleTime;
        clipName = subtitleData.ClipName;
        amount = currentContent.Count;
        isPhone = subtitleData.isPhoneCall;

        if (subtitleData.AudioFile != null)
        {
            playerAudio.clip = subtitleData.AudioFile;
            
        }

        if (isPhone)
        {
            callerImage.sprite = subtitleData.callerSprite_136_177;
            inCallImage.sprite = subtitleData.callerSprite_136_177;
            callerName.text = subtitleData.callerName;
            inCallName.text = subtitleData.callerName;

            callReminder.SetActive(true);
            toBePicked = true;
        }
        else
        {
            playerAudio.Play();
            subtitlePanel.SetActive(true);
            StartCoroutine(Display(0));
        }


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
            subtitlePanel.SetActive(false);

            subtitleText.text = "";
            currentContent = null;
            visibleTimeList = null;
            amount = 0;

            

            if (isPhone)
            {
                isSlidingOut = true;
                TOut = 0f;
                isPhone = false;
            }

            if (clipName == "Opening")
            {
                clipName = "";
                gameObject.GetComponent<GameManager>().EnableOpeningMove();
            }
            else if (clipName == "GLEnd1")
            {
                clipName = "";
                ShowSubtitle(GLEnd2);
            }
            else if (clipName == "GLEnd2")
            {
                clipName = "";
                gameObject.GetComponent<GameManager>().TransitionToUnderworld();
            }
            else
            {
                clipName = "";
            }

            

            yield break;
        }
        
    }


    public void CallSlideIn()
    {
        if (isSlidingIn)
        {
            if (TIn < 1f)
            {
                inCallPanel.GetComponent<RectTransform>().position = Vector3.Lerp(new Vector3(0, 540, 0), new Vector3(263.5f, 540, 0), TIn);
                TIn += 3 * Time.deltaTime;
            }
            else
            {
                isSlidingIn = false;
            }
        }
    }


    public void CallSlideOut()
    {
        if (isSlidingOut)
        {
            if (TOut < 1f)
            {
                inCallPanel.GetComponent<RectTransform>().position = Vector3.Lerp(new Vector3(263.5f, 540, 0), new Vector3(0, 540, 0), TOut);
                TOut += 2 * Time.deltaTime;
            }
            else
            {
                isSlidingOut = false;
            }
        }
    }

}
