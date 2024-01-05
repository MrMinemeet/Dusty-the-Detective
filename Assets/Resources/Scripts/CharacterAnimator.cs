using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Don't do anything if game is paused
        if (PauseMenu.IsGamePaused) return;
        
        Vector2 movementVector = _rigidBody.velocity;
        if (movementVector.x != 0 || movementVector.y != 0)
        {
            _animator.SetFloat("X", movementVector.x);
            _animator.SetFloat("Y", movementVector.y);
            
            _animator.SetBool("IsWalking", true);            
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
}
