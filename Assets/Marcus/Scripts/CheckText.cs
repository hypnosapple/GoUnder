using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        TextMeshProUGUI otherText = other.GetComponent<TextMeshProUGUI>();
        if (otherText != null && otherText.text == textToMatch)
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
        }
    }
}
