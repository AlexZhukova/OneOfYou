
using UnityEngine;

public class FloatingQTEItem : MonoBehaviour
{
    [SerializeField] private float minSpeed = 15f;
    [SerializeField] private float maxSpeed = 35f;
    [SerializeField] private float padding = 45f;

    [Header("Avoid Overlapping")]
    [SerializeField] private float separationDistance = 85f;
    [SerializeField] private float separationStrength = 120f;

    private RectTransform rect;
    private RectTransform parentRect;
    private Vector2 targetPosition;
    private float speed;
    private bool isDragging;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent as RectTransform;

        speed = Random.Range(minSpeed, maxSpeed);

        ChooseNewTarget();
    }

    private void Update()
    {
        if (isDragging || parentRect == null)
            return;

        rect.anchoredPosition = Vector2.MoveTowards(
            rect.anchoredPosition,
            targetPosition,
            speed * Time.unscaledDeltaTime
        );
        AvoidOtherItems();

        if (Vector2.Distance(rect.anchoredPosition, targetPosition) < 2f)
        {
            ChooseNewTarget();
        }
    }

    private void ChooseNewTarget()
    {
        float halfWidth = parentRect.rect.width / 2f;
        float halfHeight = parentRect.rect.height / 2f;

        targetPosition = new Vector2(
            Random.Range(-halfWidth + padding, halfWidth - padding),
            Random.Range(-halfHeight + padding, halfHeight - padding)
        );
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;

        if (!dragging && parentRect != null)
        {
            ChooseNewTarget();
        }
    }

    private void AvoidOtherItems()
    {
        if (parentRect == null)
            return;

        Vector2 avoidance = Vector2.zero;

        foreach (Transform child in parentRect)
        {
            if (child == transform)
                continue;

            FloatingQTEItem other = child.GetComponent<FloatingQTEItem>();

            if (other == null)
                continue;

            RectTransform otherRect = other.GetComponent<RectTransform>();

            Vector2 difference =
                rect.anchoredPosition - otherRect.anchoredPosition;

            float distance = difference.magnitude;

            if (distance < separationDistance)
            {
                if (distance < 0.001f)
                    difference = Random.insideUnitCircle.normalized;
                else
                    difference /= distance;

                float strength = 1f - distance / separationDistance;

                avoidance += difference * strength;
            }
        }

        rect.anchoredPosition +=
            avoidance * separationStrength * Time.unscaledDeltaTime;

        float halfWidth = parentRect.rect.width / 2f;
        float halfHeight = parentRect.rect.height / 2f;

        Vector2 position = rect.anchoredPosition;

        position.x = Mathf.Clamp(
            position.x,
            -halfWidth + padding,
            halfWidth - padding
        );

        position.y = Mathf.Clamp(
            position.y,
            -halfHeight + padding,
            halfHeight - padding
        );

        rect.anchoredPosition = position;
    }

}
