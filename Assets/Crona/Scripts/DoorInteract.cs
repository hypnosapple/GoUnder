using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public List<GameObject> wordList;
    public bool unlocked;
    public bool opened;

    void Start()
    {
        unlocked = false;
        opened = false;
    }

    void Update()
    {
        if (wordList.Count == 0)
        {
            unlocked = true;
        }
    }
}
