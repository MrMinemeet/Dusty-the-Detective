using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	private readonly List<Waypoint> _path = new();
	private Rigidbody2D _rigidBody;
	private Waypoint _target;
	private float _waitTime;
	private bool _isInConversation;

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();

		// Get child "PathPositions" object
		Transform pathPositions = transform.Find("PathPositions");
		if (pathPositions == null)
		{
			Debug.LogError("Could not find child \"PathPositions\" object");
			return;
		}

		// Get all children of "PathPositions" object
		foreach (Transform child in pathPositions)
			_path.Add(new Waypoint(child.position, child.GetComponent<WaypointMarker>().waitTime));

		// Destroy "PathPositions" object as they are not needed when the game is running
		Destroy(pathPositions.gameObject);
	}

	private void Start()
	{
		if (_path.Count == 0)
		{
			Debug.Log("Path is empty. Disabling Movement script");
			enabled = false;
			return;
		}

		// Move to first position in path
		_target = _path[0];
	}

	private void FixedUpdate()
	{
		if (_isInConversation)
		{
			// Stop moving and do nothing
			_rigidBody.velocity = Vector2.zero;
			return;
		}
		
		if (Vector2.Distance(transform.position, _target.Position) <= 0.35f)
		{
			// Target reached, wait
			_waitTime += Time.deltaTime;
			if (_waitTime >= _target.WaitTime)
			{
				_waitTime = 0f;

				// Wait time over, get new target
				_target = GetNewTarget();
			}
			else
			{
				// Wait
				_rigidBody.velocity = Vector2.zero;
				return;
			}
		}

		// Move towards target using velocity
		_rigidBody.velocity = (_target.Position - (Vector2)transform.position).normalized * _speed;
	}

	/**
	 * <summary> Get the next <see cref="Waypoint"/> in the path </summary>
	 * <returns> The next waypoint in the path </returns>
	 * <remarks> If the path is empty, it starts from the beginning </remarks>
	 */
	private Waypoint GetNewTarget()
	{
		// If at end of path, go back to start
		return _path.IndexOf(_target) == _path.Count - 1
			? _path[0]
			// Otherwise, go to next position in path
			: _path[_path.IndexOf(_target) + 1];
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		_isInConversation = other.CompareTag("Player") && DialogueManager.IsDialogueActive;
	}
}