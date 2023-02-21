using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public List<GameObject> wordList;
    public bool unlocked;
    public bool opened;

    public AudioClip doorLockSFX;
    public AudioClip doorOpenSFX;

  public void Start()
    {
        unlocked = false;
        opened = false;
        gameObject.GetComponent<AudioSource>().clip = doorLockSFX;
    }

 public void Update()
    {
        if (wordList.Count == 0 && !unlocked)
        {
            unlocked = true;
            gameObject.GetComponent<AudioSource>().clip = doorOpenSFX;
        }
    }
}
