using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueAnswerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI answerText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(1f, 0.5f, 0f);

    private string originalText;
    public void SetText(string text)
    {
        originalText = text;
        answerText.text = originalText;
    }
    private void Start()
    {
        answerText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        answerText.color = hoverColor;
        answerText.text = "> " + originalText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        answerText.color = normalColor;
        answerText.text = originalText;
    }

}