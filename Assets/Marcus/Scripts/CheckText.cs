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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == nameToMatch)
        {
            matches = true;
            Debug.Log("Word Matches!");
            visualWord.gameObject.SetActive(true);
            door.GetComponent<DoorInteract>().wordList.Remove(visualWord);
            //door.GetComponent<DoorInteract>().PlayOpenDoor();
            // doorInteract.unlocked = true;
        }
        else
        {
            Debug.Log("DOES NOT MATCH");
            
            other.gameObject.GetComponent<ItemDrag>().resetItemPosition();
        }
    }

    
}