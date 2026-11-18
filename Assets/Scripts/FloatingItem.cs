
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 45f;

    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // Rotate around the vertical axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Smoothly move up and down
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.localPosition = startPosition + Vector3.up * offset;
    }
}
