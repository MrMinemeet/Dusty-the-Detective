using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)] public string line;
    public AudioClip Audio;
}

[Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new();
}
public class DialogueTrigger : MonoBehaviour
{
    private bool _enterDialogue;
    
    public Dialogue dialogue;
    public Dialogue secondlDialogue;
    public Dialogue thirdDialogue;
    public Dialogue guiltyDialogue;
    private static bool _firstDialoguePlayedMaid;
    private static bool _allDirtRemoved;
    private AudioSource _audioSource;

    private static bool _firstDialoguePlayedManager;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (_enterDialogue && Input.GetKeyDown(KeyCode.E))
        {
            _enterDialogue = false;
            // Hide as interaction has been performed
            Globals.OnHideKeyHint.Invoke();
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (Globals.LeftoverTrash == 0)
        {
            _allDirtRemoved = true;
        }

        if (_firstDialoguePlayedMaid && name == "Maid" && !_allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(secondlDialogue, _audioSource);
        }
        else if (_firstDialoguePlayedMaid && name == "Maid" && _allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(thirdDialogue, _audioSource);
        }
        else if (_firstDialoguePlayedManager && name == "Manager" && !_allDirtRemoved)
        {
            DialogueManager.Instance.StartDialogue(secondlDialogue, _audioSource);
        }
        else if (_firstDialoguePlayedManager && name == "Manager" && _allDirtRemoved)
        {
            Globals.ShowGuiltDialogue = true;
            DialogueManager.Instance.StartDialogue(guiltyDialogue, _audioSource);
        }
        else
        {
            if (name == "Maid")
            {
                _firstDialoguePlayedMaid = true;
                Globals.AllowDirtPlacement = true;

            } else if (name == "Manager")
            {
                _firstDialoguePlayedManager = true;
            }

            DialogueManager.Instance.StartDialogue(dialogue, _audioSource);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _enterDialogue = true;
            
            // Invoke trigger to show Key Hint
            Globals.OnShowKeyHint.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _enterDialogue = false;
            
            // Invoke trigger to hide Key Hint
            Globals.OnHideKeyHint.Invoke();
        }
    }
}
