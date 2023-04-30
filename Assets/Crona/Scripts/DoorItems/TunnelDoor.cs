using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelDoor : MonoBehaviour
{
    public bool opened;

    [Header("Audio")]
    public AudioSource playerAudio;
    public AudioClip doorOpenSFX;

    [Header("Animation")]
    private Animator doorAnimator;

    [Header("Subtitle")]
    public SubtitleData_SO voiceAfter;

    void Start()
    {
        opened = false;
        gameObject.GetComponent<AudioSource>().clip = doorOpenSFX;
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    public void PlayOpenDoor()
    {
        if (!opened)
        {
            doorAnimator.SetBool("OpenDoor", true);
            opened = true;

            if (voiceAfter != null)
            {
                StartCoroutine(WaitForPhoneCall(voiceAfter));
            }
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void CloseDoor()
    {
        if (opened)
        {
            doorAnimator.SetBool("CloseDoor", true);
        }
    }


    IEnumerator WaitForPhoneCall(SubtitleData_SO phoneCall)
    {
        yield return new WaitForSeconds(3f);
        SubtitleManager.Instance.ShowSubtitle(phoneCall);
    }


}
