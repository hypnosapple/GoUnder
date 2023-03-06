using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;
    public GameObject InventoryCanvas;

    public GameObject relatedDoor;
    public GameObject relatedWord;

    public AudioSource playerAudio;
    public AudioClip AfterPickVO;

    public SubtitleData_SO secondVO;
    public float playAfterSeconds;

    public GameObject gameManager;

    public void Pickup()
    {
        InventoryCanvas.GetComponent<InventoryManager>().AddItem(itemData);
        //Debug.Log("pickup");

        if (relatedDoor != null)
        {
            relatedWord.SetActive(true);

            relatedDoor.GetComponent<DoorInteract>().wordList.Remove(relatedWord);
        }

        if (AfterPickVO != null)
        {
            playerAudio.clip = AfterPickVO;
            playerAudio.Play();
        }

        if (secondVO != null)
        {
            gameManager.GetComponent<SubtitleManager>().PlayAfterTime(secondVO, playAfterSeconds);
        }


        Destroy(gameObject);
    }
}
