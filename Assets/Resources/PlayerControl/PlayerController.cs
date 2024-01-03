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
    
    // Start is called before the first frame update
    void Start()
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
        _rb.velocity = _moveInput * moveSpeed;
        if (savePosition)
        {
            _gameManager.playerPosition = transform.position; 
        }
        
    }
    void OnMove(InputValue value)
    {
        // Don't do anything if game is paused
        if (PauseMenu.IsGamePaused) return;
        
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