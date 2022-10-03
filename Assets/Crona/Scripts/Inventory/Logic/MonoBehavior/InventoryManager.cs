using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Data")]

    public List<InventoryItem> AtlasItems = new List<InventoryItem>();
    public List<InventoryItem> FileItems = new List<InventoryItem>();
    public List<InventoryItem> UseableItems = new List<InventoryItem>();

    public GameObject AtlasContainer;
    public GameObject FileContainer;
    public GameObject UseableContainer;


    public void AddItem(ItemData_SO newItemData)
    {
        
        if (newItemData.itemType == ItemType.Atlas)
        {
            for (int i = 0; i < AtlasItems.Count; i++)
            {
                if (AtlasItems[i].itemData == null)
                {
                    AtlasItems[i].itemData = newItemData;
                    AtlasContainer.GetComponent<AtlasContainerUI>().AddItem(newItemData);
                    break;
                }
            }
        }

        if (newItemData.itemType == ItemType.File)
        {
            for (int i = 0; i < FileItems.Count; i++)
            {
                if (FileItems[i].itemData == null)
                {
                    FileItems[i].itemData = newItemData;
                    FileContainer.GetComponent<FileContainerUI>().AddItem(newItemData);
                    break;
                }
            }
        }

        if (newItemData.itemType == ItemType.Useable)
        {
            for (int i = 0; i < UseableItems.Count; i++)
            {
                if (UseableItems[i].itemData == null)
                {
                    UseableItems[i].itemData = newItemData;
                    UseableContainer.GetComponent<UseableContainerUI>().AddItem(newItemData);
                    break;
                }
            }
        }
    }
}



[System.Serializable]

public class InventoryItem
{
    public ItemData_SO itemData;
}
