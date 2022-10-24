using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject InventoryCanvas;
    public GameObject player;
    public GameObject CrosshairCanvas;
    public CommunicationInteract CommunicationSystem;
    public UIInteract ComputerSystem;

    public Camera mainCam;

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
                if (CommunicationSystem.FocusOnScreen == false && ComputerSystem.FocusOnScreen == false)
                {
                    Cursor.visible = false;
                    InventoryCanvas.SetActive(false);
                    CrosshairCanvas.SetActive(true);
                }
                else
                {
                    Cursor.visible = true;
                    InventoryCanvas.SetActive(false);
                    CrosshairCanvas.SetActive(true);
                }
            }
            else
            {
                Cursor.visible = true;
                InventoryCanvas.SetActive(true);
                CrosshairCanvas.SetActive(false);
            }
        }

        if (InventoryCanvas.activeInHierarchy)
        {
            if (player.GetComponent<PlayerMovement>().enabled)
            {
                player.GetComponent<PlayerMovement>().enabled = false;
            }

            if (mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
            {
                mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            }
        }
        else
        {
            if (!mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled)
            {
                mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
            }

            if (!player.GetComponent<PlayerMovement>().enabled)
            {
                player.GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }
}
