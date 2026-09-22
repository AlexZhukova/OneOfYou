using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float crouchRunSpeed = 5f;

    public float lookSpeed = 2f;
    private float pitch = 0f;

    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;
    private bool isCrouching = false;

    private CharacterController characterController;
    public Camera playerCamera;

    private bool isRunning = false;

    private Vector3 playerVelocity;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f; // Jump force

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("No CharacterController component found on the player!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }

        isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentMoveSpeed = moveSpeed;
        if (isCrouching)
        {
            currentMoveSpeed = isRunning ? crouchRunSpeed : crouchSpeed;
        }
        else if (isRunning)
        {
            currentMoveSpeed = runSpeed;
        }

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        //Debug.Log($"Movement Direction: {moveDirection}");

        if (characterController.isGrounded)
        {
            playerVelocity.y = 0f;
            if (Input.GetKeyDown(KeyCode.Space)) // Jump input
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move((moveDirection * currentMoveSpeed + playerVelocity) * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, mouseX * lookSpeed, 0);

        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);


    }

    private void ToggleCrouch()
    {
        if (isCrouching)
        {
            characterController.height = standingHeight;
            // playerCamera.transform.localPosition = new Vector3(0, standingHeight / 2, 0);
        }
        else
        {
            characterController.height = crouchHeight;
            //playerCamera.transform.localPosition = new Vector3(0, crouchHeight / 2, 0);
        }

        isCrouching = !isCrouching;
    }
}
