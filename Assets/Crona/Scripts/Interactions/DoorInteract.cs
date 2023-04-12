using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    [Header("Data")]
    public List<GameObject> wordList;
    public bool unlocked;
    public bool opened;

    [Header("Player and UI")]
    public GameObject UICanvas;
    public GameObject DoorCam;
    public GameObject crosshairCanvas;

    [Header("Audio")]
    public AudioSource playerAudio;
    public AudioClip doorLockSFX;
    public AudioClip doorOpenSFX;
    public AudioClip voiceAfterInteract;
    private bool voicePlayed = false;

    [Header("Animation")]
    private Animator doorAnimator;

    [Header("Subtitle")]
    public SubtitleData_SO voiceAfter;

    

    void Start()
    {
        //unlocked = false;
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

            if (DoorCam != null)
            {
                StartCoroutine(WaitAutoOut());
            }
            
        }

        if (DoorCam != null)
        {
            if (!unlocked)
            {
                if (UICanvas.activeInHierarchy && Input.GetKeyDown(KeyCode.E) && DoorCam.activeInHierarchy)
                {

                    UICanvas.SetActive(false);
                    Cursor.visible = false;

                    DoorCam.SetActive(false);
                    GameManager.Instance.CloseCam1();

                    StartCoroutine(WaitCameraOut());
                }
                
                
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
        else if (DoorCam != null)
        {
            if (!UICanvas.activeInHierarchy)
            {
                Cursor.visible = true;
                //Debug.Log("open");
                DoorCam.SetActive(true);
                GameManager.Instance.pauseMenuEnabled = false;
                GameManager.Instance.inventoryEnabled = false;
                StartCoroutine(WaitCameraIn());

                GameManager.Instance.LockPlayerCam();
                PlayerInteraction.Instance.interactAllowed = false;

                if (!PlayerMovement.Instance.moveDisabled)
                {
                   PlayerMovement.Instance.moveDisabled = true;
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
        SubtitleManager.Instance.ShowSubtitle(phoneCall);
    }


    IEnumerator WaitCameraOut()
    {
        yield return new WaitForSeconds(1f);

        PlayerMovement.Instance.moveDisabled = false;
        crosshairCanvas.SetActive(true);
        GameManager.Instance.UnlockPlayerCam();
        PlayerInteraction.Instance.interactAllowed = true;
        GameManager.Instance.pauseMenuEnabled = true;
        GameManager.Instance.inventoryEnabled = true;

        if (!voicePlayed)
        {
            if (voiceAfterInteract != null)
            {
                playerAudio.clip = voiceAfterInteract;
                playerAudio.Play();
                voicePlayed = true;
            }
        }
    }



    IEnumerator WaitCameraIn()
    {
        yield return new WaitForSeconds(1f);
        UICanvas.SetActive(true);
    }


    IEnumerator WaitAutoOut()
    {
        yield return new WaitForSeconds(1f);
        if (UICanvas.activeInHierarchy)
        {
            UICanvas.SetActive(false);
            Cursor.visible = false;

            DoorCam.SetActive(false);

            StartCoroutine(WaitCameraOut());
        }
    }
    
}