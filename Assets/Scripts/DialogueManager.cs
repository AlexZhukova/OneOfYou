using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI monsterText;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private GameObject answersPanel;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private BodyPartQTEManager bodyPartQTEManager;

    private DialogueData dialogueData;

    [Header("Settings")]
    [SerializeField] private float dialogueTime = 10f;

    private float timeRemaining;
    private GuardAI currentGuard;
    private bool dialogueActive;
    private PlayerMovement playerMovement;
    private PlayerController playerController;
    private Transform playerTransform;
    [SerializeField] private float turnSpeed = 5f;
    private void OnEnable()
    {
        InventorySlot.OnItemClicked += OnBodyPartSelected;
    }
    private void OnDisable()
    {
        InventorySlot.OnItemClicked -= OnBodyPartSelected;
    }
    private void Start()
    {
        dialoguePanel.SetActive(false);


        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerController = FindAnyObjectByType<PlayerController>();
        if(playerController != null )
        {
            playerTransform = playerController.transform;
        }
        timerSlider.minValue = 0f;
        //timerSlider.maxValue = dialogueTime;
        //timerSlider.value = dialogueTime;
    }


    private void Update()
    {
        if (!dialogueActive)
            return;

        timeRemaining -= Time.unscaledDeltaTime;

        timerSlider.value = timeRemaining;

        if (timeRemaining <= 0f)
        {
            DialogueFailed();
        }
    }


    public void StartDialogue(GuardAI guard)
    {
        currentGuard = guard;
        dialogueData = guard.DialogueData;

        StartCoroutine(SmoothTurnToGuard(guard.transform));

        if(playerMovement != null)
        {
            playerMovement.controlsLocked = true;
        }
        if (playerController != null)
        {
            playerController.controlsLocked = true;
        }

        dialogueActive = true;
        timeRemaining = dialogueTime;

        timerSlider.maxValue = dialogueTime;
        timerSlider.value = dialogueTime;

        dialoguePanel.SetActive(true);

        ShowNode(0);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }


    private void DialogueFailed()
    {
        dialogueActive = false;

        Debug.Log("Dialogue failed!");

        dialoguePanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentGuard.DialogueFailed();
    }

    private IEnumerator SmoothTurnToGuard(Transform guardTransform)
    {
        if (playerTransform == null || guardTransform == null)
            yield break;

        Vector3 directionToGuard = guardTransform.position - playerTransform.position;

        directionToGuard.y = 0f;

        if(directionToGuard == Vector3.zero)
            yield break;

        Quaternion startRotation = playerTransform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(directionToGuard);

        Quaternion startCameraRotation = playerController.playerCamera.transform.localRotation;
        Quaternion targetCameraRotation = Quaternion.identity;

        float progress = 0f;

        while(progress < 1f)
        {
            progress += Time.unscaledDeltaTime * turnSpeed;

            playerTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, progress);

            playerController.playerCamera.transform.localRotation = Quaternion.Slerp(startCameraRotation, targetCameraRotation, progress);
            yield return null;
        }
        playerTransform.rotation = targetRotation;
        playerController.playerCamera.transform.localRotation = targetCameraRotation;
        playerController.ResetCameraPitch();
    }

    private void ShowNode(int nodeIndex)
    {
        ClearAnswers();

        DialogueNode node = dialogueData.nodes[nodeIndex];

        monsterText.text = node.monsterText;

        timeRemaining = dialogueTime;
        timerSlider.value = dialogueTime;

        foreach (DialogueAnswer answer in node.answers)
        {
            int nextNodeIndex = answer.nextNodeIndex;
            CreateAnswer(answer.answerText, () => SelectAnswer(nextNodeIndex));
        }
    }

    private void SelectAnswer(int nextNodeIndex)
    {
        if (nextNodeIndex == -1)
        {
            StartBodyPartQTE();
            return;
        }

        ShowNode(nextNodeIndex);
    }
    private void ClearAnswers()
    {
        foreach (Transform child in answersPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void CreateAnswer(string text, UnityAction onClick)
    {
        GameObject newAnswer = Instantiate(answerPrefab, answersPanel.transform);

        DialogueAnswerButton answerButton = newAnswer.GetComponent<DialogueAnswerButton>();
        answerButton.SetText(text);

        Button button = newAnswer.GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }
    private void DialogueSucceeded()
    {
        dialogueActive = false;

        Debug.Log("Dialogue succeeded!");

        ClearAnswers();
        dialoguePanel.SetActive(false);

        Time.timeScale = 1f;

        if (playerMovement != null)
        {
            playerMovement.controlsLocked = false;
        }
        if (playerController != null)
        {
            playerController.controlsLocked = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentGuard.DialogueSucceeded();
    }
    private void OnBodyPartSelected(ItemData selectedItem)
    {
        if (!dialogueActive)
            return;

        Debug.Log("Selected body part during dialogue: " + selectedItem.itemName);

    }
    private void StartBodyPartQTE()
    {
        dialogueActive = false;

        ClearAnswers();
        dialoguePanel.SetActive(false);

        bodyPartQTEManager.StartTestQTE(currentGuard.itemData, this);
    }
    public void QTESucceeded()
    {
        Debug.Log("QTE returned SUCCESS to DialogueManager");

        dialogueActive = false;

        dialoguePanel.SetActive(true);

        ClearAnswers();

        monsterText.text = "Oh... You're fine. See you around.";

        timerSlider.value = 0f;

        CreateAnswer("See you.", DialogueSucceeded);
    }

    public void QTEFailed()
    {
        Debug.Log("QTE returned FAILED to DialogueManager");

        DialogueFailed();
    }
}