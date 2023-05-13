using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Data")]

    public List<InventoryItem> AtlasItems = new List<InventoryItem>();
    public List<InventoryItem> FileItems = new List<InventoryItem>();
    public List<InventoryItem> UseableItems = new List<InventoryItem>();

    [Header("Inventory UI")]
    public GameObject AtlasContainer;
    public GameObject FileContainer;
    public GameObject UseableContainer;

    public GameObject itemPanel;
    public GameObject filePanel;
    public Image fileImage;
    public Transform itemPrefab;
    public GameObject previewModel;

    [Header("Door UI")]
    public GameObject doorItemPages;

    [Header("Reminder UI")]
    public GameObject PopupPanel;
    public Image ReminderInfo;
    public TMP_Text ReminderName;
    public TMP_Text ReminderType;
    public int timer = 0;
    private float t = 1f;
    private float posT = 1f;

    [Header("Player Info")]
    public GameObject crosshairCanvas;
    public GameObject mainCam;

    [Header("Pause Menu")]
    public bool pauseMenuEnabled = true;

    //Alexis added
    [Header("Item Pickup")]
    private List<GameObject> itemPickup;

    [Header("Editor Control")]
    public bool isInEditor;

    //[Header("Inventory Metrics")]
    //public int itemCollected;

    private void Awake()
    {
        if (isInEditor)
        {
            for (int i = 0; i < AtlasItems.Count; i++)
            {
                PlayerPrefs.DeleteKey("AtlasItem_" + i);
            }

            for (int i = 0; i < FileItems.Count; i++)
            {
                PlayerPrefs.DeleteKey("FileItem_" + i);
            }

            for (int i = 0; i < UseableItems.Count; i++)
            {
                PlayerPrefs.DeleteKey("UseableItem_" + i);
            }
        }
    }

    private void OnEnable()
    {
        Instance = this;
        itemPickup = new List<GameObject>();
        if (!isInEditor)
        {
            LoadInventory();
        }
    }

    private void OnDisable()
    {
        if (!isInEditor)
        {
            SaveInventory();
        }
        
    }

    void Update()
    {
        ReminderSlide();

        ExitPreviewPanel();

    }


    public void AddItem(ItemData_SO newItemData)
    {
        //itemCollected += 1;
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
                    if (newItemData.filePreviewSprite_1920_1080 != null){
                        StartCoroutine(ShowFilePreview(newItemData.filePreviewSprite_1920_1080));
                    }
                    
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
                    StartCoroutine(ShowItemPreview(newItemData.modelPrefab));

                    break;
                }
            }
        }

        if (newItemData.iconInDoor_135_135 != null){
            doorItemPages.GetComponent<PageList>().AddItemToDoor(newItemData.iconInDoor_135_135, newItemData.itemName, newItemData.relatedCheckWordNumber);
        }


    }

    public void AddItemWithoutReminder(ItemData_SO newItemData)
    {
        if (newItemData == null)
        {
            return;
        }
        //itemCollected += 1;

        if (newItemData.itemType == ItemType.Atlas)
        {
            for (int i = 0; i < AtlasItems.Count; i++)
            {
                if (AtlasItems[i].itemData == null)
                {
                    AtlasItems[i].itemData = newItemData;
                    AtlasContainer.GetComponent<AtlasContainerUI>().AddItem(newItemData);
                    if (newItemData.iconInDoor_135_135 != null)
                    {
                        doorItemPages.GetComponent<PageList>().AddItemToDoor(newItemData.iconInDoor_135_135, newItemData.itemName, newItemData.relatedCheckWordNumber);
                    }
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
                    if (newItemData.iconInDoor_135_135 != null)
                    {
                        doorItemPages.GetComponent<PageList>().AddItemToDoor(newItemData.iconInDoor_135_135, newItemData.itemName, newItemData.relatedCheckWordNumber);
                    }

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
                    if (newItemData.iconInDoor_135_135 != null)
                    {
                        doorItemPages.GetComponent<PageList>().AddItemToDoor(newItemData.iconInDoor_135_135, newItemData.itemName, newItemData.relatedCheckWordNumber);
                    }
                    break;
                }
            }
        }
        ItemPickup[] obj = GameObject.FindObjectsOfType<ItemPickup>();
        foreach (ItemPickup comp in obj)
        {
            if (comp.gameObject.name == newItemData.itemName)
            {
                Debug.Log(comp.gameObject);
                Destroy(comp.gameObject);
            }

        }
        obj = null;
    }

    private void SaveInventory()
    {
        for (int i = 0; i < AtlasItems.Count; i++)
        {
            SaveItem("AtlasItem_" + i, AtlasItems[i]);
        }

        for (int i = 0; i < FileItems.Count; i++)
        {
            SaveItem("FileItem_" + i, FileItems[i]);
        }

        for (int i = 0; i < UseableItems.Count; i++)
        {
            SaveItem("UseableItem_" + i, UseableItems[i]);
        }
        PlayerPrefs.Save();
    }

    private void SaveItem(string key, InventoryItem item)
    {
        if (item != null && item.itemData != null)
        {
            PlayerPrefs.SetString(key, item.itemData.itemName);
        }
        else
        {
            PlayerPrefs.DeleteKey(key);
        }
    }


    private void LoadInventory()
    {
        for (int i = 0; i < AtlasItems.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("AtlasItem_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                ItemData_SO itemData = ItemData_SO.LoadItemDataByName(itemName);
                AddItemWithoutReminder(itemData);
            }
        }

        for (int i = 0; i < FileItems.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("FileItem_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                ItemData_SO itemData = ItemData_SO.LoadItemDataByName(itemName);
                AddItemWithoutReminder(itemData);
            }
        }

        for (int i = 0; i < UseableItems.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("UseableItem_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                ItemData_SO itemData = ItemData_SO.LoadItemDataByName(itemName);
                AddItemWithoutReminder(itemData);
            }
        }
    }


    public void ShowReminder(string title, string type)
    {
        ReminderName.text = title;
        ReminderType.text = type;

        timer = 420;
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

    private void ExitPreviewPanel()
    {
        if (itemPanel.activeInHierarchy && Input.GetKey(KeyCode.Escape))
        {
            itemPanel.SetActive(false);
            itemPrefab.gameObject.transform.parent = previewModel.transform;
            itemPrefab.gameObject.transform.position = new Vector3(10000, 10000, 10000);
            Destroy(itemPrefab.gameObject);
            itemPrefab = null;
            itemPanel.GetComponent<ItemViewer>().itemModel = null;
           

            crosshairCanvas.SetActive(true);
            GameManager.Instance.inventoryEnabled = true;
            PlayerMovement.Instance.moveDisabled = false;
            mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
            Cursor.visible = false;
            StartCoroutine(WaitForPauseMenu());
        }

        if (filePanel.activeInHierarchy && Input.GetKey(KeyCode.Escape))
        {
            filePanel.SetActive(false);
            fileImage.sprite = null;

            crosshairCanvas.SetActive(true);
            GameManager.Instance.inventoryEnabled = true;
            PlayerMovement.Instance.moveDisabled = false;
            mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
            Cursor.visible = false;
            StartCoroutine(WaitForPauseMenu());
        }
    }


    IEnumerator ShowItemPreview(Transform modelPrefab)
    {
        pauseMenuEnabled = false;
        GameManager.Instance.inventoryEnabled = false;
        PlayerMovement.Instance.moveDisabled = true;
        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        itemPanel.SetActive(true);
        crosshairCanvas.SetActive(false);


        Cursor.visible = true;
        

        if (previewModel.transform.childCount > 0)
        {
            Destroy(previewModel.transform.GetChild(0).gameObject);
            itemPanel.GetComponent<ItemViewer>().itemModel = null;
            itemPrefab = null;
        }
        
        itemPrefab = Instantiate(modelPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity);
        itemPrefab.gameObject.transform.parent = previewModel.transform;
        itemPanel.GetComponent<ItemViewer>().itemModel = itemPrefab;
    }


    IEnumerator ShowFilePreview(Sprite fileSprite)
    {
        pauseMenuEnabled = false;
        GameManager.Instance.inventoryEnabled = false;
        PlayerMovement.Instance.moveDisabled = true;
        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        filePanel.SetActive(true);
        crosshairCanvas.SetActive(false);


        Cursor.visible = true;

        fileImage.sprite = fileSprite;
    }


    IEnumerator WaitForPauseMenu()
    {
        yield return new WaitForSeconds(1f);
        pauseMenuEnabled = true;
    }

}



[System.Serializable]

public class InventoryItem
{
    public ItemData_SO itemData;
}
