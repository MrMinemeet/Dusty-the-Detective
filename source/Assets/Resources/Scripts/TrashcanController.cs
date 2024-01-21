using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrashcanController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;

    public Dialogue greetingDialogue;
    public Dialogue hasGlueDialogue;
    public Dialogue hasVomitDialogue;
    public Dialogue hasWineDialogue;
    public Dialogue allCollectedDialogue;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Don't do anything if game is paused
        if (PauseMenu.IsGamePaused) return;

        //_animator.SetBool("isEating", true);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.SetBool("isTalking", true);
            TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.SetBool("isTalking", false);
        }
    }

    private void TriggerDialogue()
    {
        if (Globals.GlueStatus == TrashStatus.DISPOSED &&
            Globals.VomitStatus == TrashStatus.DISPOSED &&
            Globals.WineStatus == TrashStatus.DISPOSED)
        {
            DialogueManager.Instance.StartDialogue(allCollectedDialogue, _audioSource);
            return;
        }

        if (Globals.GlueStatus != TrashStatus.COLLECTED && 
            Globals.VomitStatus != TrashStatus.COLLECTED && 
            Globals.WineStatus != TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(greetingDialogue, _audioSource);
            return;
        }

        var combinedDialogue = new List<Dialogue>();

        if(Globals.GlueStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasGlueDialogue);
            Globals.GlueStatus = TrashStatus.DISPOSED;
        }
        if (Globals.VomitStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasVomitDialogue);
            Globals.VomitStatus = TrashStatus.DISPOSED;
        }
        if (Globals.WineStatus == TrashStatus.COLLECTED)
        {
            combinedDialogue.Add(hasWineDialogue);
            Globals.WineStatus = TrashStatus.DISPOSED;
        }

        if (Globals.GlueStatus == TrashStatus.DISPOSED &&
            Globals.VomitStatus == TrashStatus.DISPOSED &&
            Globals.WineStatus == TrashStatus.DISPOSED)
        {
            combinedDialogue.Add(allCollectedDialogue);
        }
        DialogueManager.Instance.StartDialogue(new Dialogue() { 
            dialogueLines = combinedDialogue
            .SelectMany(dialogue => dialogue.dialogueLines)
            .ToList() }, _audioSource);
    }
}
