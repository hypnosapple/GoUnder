using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckText : MonoBehaviour
{
    public string textToMatch;
    public bool matches;

    private void OnTriggerEnter(Collider other)
    {
        TextMesh otherText = other.GetComponent<TextMesh>();
        if (otherText != null && otherText.text == textToMatch)
        {
            matches = true;
            Debug.Log("Word Matches!");
        }
    }
}
