using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageArrows : MonoBehaviour
{

    public List<GameObject> pageList = new List<GameObject>();
    private int currentIndex = 0;

    public void CycleThroughObjects()
    {
        if (pageList.Count == 0) return;

        pageList[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % pageList.Count;
        pageList[currentIndex].SetActive(true);
    }

    public void CycleBackwardsThroughObjects()
    {
        if (pageList.Count == 0) return;

        pageList[currentIndex].SetActive(false);
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = pageList.Count - 1;
        }
        pageList[currentIndex].SetActive(true);
    }
}
