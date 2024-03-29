using UnityEngine;

public class FloorChanger : MonoBehaviour
{
	[Tooltip("If true, the player will go up a floor. If false, the player will go down a floor.")]
	public bool goesUp;

	private void Awake()
	{
		// Make collider(Trigger) as big as the object
		var bd2 = GetComponent<BoxCollider2D>();
		bd2.size = new Vector2(transform.localScale.x, transform.localScale.y);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var position = transform.position;
		var color = Color.green;
		if (!goesUp)
			color = Color.red;
		Gizmos.color = color;
		Gizmos.DrawWireCube(position, new Vector3(transform.localScale.x, transform.localScale.y, 1));
		// Write up or down on the cube
	}
#endif

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Don't allow changing floors if game is paused or not a player
		if (PauseMenu.IsGamePaused || !other.CompareTag("Player")) return;

		if ((Globals.CurrentFloor == Globals.Floors.Count - 1 && goesUp) ||
		    (Globals.CurrentFloor == 0 && !goesUp))
		{
			LevelLoader ll = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
			StartCoroutine(ll.LoadLevel(Globals.Floors[Globals.CurrentFloor]));
		}
	}
}