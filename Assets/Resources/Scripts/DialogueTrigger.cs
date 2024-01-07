using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)] public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue secondlDialogue;
    public Dialogue thirdDialogue;
    public Dialogue guiltyDialogue;
    private static bool firstDialoguePlayedMaid = false;
    private static bool allDirtRemoved = false;
    
    private static bool firstDialoguePlayedManager = false;

    public void TriggerDialogue()
    {
        if (Globals.LeftoverTrash == 0)
        {
            allDirtRemoved = true;
        }
        
        if (firstDialoguePlayedMaid && name == "Maid" && !allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(secondlDialogue);
        }
        else if (firstDialoguePlayedMaid && name == "Maid" && allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(thirdDialogue);
        }
        else if(firstDialoguePlayedManager && name == "Manager" && !allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(secondlDialogue);
        }
        else if (firstDialoguePlayedManager && name == "Manager" && allDirtRemoved)
        {
            Globals.showGuiltDialogue = true;
            DialogueManager.Instance.StartDialogue(guiltyDialogue);
        }
        else
        {
            if (name == "Maid")
            {
                firstDialoguePlayedMaid = true;
            } else if (name == "Manager")
            {
                firstDialoguePlayedManager = true;
            }
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
        }
    }
}
