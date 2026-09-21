using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;

    float gravity = -9.81f;
    float velocityY;

    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    private void Update()
    {
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        velocityY += gravity * Time.deltaTime;


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * z + cameraRight * x;

        if(moveDirection.magnitude > 0.1f)
        {
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }




        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }

}
