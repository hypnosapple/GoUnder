using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseableItemUI : MonoBehaviour
{
    public Text title = null;
    public Text descriptionContent = null;
    public GameObject descriptionPanel;
    public Transform itemModel;
    public GameObject item3DViewer;

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

        item3DViewer.SetActive(true);
        if (itemModel != null)
        {
            Destroy(itemModel.gameObject);
            item3DViewer.GetComponent<ItemViewer>().itemModel = null;
        }
        itemModel = Instantiate(itemData.modelPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
        item3DViewer.GetComponent<ItemViewer>().itemModel = itemModel;

        thisItemIsShowing = true;
    }

    public void HideItem()
    {
        if (itemModel != null)
        {
            Destroy(itemModel.gameObject);
            item3DViewer.GetComponent<ItemViewer>().itemModel = null;
        }

        descriptionContent.text = null;
        descriptionPanel.SetActive(false);
        item3DViewer.SetActive(false);

        thisItemIsShowing = false;
    }

    public void HideAllInParents()
    {

        AtlasCon.GetComponent<AtlasContainerUI>().HideAll();
        FileCon.GetComponent<FileContainerUI>().HideAll();
        UseableCon.GetComponent<UseableContainerUI>().HideAll();
    }
}
