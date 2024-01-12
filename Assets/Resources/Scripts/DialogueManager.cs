using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
 
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> _lines;
    
    public static bool IsDialogueActive { get; private set; }
 
    public float typingSpeed = 0.1f;
 
    public Animator animator;

    private static int _guiltCounter;

    public GameOverScreen GameOverScreen;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, AudioSource audioSource)
    {
        IsDialogueActive = true;
        animator.Play(Globals.ShowGuiltDialogue ? "inGuilt" : "in");

        _lines.Clear();
 
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            _lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine(audioSource);
    }
 
    public void DisplayNextDialogueLine(AudioSource audioSource)
    {
        if (_lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        var currentLine = _lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();

        if(audioSource != null) audioSource.PlayOneShot(currentLine.Audio);

        StartCoroutine(TypeSentence(currentLine));
        
       
    }

    private void accuse(string name, AudioSource audioSource)
    {
        if (_guiltCounter == 0)
        {
            if (name == "Teacher")
            {
                Globals.SpilledWineCorrect = true;
            }

            _guiltCounter = 1;
        }
        else if (_guiltCounter == 1)
        {
            if (name == "Student")
            {
                Globals.VomitCorrect = true;
            }

            _guiltCounter = 2;
        }
        else if (_guiltCounter == 2)
        {
            if (name == "Activist")
            {
                Globals.GlueCorrect = true;
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
        if (Globals.ShowGuiltDialogue)
        {
            // Disable HUD
            GameObject.Find("HUD").SetActive(false);
            
            // Show game over screen
            animator.Play("outGuilt");
            GameOverScreen.Setup(Globals.VomitCorrect, Globals.SpilledWineCorrect, Globals.GlueCorrect);
        }
        else
        {
            animator.Play("out");
        }
    }
}