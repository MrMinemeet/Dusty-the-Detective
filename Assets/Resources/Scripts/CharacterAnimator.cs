using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        Vector2 movement_vector = _rigidBody.velocity;
        if (movement_vector.x != 0 || movement_vector.y != 0)
        {
            _animator.SetFloat("X", movement_vector.x);
            _animator.SetFloat("Y", movement_vector.y);
            
            _animator.SetBool("IsWalking", true);            
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
}
