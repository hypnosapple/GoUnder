using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtlasItemUI : MonoBehaviour
{
    public Image picture = null;
    public Text title = null;
    public Text descriptionContent = null;

    public GameObject descriptionPanel;
    public GameObject InfoPanel;
    public GameObject scrollBar;

    public ItemData_SO itemData;

    public bool thisItemIsShowing = false;

    public GameObject AtlasCon;
    public GameObject FileCon;
    public GameObject UseableCon;

    void Update()
    {
        if (thisItemIsShowing)
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (descriptionPanel.activeInHierarchy)
                {
                    descriptionPanel.SetActive(false);
                }
                else
                {
                    descriptionPanel.SetActive(true);
                    descriptionContent.text = itemData.description;
                    scrollBar.GetComponent<Scrollbar>().value = 1;
                }
            }
        }
    }


    public void SetupItemUI(ItemData_SO item)
    {
        if (item != null)
        {
            itemData = item;
            title.text = item.itemName;

        }
    }

    public void ShowItem()
    {
        HideAllInParents();

        InfoPanel.SetActive(true);
        picture.gameObject.SetActive(true);
        picture.sprite = itemData.itemIcon_1100_700;
        thisItemIsShowing = true;
    }

    public void HideItem()
    {
        title.color = new Color32(87, 111, 132, 255);

        if (thisItemIsShowing)
        {
            picture.sprite = null;
            descriptionContent.text = null;
            picture.gameObject.SetActive(false);
            descriptionPanel.SetActive(false);
            InfoPanel.SetActive(false);

            thisItemIsShowing = false;
        }
    }

        

    public void HideAllInParents()
    {
        
        AtlasCon.GetComponent<AtlasContainerUI>().HideAll();
        FileCon.GetComponent<FileContainerUI>().HideAll();
        UseableCon.GetComponent<UseableContainerUI>().HideAll();
    }

    public void ChangeColor()
    {
        if (title.color == new Color32(87, 111, 132, 255))
        {
            title.color = new Color32(205, 236, 251, 255);
        }
    }
}
