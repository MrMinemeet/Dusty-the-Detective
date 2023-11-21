using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 1f;

	private Vector2 _moveInput = Vector2.zero;
	private Rigidbody2D _rb;
	private Animator _a;

	// Start is called before the first frame update
	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_a = GetComponent<Animator>();
	}

	// Update is called once per frame
	private void Update()
	{
		_rb.velocity = _moveInput * moveSpeed;
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