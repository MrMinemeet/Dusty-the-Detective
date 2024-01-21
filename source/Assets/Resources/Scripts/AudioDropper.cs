using System.Collections.Generic;
using UnityEngine;

/**
 * Mutes all audio sources that are not in the camera's view
 */
public class AudioDropper : MonoBehaviour
{
	private readonly List<AudioSource> _audioSources = new();
	private Camera _camera;

	private void Awake()
	{
		// Find all GameObjects with an AudioSource component
		foreach (AudioSource obj in FindObjectsOfType<AudioSource>())
			_audioSources.Add(obj);
		
		_camera = Camera.main;
	}
	
	private void LateUpdate()
	{
		// Check if position of audio source is within the camera's view. If it is not, then mute the audio source
		foreach (AudioSource source in _audioSources)
		{
			Vector3 screenPoint = _camera.WorldToViewportPoint( source.transform.position);
			bool isOffScreen = screenPoint.x < 0 || screenPoint.x > Screen.width || 
			                   screenPoint.y < 0 || screenPoint.y > Screen.height;
			
			source.mute = isOffScreen;
		}
	}
}
