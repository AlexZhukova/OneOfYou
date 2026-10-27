using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI monsterText;
    [SerializeField] private Slider timerSlider;

    [Header("Settings")]
    [SerializeField] private float dialogueTime = 10f;

    private float timeRemaining;
    private GuardAI currentGuard;
    private bool dialogueActive;
    private PlayerMovement playerMovement;
    private PlayerController playerController;
    private Transform playerTransform;
    [SerializeField] private float turnSpeed = 5f;

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

        monsterText.text = "There's something strange about you...";

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
}