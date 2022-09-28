using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject InventoryCanvas;

    void Start()
    {
        
    }

    
    void Update()
    {
        InventoryVisibility();
    }


    public void InventoryVisibility()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryCanvas.activeInHierarchy)
            {
                InventoryCanvas.SetActive(false);
            }
            else
            {
                InventoryCanvas.SetActive(true);
            }
        }
    }
}
