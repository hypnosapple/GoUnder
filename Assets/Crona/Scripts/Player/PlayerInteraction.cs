using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;

    [Header("Range")]
    public float pickupRange;
    public float screenRange;
    public float interactionRange;

    int defaultLayerMask;
    int pickupLayerMask;
    int screen1LayerMask;
    int screen2LayerMask;
    int doorLayerMask;
    int drawerLayerMask;
    int closetDoorLayerMask;

    public GameObject onTarget;
    public GameManager gameManager;

    [SerializeField] private Camera cam;

    public GameObject InventoryCanvas;
    public ItemData_SO Code1Data;
    public GameObject Code1Page;
    private bool hasCode1 = false;

    public bool interactAllowed;

    public GameObject crosshairCanvas;


    void Start()
    {
        Instance = this;

        defaultLayerMask = 0;
        pickupLayerMask = 8;
        screen1LayerMask = 9;
        screen2LayerMask = 10;
        doorLayerMask = 11;
        drawerLayerMask = 12;
        closetDoorLayerMask = 14;
        
        interactAllowed = true;
}

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (interactAllowed && crosshairCanvas.activeInHierarchy)
        {
            if (Physics.Raycast(ray, out hit, interactionRange)){
                //Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == defaultLayerMask)
                {
                    if (onTarget.activeInHierarchy)
                    {
                        onTarget.SetActive(false);
                    }
                }

                    if (hit.collider.gameObject.layer == screen1LayerMask)
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

                else if (hit.collider.gameObject.layer == screen2LayerMask)
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

                else if (hit.collider.gameObject.layer == doorLayerMask)
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

                else if (hit.collider.gameObject.layer == drawerLayerMask)
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

                else if (hit.collider.gameObject.layer == pickupLayerMask)
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

                else if (hit.collider.gameObject.layer == closetDoorLayerMask)
                {
                    if (!hit.transform.gameObject.GetComponent<ClosetInteract>().isMoving)
                    {
                        if (!onTarget.activeInHierarchy)
                        {
                            onTarget.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hit.transform.gameObject.GetComponent<ClosetInteract>().openClosetDoor();
                        }
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
