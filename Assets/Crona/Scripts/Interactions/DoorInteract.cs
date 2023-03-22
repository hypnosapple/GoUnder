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
                    Debug.Log("closedoor");
                    UICanvas.SetActive(false);
                    Cursor.visible = false;
                    //Debug.Log("close");
                    DoorCam.SetActive(false);
                    gameManager.GetComponent<GameManager>().CloseCam1();

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
                StartCoroutine(WaitCameraIn());
                
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


    IEnumerator WaitCameraOut()
    {
        yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerMovement>().moveDisabled = false;
        crosshairCanvas.SetActive(true);
        gameManager.GetComponent<PlayerInteraction>().interactAllowed = true;
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