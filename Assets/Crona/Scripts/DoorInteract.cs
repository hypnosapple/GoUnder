using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public List<GameObject> wordList;
    public bool unlocked;
    public bool opened;

    public GameObject UICanvas;
    public GameObject DoorCam;
    public GameObject player;
    public GameObject crosshairCanvas;

    public AudioClip doorLockSFX;
    public AudioClip doorOpenSFX;

    public GameObject gameManager;
    private Animator doorAnimator;

    public SubtitleData_SO voiceAfter;

    void Start()
    {
        unlocked = false;
        opened = false;
        gameObject.GetComponent<AudioSource>().clip = doorLockSFX;
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (wordList.Count == 0 && !unlocked)
        {
            unlocked = true;
            gameObject.GetComponent<AudioSource>().clip = doorOpenSFX;

            if (UICanvas != null)
            {
                if (UICanvas.activeInHierarchy)
                {
                    UICanvas.SetActive(false);
                    Cursor.visible = false;
                    DoorCam.SetActive(false);

                    StartCoroutine(WaitForCamera());
                }
            }
            
        }

        if (UICanvas != null)
        {
            if (!unlocked && UICanvas.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
            {
                UICanvas.SetActive(false);
                Cursor.visible = false;
                DoorCam.SetActive(false);

                StartCoroutine(WaitForCamera());
            }
        }

        
    }

    public void PlayOpenDoor()
    {
        if (unlocked)
        {
            doorAnimator.SetBool("OpenDoor", true);
            opened = true;

            if (voiceAfter != null)
            {
                StartCoroutine(WaitForPhoneCall(voiceAfter));
            }
            gameObject.GetComponent<AudioSource>().Play();
        }
        else if (UICanvas != null)
        {
            if (!UICanvas.activeInHierarchy)
            {
                UICanvas.SetActive(true);
                Cursor.visible = true;
                DoorCam.SetActive(true);
                gameManager.GetComponent<PlayerInteraction>().interactAllowed = false;

                if (!player.GetComponent<PlayerMovement>().moveDisabled)
                {
                    player.GetComponent<PlayerMovement>().moveDisabled = true;
                }

                if (crosshairCanvas.activeInHierarchy)
                {
                    crosshairCanvas.SetActive(false);
                }

                gameObject.GetComponent<AudioSource>().Play();
            }
            
        }

        
    }


    IEnumerator WaitForPhoneCall(SubtitleData_SO phoneCall)
    {
        yield return new WaitForSeconds(3f);
        gameManager.GetComponent<SubtitleManager>().ShowSubtitle(phoneCall);
    }


    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerMovement>().moveDisabled = false;
        crosshairCanvas.SetActive(true);
        gameManager.GetComponent<PlayerInteraction>().interactAllowed = true;
    }



    public List<GameObject> pageList = new List<GameObject>();
    private int currentIndex = 0;

    public void CycleThroughObjects()
    {
        if (pageList.Count == 0) return;

        pageList[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % pageList.Count;
        pageList[currentIndex].SetActive(true);
    }

    public void CycleBackwardsThroughObjects()
    {
        if (pageList.Count == 0) return;

        pageList[currentIndex].SetActive(false);
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = pageList.Count - 1;
        }
        pageList[currentIndex].SetActive(true);
    }
}