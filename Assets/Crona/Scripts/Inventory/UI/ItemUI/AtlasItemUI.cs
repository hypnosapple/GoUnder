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

        picture.gameObject.SetActive(true);
        picture.sprite = itemData.itemIcon;
        thisItemIsShowing = true;
    }

    public void HideItem()
    {

        if (thisItemIsShowing)
        {
            picture.sprite = null;
            descriptionContent.text = null;
            picture.gameObject.SetActive(false);
            descriptionPanel.SetActive(false);

            thisItemIsShowing = false;
        }
    }

        

    public void HideAllInParents()
    {
        
        AtlasCon.GetComponent<AtlasContainerUI>().HideAll();
        FileCon.GetComponent<FileContainerUI>().HideAll();
        UseableCon.GetComponent<UseableContainerUI>().HideAll();
    }
}
