using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileContainerUI : MonoBehaviour
{
    public GameObject FileItemPrefab;

    public List<GameObject> FileItems = new List<GameObject>();

    public GameObject InfoPanel; 
    public Image picture = null;
    public Text descriptionContent = null;
    public GameObject descriptionPanel;
    public GameObject scrollBar;

    public GameObject AtlasCon;
    public GameObject UseableCon;

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newFileItem = Instantiate(FileItemPrefab, this.transform);
        FileItems.Add(newFileItem);

        newFileItem.GetComponent<FileItemUI>().SetupItemUI(newItemData);

        newFileItem.GetComponent<FileItemUI>().picture = picture;
        newFileItem.GetComponent<FileItemUI>().descriptionContent = descriptionContent;
        newFileItem.GetComponent<FileItemUI>().descriptionPanel = descriptionPanel;
        newFileItem.GetComponent<FileItemUI>().scrollBar = scrollBar;
        newFileItem.GetComponent<FileItemUI>().InfoPanel = InfoPanel;

        newFileItem.GetComponent<FileItemUI>().AtlasCon = AtlasCon;
        newFileItem.GetComponent<FileItemUI>().FileCon = this.gameObject;
        newFileItem.GetComponent<FileItemUI>().UseableCon = UseableCon;
    }

    public void HideAll()
    {
        for (int i = 0; i < FileItems.Count; i++)
        {
            FileItems[i].GetComponent<FileItemUI>().HideItem();
        }
    }
}
