using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public int itemsInThisPage;

    private void Awake()
    {
        itemsInThisPage = 0;
    }
}
