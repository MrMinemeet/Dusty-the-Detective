using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;


    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = _moveInput * moveSpeed;
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
