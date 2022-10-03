using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtlasItemUI : MonoBehaviour
{
    public Image picture = null;

    public Text description = null;

    public Text title = null;


    public void SetupItemUI(ItemData_SO item)
    {
        if (item != null)
        {
            picture.sprite = item.itemIcon;
            description.text = item.description;
            title.text = item.itemName;


        }
    }

    public void ShowItem()
    {
        picture.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        picture.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
    }
}
