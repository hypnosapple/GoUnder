using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;

    public void Pickup()
    {
        InventoryManager.Instance.inventoryData.AddItem(itemData);

        Destroy(gameObject);
    }
}
