using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 1f;


	private Vector2 _moveInput = Vector2.zero;
	private Rigidbody2D _rb;

	// Start is called before the first frame update
	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		_rb.velocity = _moveInput * moveSpeed;
	}

	private void OnMove(InputValue value)
	{
		_moveInput = value.Get<Vector2>();
	}
}