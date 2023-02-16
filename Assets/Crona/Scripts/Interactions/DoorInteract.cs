using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public List<GameObject> wordList;
    private bool unlocked;
    public bool opened;

    public AudioClip doorLockSFX;
    public AudioClip doorOpenSFX;

    public GameManager gameManager;
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
        }
    }

    public void PlayOpenDoor()
    {
        if (unlocked)
        {
            doorAnimator.SetBool("OpenDoor", true);
            opened = true;
            StartCoroutine(WaitForPhoneCall(voiceAfter));
        }
        gameObject.GetComponent<AudioSource>().Play();
    }


    IEnumerator WaitForPhoneCall(SubtitleData_SO phoneCall)
    {
        yield return new WaitForSeconds(3f);
        gameManager.GetComponent<SubtitleManager>().ShowSubtitle(phoneCall);
    }
}
