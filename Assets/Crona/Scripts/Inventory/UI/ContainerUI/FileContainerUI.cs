using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileContainerUI : MonoBehaviour
{
    public GameObject FileItemPrefab;

    public List<GameObject> FileItems = new List<GameObject>();

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newAtlasItem = Instantiate(FileItemPrefab, this.transform);
        FileItems.Add(newAtlasItem);

        newAtlasItem.GetComponent<FileItemUI>().SetupItemUI(newItemData);
    }

    public void HideAll()
    {
        for (int i = 0; i < FileItems.Count; i++)
        {
            FileItems[i].GetComponent<FileItemUI>().HideItem();
        }
    }
}
