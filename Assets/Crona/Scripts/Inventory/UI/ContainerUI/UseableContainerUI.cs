using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseableContainerUI : MonoBehaviour
{
    public GameObject UseableItemPrefab;

    public List<GameObject> UseableItems = new List<GameObject>();

    public Text descriptionContent = null;
    public GameObject descriptionPanel;
    public GameObject item3DViewer;

    public GameObject FileCon;
    public GameObject AtlasCon;

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newUseableItem = Instantiate(UseableItemPrefab, this.transform);
        UseableItems.Add(newUseableItem);

        newUseableItem.GetComponent<UseableItemUI>().SetupItemUI(newItemData);

        newUseableItem.GetComponent<UseableItemUI>().descriptionContent = descriptionContent;
        newUseableItem.GetComponent<UseableItemUI>().descriptionPanel = descriptionPanel;
        newUseableItem.GetComponent<UseableItemUI>().item3DViewer = item3DViewer;

        newUseableItem.GetComponent<UseableItemUI>().AtlasCon = AtlasCon;
        newUseableItem.GetComponent<UseableItemUI>().FileCon = FileCon;
        newUseableItem.GetComponent<UseableItemUI>().UseableCon = this.gameObject;
    }

    public void HideAll()
    {
        for (int i = 0; i < UseableItems.Count; i++)
        {
            UseableItems[i].GetComponent<UseableItemUI>().HideItem();
        }
    }
}
