using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
 
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
 
    private Queue<DialogueLine> lines;
    
    public static bool IsDialogueActive { get; private set; }
 
    public float typingSpeed = 0.1f;
 
    public Animator animator;

    private static int guiltCounter = 0;

    public GameOverScreen GameOverScreen;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, AudioSource audioSource)
    {
        IsDialogueActive = true;
        if (Globals.showGuiltDialogue)
        {
            animator.Play("inGuilt");
        }
        else
        {
            animator.Play("in");
        }
        
 
        lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine(audioSource);
    }
 
    public void DisplayNextDialogueLine(AudioSource audioSource)
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();

        if(audioSource != null) audioSource.PlayOneShot(currentLine.Audio);

        StartCoroutine(TypeSentence(currentLine));
        
       
    }

    public void accuse(String name, AudioSource audioSource)
    {
        if (guiltCounter == 0)
        {
            if (name == "Teacher")
            {
                Globals.spilledWineCorrect = true;
            }
            guiltCounter = 1;
        } else if (guiltCounter == 1)
        {
            if (name == "Student")
            {
                Globals.vomitCorrect = true;
            }
            guiltCounter = 2;
        } else if (guiltCounter == 2)
        {
            if (name == "Activist")
            {
                Globals.glueCorrect = true;
            }
        }
        DisplayNextDialogueLine(audioSource);
    }

    public void accuseTeacher(AudioSource audioSource)
    {
        accuse("Teacher", audioSource);
    }
    
    public void accuseChild(AudioSource audioSource)
    {
        accuse("Child", audioSource);
    }
    
    public void accuseStudent(AudioSource audioSource)
    {
        accuse("Student", audioSource);
    }
    
    public void accuseActivist(AudioSource audioSource)
    {
        accuse("Activist", audioSource);
    }
    
    public void accuseArtist(AudioSource audioSource)
    {
        accuse("Artist", audioSource);
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        IsDialogueActive = false;
        if (Globals.showGuiltDialogue)
        {
            animator.Play("outGuilt");
            GameOverScreen.Setup(Globals.vomitCorrect, Globals.spilledWineCorrect, Globals.glueCorrect);
        }
        else
        {
            animator.Play("out");
        }
    }
}