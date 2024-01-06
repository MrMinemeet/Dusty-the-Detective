using UnityEngine;

public class Waypoint
{
	public Vector2 Position { get; private set; }
	public float WaitTime { get; private set; }
	
	public Waypoint(Vector2 position, float waitTime)
	{
		Position = position;
		WaitTime = waitTime;
	}
}
