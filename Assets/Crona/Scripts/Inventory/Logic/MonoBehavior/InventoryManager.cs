using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Data")]

    public List<InventoryItem> AtlasItems = new List<InventoryItem>();
    public List<InventoryItem> FileItems = new List<InventoryItem>();
    public List<InventoryItem> UseableItems = new List<InventoryItem>();

    public GameObject AtlasContainer;
    public GameObject FileContainer;
    public GameObject UseableContainer;

    public GameObject PopupPanel;
    public Image ReminderInfo;
    public TMP_Text ReminderName;
    public TMP_Text ReminderType;
    public int timer = 0;
    private float t = 1f;
    private float posT = 1f;


    

    void Update()
    {
        ReminderSlide();
        
    }

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

                    ShowReminder(newItemData.name, "Info Added");
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

                    ShowReminder(newItemData.name, "File Added");
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

                    ShowReminder(newItemData.name, "Item Added");
                    break;
                }
            }
        }
    }


    public void ShowReminder(string title, string type)
    {
        ReminderName.text = title;
        ReminderType.text = type;

        timer = 540;
        t = 0f;
        posT = 0f;
        
    }

    public void ReminderSlide()
    {
        if (timer > 0)
        {
            timer--;
        }

        if (timer > 60)
        {
            if (posT < 1f)
            {
                PopupPanel.GetComponent<RectTransform>().position = Vector3.Lerp(new Vector3(2300, 520, 0), new Vector3(1920, 520, 0), posT);

                posT += 4 * Time.deltaTime;
            }
            else
            {
                PopupPanel.GetComponent<RectTransform>().position = new Vector3(1920, 520, 0);
            }


            if (t < 1f)
            {
                ReminderInfo.color = Color.Lerp(new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 0f), new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 1f), t);
                ReminderName.color = Color.Lerp(new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 0f), new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 1f), t);
                ReminderType.color = Color.Lerp(new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 0f), new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 1f), t);

                t += 3f * Time.deltaTime;
            }
            else
            {
                ReminderInfo.color = new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 1f);
                ReminderName.color = new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 1f);
                ReminderType.color = new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 1f);
            }
        }

        else if (timer == 60)
        {
            t = 0f;
            posT = 0f;
        }

        else if (timer < 60)
        {

            if (t < 1f)
            {
                ReminderInfo.color = Color.Lerp(new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 1f), new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 0f), t);
                ReminderName.color = Color.Lerp(new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 1f), new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 0f), t);
                ReminderType.color = Color.Lerp(new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 1f), new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 0f), t);

                t += 4f * Time.deltaTime;
            }
            else
            {
                ReminderInfo.color = new Color(ReminderInfo.color.r, ReminderInfo.color.g, ReminderInfo.color.b, 0f);
                ReminderName.color = new Color(ReminderName.color.r, ReminderName.color.g, ReminderName.color.b, 0f);
                ReminderType.color = new Color(ReminderType.color.r, ReminderType.color.g, ReminderType.color.b, 0f);
            }

            if (posT < 1f)
            {
                PopupPanel.GetComponent<RectTransform>().position = Vector3.Lerp(new Vector3(1920, 520, 0), new Vector3(2300, 520, 0), posT);

                posT += 4 * Time.deltaTime;
            }
            else
            {
                PopupPanel.GetComponent<RectTransform>().position = new Vector3(2300, 520, 0);
            }
        }
    }

    
}



[System.Serializable]

public class InventoryItem
{
    public ItemData_SO itemData;
}
