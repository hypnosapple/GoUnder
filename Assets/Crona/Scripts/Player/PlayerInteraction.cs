using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float pickupRange;
    public float screenRange;
    public float interactionRange;
    int pickupLayerMask;
    int screen1LayerMask;
    int screen2LayerMask;
    int doorLayerMask;
    int drawerLayerMask;

    public GameObject onTarget;
    public GameManager gameManager;

    [SerializeField] private Camera cam;

    public GameObject InventoryCanvas;
    public ItemData_SO Code1Data;
    public GameObject Code1Page;
    private bool hasCode1 = false;

    public bool interactAllowed;


    void Start()
    {
        pickupLayerMask = LayerMask.GetMask("Pickup");
        screen1LayerMask = LayerMask.GetMask("Screen1");
        screen2LayerMask = LayerMask.GetMask("Screen2");
        doorLayerMask = LayerMask.GetMask("Door");
        drawerLayerMask = LayerMask.GetMask("Drawer");

        interactAllowed = true;
}

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (interactAllowed)
        {
            if (Physics.Raycast(ray, out hit, pickupRange, pickupLayerMask))
            {
                if (!onTarget.activeInHierarchy)
                {
                    onTarget.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<ItemPickup>().Pickup();
                }
            }

            else if (Physics.Raycast(ray, out hit, screenRange, screen1LayerMask))
            {
                if (!onTarget.activeInHierarchy)
                {
                    onTarget.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<UIInteract>().ToComputer();
                }
            }

            else if (Physics.Raycast(ray, out hit, screenRange, screen2LayerMask))
            {
                if (!onTarget.activeInHierarchy)
                {
                    onTarget.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<CommunicationInteract>().ToScreen();
                }
            }

            else if (Physics.Raycast(ray, out hit, interactionRange, doorLayerMask))
            {
                if (!hit.transform.gameObject.GetComponent<DoorInteract>().opened)
                {
                    if (!onTarget.activeInHierarchy)
                    {
                        onTarget.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.gameObject.GetComponent<DoorInteract>().PlayOpenDoor();

                    }
                }

            }

            else if (Physics.Raycast(ray, out hit, interactionRange, drawerLayerMask))
            {
                if (!hit.transform.gameObject.GetComponent<DrawerInteract>().isMoving)
                {
                    if (!onTarget.activeInHierarchy)
                    {
                        onTarget.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.gameObject.GetComponent<DrawerInteract>().MoveDrawer();
                    }
                }

            }

            else
            {
                if (onTarget.activeInHierarchy)
                {
                    onTarget.SetActive(false);
                }
            }
        }
        else
        {
            if (onTarget.activeInHierarchy)
            {
                onTarget.SetActive(false);
            }
        }
        


        if (!hasCode1 && Code1Page.activeInHierarchy)
        {
            InventoryCanvas.GetComponent<InventoryManager>().AddItem(Code1Data);
            hasCode1 = true;
        }
    }


}
