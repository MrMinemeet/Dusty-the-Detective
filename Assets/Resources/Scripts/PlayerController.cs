using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float normalMoveSpeed = 3f;
    public readonly UnityEvent OnSkipDialogueEvent = new();
    
    public bool savePosition;
    public string gameManagerName;

    private float _moveSpeed = 3f;
    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _rb;
    private Animator _a;
    private GameManager _gameManager;

    // Indicates if the player released the input at leased once since coming to the scene
    private bool _released;
    
    // Dash related variables
    private float _dashTimer;
    private bool _doesDash;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _a = GetComponent<Animator>();
        if (savePosition)
        {
            _gameManager = GameObject.FindWithTag(gameManagerName).GetComponent<GameManager>();
            
            Vector3 newPosition = _gameManager.playerPosition;
            float yOffset = 0.5f;
            newPosition.y -= yOffset;
            transform.position = newPosition; 
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Reduce dash timer
        if (_dashTimer > 0)
            _dashTimer -= Time.deltaTime;
        
        // Check if player is not moving, in order to mark input as released at least once
        if (!_released && _moveInput is { x: 0, y: 0 })
        {
            _released = true;
            return;
        }
        
        // Skip if dialogue is active
        if (DialogueManager.IsDialogueActive || PauseMenu.IsGamePaused || Globals.IsMiniGameActive)
        {
            _moveInput = Vector2.zero;
            _rb.velocity = Vector2.zero;
            return;
        }
        
        if (savePosition)
            _gameManager.playerPosition = transform.position; 

        // Only move if the player had released the input at least once
        if (_released)
            _rb.velocity = _moveInput * _moveSpeed;
    }

    private void OnSkipDialogue(InputValue value)
    {
        // Trigger skip event when in a dialogue
        if (DialogueManager.IsDialogueActive)
            OnSkipDialogueEvent.Invoke();
    }
    
    private void OnDash(InputValue value)
    {
        if (PauseMenu.IsGamePaused || Globals.IsMiniGameActive) return;
        if (DialogueManager.IsDialogueActive) return;
        if (_dashTimer > 0) return;
        
        // Double move speed for 1 second
        _moveSpeed = normalMoveSpeed * 2f;
        _dashTimer = 1f;
        Invoke(nameof(StopDash), 0.5f);
    }

    private void StopDash()
    {
        // Reset move speed
        _moveSpeed = normalMoveSpeed;
    }

    private void OnMove(InputValue value)
    {
        // Don't do anything if game is paused or player is in a minigame
        if (PauseMenu.IsGamePaused) return;

        if (DialogueManager.IsDialogueActive || Globals.IsMiniGameActive)
        {
            // Stop animations
            _a.SetBool("IsWalking", false);
            return;
        }
        
        _moveInput = value.Get<Vector2>();

        // Only move if the player had released the input at least once and the input is not 0
        if (_released && (_moveInput.x != 0 || _moveInput.y != 0))
        {
            _a.SetFloat("X", _moveInput.x);
            _a.SetFloat("Y", _moveInput.y);
            
            _a.SetBool("IsWalking", true);            
        }
        else
        {
            _a.SetBool("IsWalking", false);
        }
    }
}