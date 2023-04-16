using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public InventoryManager inventoryManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.LoadInventory();
            Debug.Log("Checkpoint reached, inventory loaded.");
        }
    }
}