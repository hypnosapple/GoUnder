using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckText : MonoBehaviour
{
    public string textToMatch;
    public bool matches;
    public GameObject visualWord;

    public CheckText instance;

    public GameObject door;


    public void Start()

    {
        instance = this;

    }

    public void OnTriggerEnter(Collider other)
    {
        Text otherText = other.GetComponent<Text>();
        if (otherText != null && otherText.text == textToMatch)
        {
            matches = true;
            Debug.Log("Word Matches!");
            visualWord.gameObject.SetActive(true);
            door.GetComponent<DoorInteract>().wordList.RemoveAt(0);
            door.GetComponent<DoorInteract>().PlayOpenDoor();

            // doorInteract.unlocked = true;



        }

        else
        {
            Debug.Log("DOES NOT MATCH");
        }
    }
}
