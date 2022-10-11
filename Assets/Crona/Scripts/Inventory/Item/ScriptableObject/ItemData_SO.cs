using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Atlas, File, Useable}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]

public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;

    public string itemName;

    public Sprite itemIcon;

    public string description = "";

    public Transform modelPrefab;
}
