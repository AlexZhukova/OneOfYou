using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class InventoryItem 
{
    public ItemData itemData;

    public InventoryItem(ItemData data)
    {
        itemData = data;
    }
}
