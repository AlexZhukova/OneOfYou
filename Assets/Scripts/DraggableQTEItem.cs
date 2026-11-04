
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableQTEItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Settings")]
    [SerializeField] private FaceZone correctZone;
    private ItemData itemData;

    private FloatingQTEItem floatingItem;
    public ItemData ItemData => itemData;
    public FaceZone CorrectZone => correctZone;
    public void Setup(ItemData data, FaceZone zone)
    {
        itemData = data;
        correctZone = zone;
    }
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private Transform originalParent;
    private Vector2 originalPosition;

    private void Awake()
    {
        floatingItem = GetComponent<FloatingQTEItem>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (floatingItem != null)
            floatingItem.SetDragging(true);
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        transform.SetParent(originalParent, false);
        if (floatingItem == null)
            rectTransform.anchoredPosition = originalPosition;
        if (floatingItem != null)
            floatingItem.SetDragging(false);
    }
}
