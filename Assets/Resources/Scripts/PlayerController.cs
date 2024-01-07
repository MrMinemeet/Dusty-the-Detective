using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public bool savePosition;
    public string gameManagerName;

    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _rb;
    private Animator _a;
    private GameManager _gameManager;

    // Indicates if the player released the input at leased once since coming to the scene
    private bool _released;

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
        // Check if player is not moving, in order to mark input as released at least once
        if (!_released && _moveInput is { x: 0, y: 0 })
        {
            _released = true;
            return;
        }
        
        // Skip if dialogue is active
        if (DialogueManager.IsDialogueActive)
        {
            _moveInput = Vector2.zero;
            _rb.velocity = Vector2.zero;
            return;
        }

        // Only move if the player had released the input at least once
        if (_released)
            _rb.velocity = _moveInput * moveSpeed;
        
        if (savePosition)
            _gameManager.playerPosition = transform.position; 
    }

    private void OnMove(InputValue value)
    {
        // Don't do anything if game is paused
        if (PauseMenu.IsGamePaused) return;

        if (DialogueManager.IsDialogueActive)
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