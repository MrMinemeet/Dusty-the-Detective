using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrashcanController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource audioSource;

    public Dialogue greetingDialogue;
    public Dialogue hasGlueDialogue;
    public Dialogue hasVomitDialogue;
    public Dialogue hasWineDialogue;
    public Dialogue allCollectedDialogue;

    void Start()
    {
        _animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Don't do anything if game is paused
        if (PauseMenu.IsGamePaused) return;

        //_animator.SetBool("isEating", true);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _animator.SetBool("isTalking", true);
            TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _animator.SetBool("isTalking", false);
        }
    }

    private void TriggerDialogue()
    {
        if (Globals.glueStatus == TrashStatus.DISPOSED &&
            Globals.vomitStatus == TrashStatus.DISPOSED &&
            Globals.wineStatus == TrashStatus.DISPOSED)
        {
            DialogueManager.Instance.StartDialogue(allCollectedDialogue, audioSource);
            return;
        }

        if (Globals.glueStatus != TrashStatus.COLLECTED && 
            Globals.vomitStatus != TrashStatus.COLLECTED && 
            Globals.wineStatus != TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(greetingDialogue, audioSource);
            return;
        }

        var combinedDialogue = new List<Dialogue>();

        if(Globals.glueStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasGlueDialogue);
            Globals.glueStatus = TrashStatus.DISPOSED;
        }
        if (Globals.vomitStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasVomitDialogue);
            Globals.vomitStatus = TrashStatus.DISPOSED;
        }
        if (Globals.wineStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasWineDialogue);
            Globals.wineStatus = TrashStatus.DISPOSED;
        }

        if (Globals.glueStatus == TrashStatus.DISPOSED &&
            Globals.vomitStatus == TrashStatus.DISPOSED &&
            Globals.wineStatus == TrashStatus.DISPOSED)
        {
            combinedDialogue.Add(allCollectedDialogue);
        }
        DialogueManager.Instance.StartDialogue(new Dialogue() { 
            dialogueLines = combinedDialogue
            .SelectMany(dialogue => dialogue.dialogueLines)
            .ToList() }, audioSource);
    }
}
