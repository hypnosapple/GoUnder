using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange;
    int pickupLayerMask;

    public GameObject onTarget;

    [SerializeField] private Camera cam;

    public GameObject InventoryCanvas;
    public ItemData_SO Code1Data;
    public GameObject Code1Page;
    private bool hasCode1 = false;

    void Start()
    {
        pickupLayerMask = LayerMask.GetMask("Pickup");
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
