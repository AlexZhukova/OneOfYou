using UnityEngine;
using UnityEngine.UI;
using System;

public class BodyPartQTEButton : MonoBehaviour
{
    [SerializeField] private Image icon;

    private ItemData itemData;
    private Action<ItemData> onClicked;

    public void Setup(ItemData item, Action<ItemData> clickAction)
    {
        itemData = item;
        onClicked = clickAction;

        icon.sprite = item.icon;

        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        onClicked?.Invoke(itemData);
    }
}