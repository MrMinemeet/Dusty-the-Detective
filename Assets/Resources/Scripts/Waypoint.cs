using UnityEngine;

public class Waypoint
{
	public Vector2 Position { get; private set; }
	public float WaitTime { get; private set; }
	public AudioClip AudioClip { get; private set; }
	public float Volume { get; private set; }
	
	public Waypoint(Vector2 position, float waitTime, AudioClip audioClip)
	{
		Position = position;
		WaitTime = waitTime;
		AudioClip = audioClip;
	}

	public Waypoint(Vector2 position, WaypointMarker wpm)
	{
		Position = position;
		WaitTime = wpm.waitTime;
		AudioClip = wpm.audioClip;
		Volume = wpm.volume;
	}
}
