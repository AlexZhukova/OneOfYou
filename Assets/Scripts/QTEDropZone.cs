
using UnityEngine;
using UnityEngine.EventSystems;

public enum FaceZone
{
    Forehead,
    Middle,
    Mouth
}

public class QTEDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private FaceQTEManager faceQTEManager;

    [Header("Face Zone")]
    [SerializeField] private FaceZone zone;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DraggableQTEItem item =
            eventData.pointerDrag.GetComponent<DraggableQTEItem>();

        if (item == null)
            return;

        faceQTEManager.CheckDroppedItem(
            item.ItemData,
            zone,
            item.CorrectZone
        );
    }
}
