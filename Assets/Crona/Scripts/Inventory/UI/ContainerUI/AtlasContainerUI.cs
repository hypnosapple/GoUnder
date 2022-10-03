using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlasContainerUI : MonoBehaviour
{
    public GameObject AtlasItemPrefab;

    public List<GameObject> AtlasItems = new List<GameObject>();

    public void AddItem(ItemData_SO newItemData)
    {
        GameObject newAtlasItem = Instantiate(AtlasItemPrefab, this.transform);
        AtlasItems.Add(newAtlasItem);

        newAtlasItem.GetComponent<AtlasItemUI>().SetupItemUI(newItemData);
    }

    public void HideAll()
    {
        for (int i = 0; i < AtlasItems.Count; i++)
        {
            AtlasItems[i].GetComponent<AtlasItemUI>().HideItem();
        }
    }
}
