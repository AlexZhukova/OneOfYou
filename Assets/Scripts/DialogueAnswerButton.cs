using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueAnswerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI answerText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.5f, 0f);

    private void Start()
    {
        answerText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        answerText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        answerText.color = normalColor;
    }
}