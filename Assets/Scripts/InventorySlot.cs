using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public void DrawSlot(InventoryItem item)
    {
        if (item == null)
        {
            return;
        }
 
        icon.sprite = item.itemData.icon;
        icon.enabled = true;
    }
}
