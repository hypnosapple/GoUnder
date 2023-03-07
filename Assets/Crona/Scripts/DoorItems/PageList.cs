using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageList : MonoBehaviour
{
    public List<GameObject> pageList;
    public List<GameObject> checkWordList;

    public GameObject doorUICanvas;

    public GameObject doorItemPrefab;

    public GameObject itemHolder01;
    public GameObject itemHolder02;
    public GameObject itemHolder03;

    public void AddItemToDoor(Sprite itemSprite, string itemName, int checkWordNumber)
    {
        for (int i = 0; i < pageList.Count; i++)
        {
            if (pageList[i].GetComponent<ItemList>().itemsInThisPage < 3)
            {
                GameObject newDoorItem = Instantiate(doorItemPrefab, pageList[i].transform);

                if (pageList[i].GetComponent<ItemList>().itemsInThisPage == 0 && i != 0)
                {
                    doorUICanvas.GetComponent<PageArrows>().pageList.Add(pageList[i]);
                    //newDoorItem.GetComponent<RectTransform>().anchoredPosition3D = itemHolder01.GetComponent<RectTransform>().anchoredPosition3D;
                }
                else if (pageList[i].GetComponent<ItemList>().itemsInThisPage == 1)
                {
                    //newDoorItem.GetComponent<RectTransform>().anchoredPosition3D = itemHolder02.GetComponent<RectTransform>().anchoredPosition3D;
                }
                else if (pageList[i].GetComponent<ItemList>().itemsInThisPage == 2)
                {
                    //newDoorItem.GetComponent<RectTransform>().anchoredPosition3D = itemHolder03.GetComponent<RectTransform>().anchoredPosition3D;
                }


                pageList[i].GetComponent<ItemList>().itemsInThisPage += 1;

                
                newDoorItem.name = itemName;
                newDoorItem.GetComponent<Image>().sprite = itemSprite;

                if (checkWordNumber >= 0)
                {
                    newDoorItem.GetComponent<ItemDrag>().checkText = checkWordList[checkWordNumber].GetComponent<CheckText>();
                }

                break;
            }
        }
    }
}
