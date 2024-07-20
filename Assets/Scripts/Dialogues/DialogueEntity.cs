using UnityEngine;
using System;
using System.Collections;

public abstract class DialogueEntity : MonoBehaviour
{
    public const string RoleVariable = "role";
    public const string PlayerNumberVariable = "playerNumber";

    public static event Action<string, DialogueEntity> OnWantsToSay;

    public abstract string DialogueName { get; protected set; }

    private void SayLine(string line) => OnWantsToSay.Invoke(line, this);

    public IEnumerator SayDialogueLineCoroutine(string line)
    {
        SayLine(line);
        yield return new WaitWhile(() => DialoguePanel.Instance.SentenceIsTyping);
    }

    public string GetRandomDialogueLine(string[] dialogueLines)
    {
        int randomIndex = UnityEngine.Random.Range(0, dialogueLines.Length);
        return dialogueLines[randomIndex];
    }

    public string TryToReplaceVariable(string dialogueLine, string variableName, string replacement)
    {
        dialogueLine = dialogueLine.Replace($"{{{variableName}}}", replacement);
        return dialogueLine;
    }
}
