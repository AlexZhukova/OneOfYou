using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private void OnEnable()
    {
        Inventory.OnInventoryChanged += DrawInventory;
    }
    private void OnDisable()
    {
        Inventory.OnInventoryChanged -= DrawInventory;
    }
    private void ResetInventory()
    {
        foreach (Transform child in transform)
        {
            Destroy (child.gameObject);
        }
        inventorySlots.Clear();
    }

    private void DrawInventory(List<InventoryItem> inventory)
    {
        ResetInventory();

        foreach (InventoryItem item in inventory)
        {
            CreateInventorySlot(item);
        }
    }

    private void CreateInventorySlot(InventoryItem item)
    {
        GameObject newSlot = Instantiate(slotPrefab, transform);

        InventorySlot slot = newSlot.GetComponent<InventorySlot>();

        slot.DrawSlot(item);

        inventorySlots.Add(slot);
    }
}
