using System.Collections;
using System.Collections.Generic;
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
        if (Globals.glueStatus == Globals.TrashStatus.DISPOSED &&
            Globals.vomitStatus == Globals.TrashStatus.DISPOSED &&
            Globals.wineStatus == Globals.TrashStatus.DISPOSED)
        {
            DialogueManager.Instance.StartDialogue(allCollectedDialogue, audioSource);
            return;
        }

        if (Globals.glueStatus != Globals.TrashStatus.COLLECTED && 
            Globals.vomitStatus != Globals.TrashStatus.COLLECTED && 
            Globals.wineStatus != Globals.TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(greetingDialogue, audioSource);
            return;
        }

        if(Globals.glueStatus == Globals.TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(hasGlueDialogue, audioSource);
            Globals.glueStatus = Globals.TrashStatus.DISPOSED;
        }
        if (Globals.vomitStatus == Globals.TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(hasVomitDialogue, audioSource);
            Globals.vomitStatus = Globals.TrashStatus.DISPOSED;
        }
        if (Globals.wineStatus == Globals.TrashStatus.COLLECTED)
        {
            DialogueManager.Instance.StartDialogue(hasWineDialogue, audioSource);
            Globals.wineStatus = Globals.TrashStatus.DISPOSED;
        }

        if (Globals.glueStatus == Globals.TrashStatus.DISPOSED &&
            Globals.vomitStatus == Globals.TrashStatus.DISPOSED &&
            Globals.wineStatus == Globals.TrashStatus.DISPOSED)
        {
            DialogueManager.Instance.StartDialogue(allCollectedDialogue, audioSource);
            return;
        }
    }
}
