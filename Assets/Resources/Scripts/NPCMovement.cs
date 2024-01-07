using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	private readonly List<Waypoint> _path = new();
	private Rigidbody2D _rigidBody;
	private Waypoint _targetWaypoint;
	private float _currentlyWaitedTime;
	private bool _isInConversation;
	private AudioSource _audioSource;

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();

		// Get child "PathPositions" object
		Transform pathPositions = transform.Find("PathPositions");
		if (pathPositions == null)
		{
			Debug.LogError("Could not find child \"PathPositions\" object");
			return;
		}

		// Get all children of "PathPositions" object
		foreach (Transform child in pathPositions)
		{
			WaypointMarker wpm = child.GetComponent<WaypointMarker>();
			_path.Add(new Waypoint(child.position, wpm));
		}

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
		_targetWaypoint = _path[0];
	}

	private void FixedUpdate()
	{
		if (_isInConversation)
		{
			// Stop moving and do nothing
			_rigidBody.velocity = Vector2.zero;
			return;
		}
		
		if (Vector2.Distance(transform.position, _targetWaypoint.Position) <= 0.35f)
		{
			
			PlaySound(_targetWaypoint.AudioClip, _targetWaypoint.Volume);
			
			// Target reached, wait
			_currentlyWaitedTime += Time.deltaTime;
			if (_currentlyWaitedTime >= _targetWaypoint.WaitTime)
			{
				_currentlyWaitedTime = 0f;
				
				// Finished waiting at last waypoint, stop sound
				StopSound();
				
				// Wait time over, get new target
				_targetWaypoint = GetNewTarget();
			}
			else
			{
				// Wait
				_rigidBody.velocity = Vector2.zero;
				return;
			}
		}

		// Move towards target using velocity
		_rigidBody.velocity = (_targetWaypoint.Position - (Vector2)transform.position).normalized * _speed;
	}

	/**
	 * <summary> Get the next <see cref="Waypoint"/> in the path </summary>
	 * <returns> The next waypoint in the path </returns>
	 * <remarks> If the path is empty, it starts from the beginning </remarks>
	 */
	private Waypoint GetNewTarget()
	{
		// If at end of path, go back to start
		return _path.IndexOf(_targetWaypoint) == _path.Count - 1
			? _path[0]
			// Otherwise, go to next position in path
			: _path[_path.IndexOf(_targetWaypoint) + 1];
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		_isInConversation = other.CompareTag("Player") && DialogueManager.IsDialogueActive;
	}
	
	/**
	 * Plays the passed audio clip.
	 * Playback is skipped if _audioSource is null, the audioClip is null, the audioSource is already playing or the volume is 0.
	 */
	private void PlaySound(AudioClip audioClip, float volume)
	{
		if (_audioSource == null || audioClip == null || _audioSource.isPlaying || volume == 0f) return;
		_audioSource.clip = audioClip;
		_audioSource.volume = volume;
		_audioSource.Play();
	}

	/**
	 * Stops the audioSource from playing
	 */
	private void StopSound()
	{
		if(_audioSource != null) _audioSource.Stop();
	}
}