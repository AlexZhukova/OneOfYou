using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BodyRegion
{
    Head,
    Torso,
    Legs
}


[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Header("Body Region")]
    public BodyRegion bodyRegion;
}

