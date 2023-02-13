using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange;
    public float screenRange;
    public float interactionRange;
    int pickupLayerMask;
    int screen1LayerMask;
    int screen2LayerMask;
    int doorLayerMask;

    public GameObject onTarget;
    public GameManager gameManager;

    [SerializeField] private Camera cam;

    public GameObject InventoryCanvas;
    public ItemData_SO Code1Data;
    public GameObject Code1Page;
    private bool hasCode1 = false;

    public Animator door31Animator;
    public Animator door32Animator;
    public Animator door33Animator;

    public AudioSource playerAudio;
    public AudioClip doorLockSFX;
    public AudioClip doorOpenSFX;

    public SubtitleData_SO Call301;
    public SubtitleData_SO Call302;
    public SubtitleData_SO Call303;

    void Start()
    {
        pickupLayerMask = LayerMask.GetMask("Pickup");
        screen1LayerMask = LayerMask.GetMask("Screen1");
        screen2LayerMask = LayerMask.GetMask("Screen2");
        doorLayerMask = LayerMask.GetMask("Door");
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

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
            if (hit.transform.gameObject.GetComponent<DoorInteract>().opened == false)
            {
                if (!onTarget.activeInHierarchy)
                {
                    onTarget.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.transform.gameObject.GetComponent<DoorInteract>().unlocked)
                    {
                        if (hit.transform.gameObject.name == "Door3.1Pivot")
                        {
                            door31Animator.SetBool("Door3.1Open", true);
                            StartCoroutine(WaitForPhoneCall(Call301));
                        }
                        else if (hit.transform.gameObject.name == "Door3.2Pivot")
                        {
                            door32Animator.SetBool("Door3.2Open", true);
                            StartCoroutine(WaitForPhoneCall(Call302));
                        }
                        else if (hit.transform.gameObject.name == "Door3.3Pivot")
                        {
                            door33Animator.SetBool("Door3.3Open", true);
                            StartCoroutine(WaitForPhoneCall(Call303));
                        }


                        
                        hit.transform.gameObject.GetComponent<DoorInteract>().opened = true;
                    }

                    hit.transform.gameObject.GetComponent<AudioSource>().Play();
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

    IEnumerator WaitForPhoneCall(SubtitleData_SO phoneCall)
    {
        yield return new WaitForSeconds(3f);
        gameManager.GetComponent<SubtitleManager>().ShowSubtitle(phoneCall);
    }
}
