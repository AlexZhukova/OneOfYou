
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyQTEManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject bodyQTEPanel;
    [SerializeField] private RectTransform leftItemsPanel;
    [SerializeField] private RectTransform rightItemsPanel;

    [Header("Prefab")]
    [SerializeField] private DraggableQTEItem draggableItemPrefab;

    [Header("Timer")]
    [SerializeField] private Slider qteTimerSlider;
    [SerializeField] private float qteTime = 5f;

    private float timeRemaining;
    private bool qteActive;

    private ItemData requiredItem;
    private DialogueManager dialogueManager;

    private void Update()
    {
        if (!qteActive)
            return;

        timeRemaining -= Time.unscaledDeltaTime;
        qteTimerSlider.value = Mathf.Max(0f, timeRemaining);

        if (timeRemaining <= 0f)
            FinishQTE(false);
    }

    public void StartBodyQTE(
        ItemData item,
        DialogueManager manager)
    {
        requiredItem = item;
        dialogueManager = manager;

        ClearItems();

        bodyQTEPanel.SetActive(true);

        qteActive = true;
        timeRemaining = qteTime;

        qteTimerSlider.minValue = 0f;
        qteTimerSlider.maxValue = qteTime;
        qteTimerSlider.value = qteTime;

        SpawnItems();
    }

    private void SpawnItems()
    {
        List<InventoryItem> items =
            new List<InventoryItem>(Inventory.Instance.inventory);

        // Shuffle items
        for (int i = items.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            InventoryItem temp = items[i];
            items[i] = items[randomIndex];
            items[randomIndex] = temp;
        }

        List<Vector2> leftPositions = new List<Vector2>();
        List<Vector2> rightPositions = new List<Vector2>();

        for (int i = 0; i < items.Count; i++)
        {
            RectTransform parent =
                i % 2 == 0 ? leftItemsPanel : rightItemsPanel;

            DraggableQTEItem newItem =
                Instantiate(draggableItemPrefab, parent);

            ItemData data = items[i].itemData;

            Image image = newItem.GetComponent<Image>();
            image.sprite = data.icon;

            // Body QTE uses its own drop-zone validation.
            newItem.Setup(data, FaceZone.Middle);

            RectTransform rect =
                newItem.GetComponent<RectTransform>();

            List<Vector2> usedPositions =
                i % 2 == 0 ? leftPositions : rightPositions;

            float padding = 45f;
            float minimumDistance = 90f;

            float halfWidth = parent.rect.width / 2f;
            float halfHeight = parent.rect.height / 2f;

            Vector2 randomPosition = Vector2.zero;
            bool foundPosition = false;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                Vector2 candidate = new Vector2(
                    Random.Range(
                        -halfWidth + padding,
                        halfWidth - padding),
                    Random.Range(
                        -halfHeight + padding,
                        halfHeight - padding)
                );

                bool positionFree = true;

                foreach (Vector2 used in usedPositions)
                {
                    if (Vector2.Distance(candidate, used)
                        < minimumDistance)
                    {
                        positionFree = false;
                        break;
                    }
                }

                if (positionFree)
                {
                    randomPosition = candidate;
                    foundPosition = true;
                    break;
                }
            }

            if (!foundPosition)
                Debug.LogWarning(
                    "Not enough space for Body QTE items!");

            rect.anchoredPosition = randomPosition;
            usedPositions.Add(randomPosition);
        }
    }

    public void CheckDroppedItem(
        ItemData selectedItem,
        BodyZone droppedZone)
    {
        if (!qteActive)
            return;

        BodyZone correctZone = GetCorrectZone(requiredItem);

        bool success =
            selectedItem == requiredItem &&
            droppedZone == correctZone;

        FinishQTE(success);
    }

    private BodyZone GetCorrectZone(ItemData item)
    {
        string itemName = item.itemName.ToLower();

        if (itemName == "mask" || itemName == "cloak")
            return BodyZone.Head;

        if (itemName == "clothes" || itemName == "tail")
            return BodyZone.Torso;

        Debug.LogWarning("Unknown Body QTE item: " + item.itemName);
        return BodyZone.Torso;
    }

    private void FinishQTE(bool success)
    {
        if (!qteActive)
            return;

        qteActive = false;

        Debug.Log(success
            ? "Body QTE SUCCESS!"
            : "Body QTE FAILED!");

        bodyQTEPanel.SetActive(false);
        ClearItems();

        if (dialogueManager != null)
        {
            if (success)
                dialogueManager.QTESucceeded();
            else
                dialogueManager.QTEFailed();
        }
    }

    private void ClearItems()
    {
        foreach (Transform child in leftItemsPanel)
            Destroy(child.gameObject);

        foreach (Transform child in rightItemsPanel)
            Destroy(child.gameObject);
    }
}
