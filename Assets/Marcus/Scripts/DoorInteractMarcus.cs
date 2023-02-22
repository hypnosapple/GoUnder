using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractMarcus : MonoBehaviour
{
    public float raycastLength = 5.0f;

    private bool cameraLocked = false;

    public PlayerMovement playermovement;


    private void Start()
    {
        playermovement = FindObjectOfType<PlayerMovement>();

  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raycastLength))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    // Do something when the raycast hits an object with the "Door" tag
                    Debug.Log("Hit a door!");

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    if (!cameraLocked)
                    {
                        cameraLocked = true;
                        playermovement.islocked = true;
                        GameObject uiMarcus = GameObject.Find("UI - Marcus");
                        Transform doorUI = uiMarcus.transform.Find("Door UI");

                        doorUI.gameObject.SetActive(true);
                    }

                    else
                    {
                        cameraLocked = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;

                        playermovement.islocked = false;

                        GameObject uiMarcus = GameObject.Find("UI - Marcus");
                        Transform doorUI = uiMarcus.transform.Find("Door UI");

                        doorUI.gameObject.SetActive(false);


                    }
                }
            }
        }
    }
}