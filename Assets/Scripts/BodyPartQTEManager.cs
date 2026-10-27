using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartQTEManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private Transform bodyPartsContainer;

    [Header("Prefab")]
    [SerializeField] private BodyPartQTEButton BodyPartQTEButtonPrefab;

    [Header("QTE Timer")]
    [SerializeField] private Slider qteTimerSlider;
    [SerializeField] private float qteTime = 3f;

    private float timeRemaining;
    private bool qteActive;

    private ItemData correctItem;
    private DialogueManager dialogueManager;

    private void Update()
    {
        if (!qteActive)
            return;

        timeRemaining -= Time.unscaledDeltaTime;
        qteTimerSlider.value = timeRemaining;

        if (timeRemaining <= 0f)
        {
            qteActive = false;

            Debug.Log("QTE FAILED! Time is over!");

            qtePanel.SetActive(false);
            dialogueManager.QTEFailed();
        }
    }

    public void StartTestQTE(ItemData correctItem, DialogueManager dialogueManager)
    {
        this.correctItem = correctItem;
        this.dialogueManager = dialogueManager;

        qtePanel.SetActive(true);

        qteActive = true;
        timeRemaining = qteTime;

        qteTimerSlider.minValue = 0f;
        qteTimerSlider.maxValue = qteTime;
        qteTimerSlider.value = qteTime;

        List<Vector2> usedPositions = new List<Vector2>();

        foreach (InventoryItem inventoryItem in Inventory.Instance.inventory)
        {
            BodyPartQTEButton newButton =
                Instantiate(BodyPartQTEButtonPrefab, bodyPartsContainer);

            newButton.Setup(inventoryItem.itemData, OnBodyPartClicked);

            RectTransform buttonRect = newButton.GetComponent<RectTransform>();
            RectTransform containerRect = bodyPartsContainer.GetComponent<RectTransform>();

            float halfWidth = containerRect.rect.width / 2f;
            float halfHeight = containerRect.rect.height / 2f;

            Vector2 newPosition = Vector2.zero;

            float minimumDistance = 100f;

            for (int attempt = 0; attempt < 50; attempt++)
            {
                float x = Random.Range(-halfWidth + 50f, halfWidth - 50f);
                float y = Random.Range(-halfHeight + 50f, halfHeight - 50f);

                Vector2 testPosition = new Vector2(x, y);

                bool positionIsFree = true;

                foreach (Vector2 usedPosition in usedPositions)
                {
                    if (Vector2.Distance(testPosition, usedPosition) < minimumDistance)
                    {
                        positionIsFree = false;
                        break;
                    }
                }

                if (positionIsFree)
                {
                    newPosition = testPosition;
                    break;
                }
            }

            buttonRect.anchoredPosition = newPosition;
            usedPositions.Add(newPosition);
        }
    }
    private void OnBodyPartClicked(ItemData selectedItem)
    {
        if (selectedItem == correctItem)
        {
            qteActive = false;

            Debug.Log("QTE SUCCESS! Correct body part: " + selectedItem.itemName);

            qtePanel.SetActive(false);
            dialogueManager.QTESucceeded();
        }
        else
        {
            qteActive = false;

            Debug.Log("QTE FAILED! Wrong body part: " + selectedItem.itemName);

            qtePanel.SetActive(false);
            dialogueManager.QTEFailed();
        }
    }
}
