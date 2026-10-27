using UnityEngine;
using UnityEngine.UI;
using System;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public ItemData itemData;
    public static event Action<ItemData> OnItemClicked;

    public void DrawSlot(InventoryItem item)
    {
        if (item == null)
            return;

        itemData = item.itemData;

        icon.sprite = itemData.icon;
        icon.enabled = true;
    }
    public void OnSlotClicked()
    {
        if (itemData == null)
            return;
       
        Debug.Log($"Clicked on slot with item: {itemData.itemName}");

        OnItemClicked?.Invoke(itemData);
    }
}