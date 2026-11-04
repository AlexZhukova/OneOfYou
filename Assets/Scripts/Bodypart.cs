using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class Bodypart : MonoBehaviour, ICollectible
{
    public static event BodypartCollectedHandler OnBodypartCollected;
    public delegate void BodypartCollectedHandler(ItemData itemData);
    public ItemData itemData;
    public UnityEvent onCollected;
    public void Collect()
    {
        Debug.Log("Bodypart collected!");
        Destroy(gameObject);
        OnBodypartCollected?.Invoke(itemData);
        onCollected?.Invoke();
    }
}
