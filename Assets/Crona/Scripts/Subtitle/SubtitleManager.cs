using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;

    [Header("Manager Current Info")]
    public List<string> currentContent;
    public List<float> visibleTimeList;
    public int amount;

    [Header("Audio and UI")]
    public Text subtitleText;
    public AudioSource playerAudio;
    public string clipName = "";

    public GameObject callReminder;
    public Image callerImage;
    public TMP_Text callerName;

    public GameObject inCallPanel;
    public Image inCallImage;
    public TMP_Text inCallName;
    public AudioClip phoneCallAudio;

    public GameObject blackPanel;
    public GameObject subtitlePanel;

    public bool isPhone = false;
    private bool toBePicked = false;
    private bool isSlidingIn = false;
    private bool isSlidingOut = false;

    private float TIn;
    private float TOut;

    public SubtitleData_SO GLEnd2;

    public AudioClip afterCall01;
    public AudioClip afterCall03;
    public AudioClip phoneRing;
    public AudioClip phoneHang;

    public bool audioPlaying;
    public bool Call03Ended;


    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if (toBePicked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                callReminder.SetActive(false);
                toBePicked = false;
                playerAudio.loop = false;
                playerAudio.Stop();

                playerAudio.clip = phoneCallAudio;
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

        if (audioPlaying)
        {
            if (!playerAudio.isPlaying)
            {
                audioPlaying = false;
                PlayerInteraction.Instance.interactAllowed = true;
            }
        }
        //Debug.Log(inCallPanel.GetComponent<RectTransform>().position);
        CallSlideIn();
        CallSlideOut();
    }


    public void ShowSubtitle(SubtitleData_SO subtitleData)
    {
        StopAllCoroutines();
        currentContent = subtitleData.Contents;
        visibleTimeList = subtitleData.VisibleTime;
        clipName = subtitleData.ClipName;
        amount = currentContent.Count;
        isPhone = subtitleData.isPhoneCall;
        PlayerInteraction.Instance.interactAllowed = false;

        if (subtitleData.AudioFile != null)
        {
            phoneCallAudio = subtitleData.AudioFile;
            
        }

        if (isPhone)
        {
            callerImage.sprite = subtitleData.callerSprite_136_177;
            inCallImage.sprite = subtitleData.callerSprite_136_177;
            callerName.text = subtitleData.callerName;
            inCallName.text = subtitleData.callerName;

            callReminder.SetActive(true);
            toBePicked = true;

            playerAudio.clip = phoneRing;
            playerAudio.loop = true;
            playerAudio.Play();
        }
        else
        {
            playerAudio.clip = phoneCallAudio;
            playerAudio.Play();
            GameManager.Instance.ControlRendererFeature(1, true);
            subtitlePanel.SetActive(true);
            StartCoroutine(Display(0));
        }


    }


    IEnumerator Display(int i)
    {
        if (i < amount)
        {
            subtitleText.text = "";
            subtitleText.DOText(currentContent[i], 1f);
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
            PlayerInteraction.Instance.interactAllowed = true;
            

            if (isPhone)
            {
                playerAudio.clip = phoneHang;
                playerAudio.Play();

                

                isSlidingOut = true;
                TOut = 0f;
                isPhone = false;
            }
            else
            {
                GameManager.Instance.ControlRendererFeature(1, false);
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
            else if (clipName == "3rd_Call01")
            {
                clipName = "";
                StartCoroutine(PlayVOAfterCall01());
            }
            else if (clipName == "3rd_Call03")
            {
                clipName = "";
                StartCoroutine(PlayVOAfterCall03());
            }
            else if (clipName == "2nd_Call04")
            {
                clipName = "";
                Tinylytics.AnalyticsManager.LogSecondFloorPlaytime();
                
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
                inCallPanel.GetComponent<RectTransform>().position = new Vector3(263.5f, 540, 0);
                GameManager.Instance.ControlRendererFeature(1, true);
                GameManager.Instance.pauseMenuEnabled = false;
                GameManager.Instance.inventoryEnabled = false;
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
                inCallPanel.GetComponent<RectTransform>().position = new Vector3(0, 540, 0);
                isSlidingOut = false;
                GameManager.Instance.ControlRendererFeature(1, false);
                GameManager.Instance.pauseMenuEnabled = true;
                GameManager.Instance.inventoryEnabled = true;
            }
        }
    }


    IEnumerator PlayVOAfterCall01()
    {
        yield return new WaitForSeconds(2f);
        playerAudio.clip = afterCall01;
        playerAudio.Play();

        audioPlaying = true;
        PlayerInteraction.Instance.interactAllowed = false;
    }

    IEnumerator PlayVOAfterCall03()
    {
        yield return new WaitForSeconds(2f);
        playerAudio.clip = afterCall03;
        playerAudio.Play();
        audioPlaying = true;
        PlayerInteraction.Instance.interactAllowed = false;

        yield return new WaitForSeconds(4f);
        Call03Ended = true;
    }


    public void PlayAfterTime(SubtitleData_SO subtitle, float waitTime)
    {
        StartCoroutine(PlayVOAfterSeconds(subtitle, waitTime));
    }

    IEnumerator PlayVOAfterSeconds(SubtitleData_SO subtitle, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowSubtitle(subtitle);
    }
}
