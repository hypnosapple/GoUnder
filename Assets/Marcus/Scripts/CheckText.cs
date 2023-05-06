using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CheckText : MonoBehaviour
{
    public string nameToMatch;
    public bool matches;
    public GameObject visualWord;
    public GameObject door;

    public bool rightWordIn;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == nameToMatch && other.gameObject.GetComponent<ItemDrag>() != null)
        {
            rightWordIn = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == nameToMatch && other.gameObject.GetComponent<ItemDrag>() != null)
        {
            rightWordIn = false;
        }
        
    }

    public void RightWord()
    {
        matches = true;

        visualWord.gameObject.SetActive(true);
        door.GetComponent<DoorInteract>().wordList.Remove(visualWord);
        //GameManager.Instance.currentRoomIndex += 1;
        //door.GetComponent<DoorInteract>().PlayOpenDoor();
        // doorInteract.unlocked = true;
    }

   


}