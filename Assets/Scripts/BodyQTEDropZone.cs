
using UnityEngine;
using UnityEngine.EventSystems;

public enum BodyZone
{
    Head,
    Torso
}

public class BodyQTEDropZone : MonoBehaviour, IDropHandler
{
    [Header("Body Zone")]
    [SerializeField] private BodyZone zone;
    [SerializeField] private BodyQTEManager bodyQTEManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DraggableQTEItem item =
            eventData.pointerDrag.GetComponent<DraggableQTEItem>();

        if (item == null || item.ItemData == null)
            return;

        bodyQTEManager.CheckDroppedItem(item.ItemData, zone);
    }
}
