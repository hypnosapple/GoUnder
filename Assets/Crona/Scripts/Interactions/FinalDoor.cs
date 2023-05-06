using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public bool opened;

    [Header("Audio")]
    public AudioSource playerAudio;
    public AudioClip doorOpenSFX;

    [Header("Animation")]
    private Animator doorAnimator;

   

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

            
            gameObject.GetComponent<AudioSource>().Play();

            GameManager.Instance.End();
        }
    }

    
}
