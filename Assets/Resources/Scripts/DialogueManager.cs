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
    private AudioSource _audioSource;
    
    public static bool IsDialogueActive { get; private set; }
 
    public float typingSpeed = 0.1f;
 
    public Animator animator;

    private static int _guiltCounter;

    public GameOverScreen GameOverScreen;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _lines = new Queue<DialogueLine>();
    }

    private void Start()
    {
        // Register on OnSkipDialogueEvent
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().OnSkipDialogueEvent.AddListener(() => DisplayNextDialogueLine(_audioSource));
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

    private void Accuse(string guestName, AudioSource audioSource)
    {
        if (_guiltCounter == 0)
        {
            if (guestName == "Teacher")
            {
                Globals.SpilledWineCorrect = true;
            }

            _guiltCounter = 1;
        }
        else if (_guiltCounter == 1)
        {
            if (guestName == "Student")
            {
                Globals.VomitCorrect = true;
            }

            _guiltCounter = 2;
        }
        else if (_guiltCounter == 2)
        {
            if (guestName == "Activist")
            {
                Globals.GlueCorrect = true;
            }
        }
        DisplayNextDialogueLine(audioSource);
    }

    public void accuseTeacher(AudioSource audioSource)
    {
        Accuse("Teacher", audioSource);
    }
    
    public void accuseChild(AudioSource audioSource)
    {
        Accuse("Child", audioSource);
    }
    
    public void accuseStudent(AudioSource audioSource)
    {
        Accuse("Student", audioSource);
    }
    
    public void accuseActivist(AudioSource audioSource)
    {
        Accuse("Activist", audioSource);
    }
    
    public void accuseArtist(AudioSource audioSource)
    {
        Accuse("Artist", audioSource);
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line)
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