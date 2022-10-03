using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableContainerUI : MonoBehaviour
{
    public GameObject UseableItemPrefab;

    public List<GameObject> UseableItems = new List<GameObject>();

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newAtlasItem = Instantiate(UseableItemPrefab, this.transform);
        UseableItems.Add(newAtlasItem);

        newAtlasItem.GetComponent<UseableItemUI>().SetupItemUI(newItemData);
    }

    public void HideAll()
    {
        for (int i = 0; i < UseableItems.Count; i++)
        {
            UseableItems[i].GetComponent<UseableItemUI>().HideItem();
        }
    }
}
