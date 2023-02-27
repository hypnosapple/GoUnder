using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightArrow : MonoBehaviour
{
    public List<GameObject> objectsList = new List<GameObject>();
    private int currentIndex = 0;

    public void CycleThroughObjects()
    {
        if (objectsList.Count == 0) return;

        objectsList[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % objectsList.Count;
        objectsList[currentIndex].SetActive(true);
    }

    public void CycleBackwardsThroughObjects()
    {
        if (objectsList.Count == 0) return;

        objectsList[currentIndex].SetActive(false);
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = objectsList.Count - 1;
        }
        objectsList[currentIndex].SetActive(true);
    }

}
