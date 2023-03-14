using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Atlas, File, Useable}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]

public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;

    public string itemName;

    public Sprite itemIcon_1100_700;

    public Sprite iconInDoor_135_135;

    public Sprite filePreviewSprite_1920_1080;

    public int relatedCheckWordNumber = -1;

    [TextArea(10, 200)]
    public string description = "";

    public Transform modelPrefab;

    
}
