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

    public SubtitleData_SO firstSub;
    

    public Camera mainCam;
    public GameObject CMStart;
    public GameObject CMStart2;
    public GameObject CMStart3;

    void Start()
    {
        player.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<SubtitleManager>().ShowSubtitle(firstSub);
        //EnableMove();
        
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

    public void EnableMove()
    {
        player.GetComponent<CharacterController>().enabled = true;
        StartCoroutine(SwitchCam());
    }

    IEnumerator SwitchCam()
    {
        CMStart.SetActive(false);
        yield return new WaitForSeconds(6f);
        CMStart2.SetActive(false);
        yield return new WaitForSeconds(5f);
        CMStart3.SetActive(false);
    }
}
