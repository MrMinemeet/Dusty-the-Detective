using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	private readonly List<Waypoint> _path = new();
	private Rigidbody2D _rigidBody;
	private Waypoint _targetWaypoint;
	private float _currentlyWaitedTime;
	private bool _soundPlayed;
	private AudioSource _audioSource;
	private bool _playerInTrigger;

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
		if (_playerInTrigger && DialogueManager.IsDialogueActive)
		{
			// Stop moving and do nothing
			_rigidBody.velocity = Vector2.zero;
			return;
		}
		
		if (Vector2.Distance(transform.position, _targetWaypoint.Position) <= 0.35f)
		{
			// Avoid looping sound and don't play sound if audio source is disabled
			if (!_soundPlayed && _audioSource.enabled) {
				PlaySound(_targetWaypoint.AudioClip, _targetWaypoint.Volume);
				_soundPlayed = true;
			}
			
			// Target reached, wait
			_currentlyWaitedTime += Time.deltaTime;
			if (_currentlyWaitedTime >= _targetWaypoint.WaitTime)
			{
				_currentlyWaitedTime = 0f;
				
				// Finished waiting at last waypoint, stop sound
				_soundPlayed = false;
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player")) _playerInTrigger = true;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player")) _playerInTrigger = false;
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

	/**
	 * Plays the passed audio clip.
	 * Playback is skipped if _audioSource is null, the audioClip is null, the audioSource is already playing or the volume is 0.
	 */
	private void PlaySound(AudioClip audioClip, float volume)
	{
		if (_audioSource == null || audioClip == null || volume == 0f) return;
		_audioSource.PlayOneShot(audioClip, volume); // Allow multiple sounds to be played without interrupting each other
	}

	/**
	 * Stops the audioSource from playing
	 */
	private void StopSound()
	{
		if (_audioSource != null) _audioSource.Stop();

	}
}