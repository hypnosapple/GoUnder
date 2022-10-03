using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData_SO itemData;
    public GameObject InventoryCanvas;

    public void Pickup()
    {
        InventoryCanvas.GetComponent<InventoryManager>().AddItem(itemData);
        //Debug.Log("pickup");

        Destroy(gameObject);
    }
}
