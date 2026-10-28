using UnityEngine;
using UnityEngine.UI;
using System;

public class BodyPartQTEButton : MonoBehaviour
{
    [Header("Float")]
    [SerializeField] private float floatAmount = 50f;
    [SerializeField] private float floatSpeed = 3f;

    private float randomOffsetX;
    private float randomOffsetY;

    [Header("Shake")]
    [SerializeField] private float shakeAmount = 5f;

    private RectTransform rectTransform;
    private Vector2 basePosition;

    [SerializeField] private Image icon;

    private ItemData itemData;
    private Action<ItemData> onClicked;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        basePosition = rectTransform.anchoredPosition;

        randomOffsetX = UnityEngine.Random.Range(0f, 100f);
        randomOffsetY = UnityEngine.Random.Range(100f, 200f);
    }

    private void Update()
    {
        if (rectTransform == null)
            return;

        float floatX =
            (Mathf.PerlinNoise(randomOffsetX, Time.unscaledTime * floatSpeed) - 0.5f)
            * 2f * floatAmount;

        float floatY =
            (Mathf.PerlinNoise(randomOffsetY, Time.unscaledTime * floatSpeed) - 0.5f)
            * 2f * floatAmount;

        float shakeX = UnityEngine.Random.Range(-shakeAmount, shakeAmount);
        float shakeY = UnityEngine.Random.Range(-shakeAmount, shakeAmount);

        rectTransform.anchoredPosition =
            basePosition +
            new Vector2(floatX + shakeX, floatY + shakeY);
    }

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