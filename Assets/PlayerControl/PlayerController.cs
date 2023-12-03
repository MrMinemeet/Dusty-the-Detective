using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Boolean savePosition = false;
    public string gameManagerName;

    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _rb;
    private Animator _a;
    private GameManager _gameManager;
    private bool initialSetupDone = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _a = GetComponent<Animator>();
        if (savePosition)
        {
            _gameManager = GameObject.FindWithTag(gameManagerName).GetComponent<GameManager>();

            if (!initialSetupDone)
            {
                SetInitialPlayerPosition();
                initialSetupDone = true;
            }
            else
            {
                Vector3 newPosition = _gameManager.playerPosition;
                float yOffset = 0.5f;
                newPosition.y -= yOffset;
                transform.position = newPosition; 
            }
        }
    }
    
    private void SetInitialPlayerPosition()
    {
        Vector3 initialPosition = new Vector3(0.0f, 0.0f, 0.0f);
        
        // Set the initial position as needed
        if (gameManagerName == "GameManagerLobby")
        {
            initialPosition = new Vector3(-1.0f, -2.5f, 0.0f);
        }

        // Assign the initial position to the GameManager and the player
        _gameManager.playerPosition = initialPosition;
        transform.position = initialPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        _rb.velocity = _moveInput * moveSpeed;
        if (savePosition)
        {
            _gameManager.playerPosition = transform.position; 
        }
        
    }
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        if (_moveInput.x != 0 || _moveInput.y != 0)
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