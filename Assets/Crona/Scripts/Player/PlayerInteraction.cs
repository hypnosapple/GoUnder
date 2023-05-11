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
    int tunnelDoorLayerMask;
    int finalDoorLayerMask;
    int drawerLayerMask;
    int closetDoorLayerMask;
    int sinkLayerMask;
    int TVLayerMask;
    int tunnelTVLayerMask;
    int tunnelTV2LayerMask;

    [Header("UI")]
    public GameObject onTarget;
    public GameObject targetDisabled;

    [SerializeField] private Camera cam;

    public GameObject InventoryCanvas;
    public ItemData_SO Code1Data;
    public GameObject Code1Page;
    private bool hasCode1 = false;

    public bool interactAllowed;

    public GameObject crosshairCanvas;

    [Header("Audio")]
    public AudioSource pickupSFX;


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
        sinkLayerMask = 15;
        TVLayerMask = 16;
        tunnelDoorLayerMask = 18;
        tunnelTVLayerMask = 19;
        tunnelTV2LayerMask = 21;
        finalDoorLayerMask = 22;

        interactAllowed = true;
}

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (interactAllowed && crosshairCanvas.activeInHierarchy)
        {
            if (targetDisabled.activeInHierarchy)
            {
                targetDisabled.SetActive(false);
            }

            if (Physics.Raycast(ray, out hit, interactionRange)) {
                //Debug.Log(hit.collider.gameObject.layer);
                

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

                else if (hit.collider.gameObject.layer == tunnelDoorLayerMask)
                {
                    if (!hit.transform.gameObject.GetComponent<TunnelDoor>().opened)
                    {
                        if (!onTarget.activeInHierarchy)
                        {
                            onTarget.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hit.transform.gameObject.GetComponent<TunnelDoor>().PlayOpenDoor();

                        }
                    }

                }

                else if (hit.collider.gameObject.layer == finalDoorLayerMask)
                {
                    if (!hit.transform.gameObject.GetComponent<FinalDoor>().opened)
                    {
                        if (!onTarget.activeInHierarchy)
                        {
                            onTarget.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hit.transform.gameObject.GetComponent<FinalDoor>().PlayOpenDoor();

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
                        pickupSFX.Play();
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


                else if (hit.collider.gameObject.layer == sinkLayerMask)
                {
                    if (!onTarget.activeInHierarchy)
                    {
                        onTarget.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (!hit.transform.GetChild(0).gameObject.activeInHierarchy)
                        {
                            hit.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            hit.transform.GetChild(0).gameObject.SetActive(false);
                        }

                    }

                }

                else if (hit.collider.gameObject.layer == TVLayerMask)
                {
                    if (!onTarget.activeInHierarchy)
                    {
                        onTarget.SetActive(true);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.gameObject.GetComponent<TVInteract>().SwitchTV();
                    }
                }

                else if (hit.collider.gameObject.layer == tunnelTVLayerMask)
                {
                    if (SubtitleManager.Instance.Call03Ended)
                    {
                        if (!onTarget.activeInHierarchy)
                        {
                            onTarget.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            GameManager.Instance.ControlRendererFeature(0, false);
                            GetComponent<GameManager>().PlayVideo1();
                        }
                    }
                    
                }

                else if (hit.collider.gameObject.layer == tunnelTV2LayerMask)
                {
                    if (SubtitleManager.Instance.Call04Ended)
                    {
                        if (!onTarget.activeInHierarchy)
                        {
                            onTarget.SetActive(true);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            //GameManager.Instance.ControlRendererFeature(0, false);
                            GetComponent<GameManager>().PlayVideo2();
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
        }
        else if (!interactAllowed && crosshairCanvas.activeInHierarchy)
        {
            if (onTarget.activeInHierarchy)
            {
                onTarget.SetActive(false);
            }

            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                if (hit.collider.gameObject.layer == pickupLayerMask || hit.collider.gameObject.layer == screen1LayerMask || hit.collider.gameObject.layer == screen2LayerMask || hit.collider.gameObject.layer == doorLayerMask || hit.collider.gameObject.layer == drawerLayerMask || hit.collider.gameObject.layer == closetDoorLayerMask || hit.collider.gameObject.layer == sinkLayerMask || hit.collider.gameObject.layer == TVLayerMask)
                {
                    if (!targetDisabled.activeInHierarchy)
                    {
                        targetDisabled.SetActive(true);
                    }
                }

                else
                {
                    if (targetDisabled.activeInHierarchy)
                    {
                        targetDisabled.SetActive(false);
                    }
                }
            }

            else
            {
                if (targetDisabled.activeInHierarchy)
                {
                    targetDisabled.SetActive(false);
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
        

        if (Code1Page != null)
        {
            if (!hasCode1 && Code1Page.activeInHierarchy)
            {
                InventoryCanvas.GetComponent<InventoryManager>().AddItem(Code1Data);
                hasCode1 = true;
            }
        }
        
    }


}
