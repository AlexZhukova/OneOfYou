using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();

    [Header("After successful QTE")]
    [TextArea]
    public string successMonsterText = "Oh... You're fine. See you around.";

    public string successPlayerAnswer = "See you.";
}