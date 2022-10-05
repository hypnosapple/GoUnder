using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtlasContainerUI : MonoBehaviour
{
    public GameObject AtlasItemPrefab;

    public List<GameObject> AtlasItems = new List<GameObject>();

    public Image picture = null;
    public Text descriptionContent = null;
    public GameObject descriptionPanel;

    public GameObject FileCon;
    public GameObject UseableCon;

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newAtlasItem = Instantiate(AtlasItemPrefab, this.transform);
        AtlasItems.Add(newAtlasItem);

        newAtlasItem.GetComponent<AtlasItemUI>().SetupItemUI(newItemData);

        newAtlasItem.GetComponent<AtlasItemUI>().picture = picture;
        newAtlasItem.GetComponent<AtlasItemUI>().descriptionContent = descriptionContent;
        newAtlasItem.GetComponent<AtlasItemUI>().descriptionPanel = descriptionPanel;

        newAtlasItem.GetComponent<AtlasItemUI>().AtlasCon = this.gameObject;
        newAtlasItem.GetComponent<AtlasItemUI>().FileCon = FileCon;
        newAtlasItem.GetComponent<AtlasItemUI>().UseableCon = UseableCon;
    }

    public void HideAll()
    {
        for (int i = 0; i < AtlasItems.Count; i++)
        {
            AtlasItems[i].GetComponent<AtlasItemUI>().HideItem();
        }
    }
}
