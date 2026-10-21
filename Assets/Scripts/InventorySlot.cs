using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public ItemData itemData;

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
        if (itemData != null)
            return;
       
        Debug.Log($"Clicked on slot with item: {itemData.itemName}");
        foreach (var guard in GuardAI.guards)
        {
            if (guard.itemData == itemData)
            {
                guard.playerDead = false;
            }
        }
    }
}