using System;
using System.Collections.Generic;

[Serializable]
public class DialogueNode
{
    public string monsterText;
    public List<DialogueAnswer> answers = new List<DialogueAnswer>();
}

[Serializable]
public class DialogueAnswer
{
    public string answerText;
    public int nextNodeIndex;
}