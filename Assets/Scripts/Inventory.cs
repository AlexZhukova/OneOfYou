using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    public static event Action<List<InventoryItem>> OnInventoryChanged;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private void OnEnable()
    {
        Bodypart.OnBodypartCollected += AddItem;
    }
    public void AddItem(ItemData itemData)
    {
        InventoryItem newItem = new InventoryItem(itemData);
        inventory.Add(newItem);
        OnInventoryChanged?.Invoke(inventory);
    }

}
