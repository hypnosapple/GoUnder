using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Data")]
    
    public ItemData_SO itemData;
    public GameObject relatedDoor;
    public GameObject relatedWord;

    [Header("Audio")]
    public AudioSource playerAudio;
    public AudioClip AfterPickVO;

    [Header("Subtitle Data")]
    public SubtitleData_SO secondVO;
    public float playAfterSeconds;

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(itemData);

        //Debug.Log("pickup");

        if (relatedDoor != null && relatedWord != null)
        {
            relatedWord.SetActive(true);

            relatedDoor.GetComponent<DoorInteract>().wordList.Remove(relatedWord);
        }

        if (AfterPickVO != null)
        {
            playerAudio.clip = AfterPickVO;
            playerAudio.Play();
            SubtitleManager.Instance.audioPlaying = true;
            PlayerInteraction.Instance.interactAllowed = false;
        }

        if (secondVO != null)
        {
            SubtitleManager.Instance.PlayAfterTime(secondVO, playAfterSeconds);
        }

        DestroyPickup(gameObject);
    }

    public void DestroyPickup(GameObject GO)
    {
        Destroy(GO);
    }
}
