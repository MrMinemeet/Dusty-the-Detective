using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DirtSpotDistributor : MonoBehaviour
{
	[SerializeField]
	public Tilemap tileMap;
	private String _currentFloor;

	private void Awake()
	{
		if (tileMap == null)
		{
			tileMap = GameObject.Find("Floor").GetComponent<Tilemap>();
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		_currentFloor = SceneManager.GetActiveScene().name;
		Debug.Log("Current Floor: " + _currentFloor);

		#region DirtPlacement

		if (Globals.TrashSpriteMap.TryGetValue(_currentFloor, out var value))
		{
			Debug.Log("Placing Dirt Spots...");
			
			// Place dirt spots
			for (int i = 0; i < Globals.MAX_TRASH_PER_FLOOR; ++i)
			{
				GameObject dirtSpot = new GameObject("DirtSpot" + i);
				dirtSpot.transform.SetParent(this.transform);
				dirtSpot.transform.localScale = new Vector3(0.5f, 0.5f, 1);
				
				SpriteRenderer sr = dirtSpot.AddComponent<SpriteRenderer>();
				sr.sprite = value[i];
				sr.sortingOrder = 1; // To be on top of the floor

				// If the dirt spot is null, place it in a random position
				if (Globals.TrashPositionMap[_currentFloor][i] == null)
				{
					Bounds b = tileMap.localBounds;
					b.extents = new Vector3(b.extents.x - 1.5f, b.extents.y - 1.5f, b.extents.z);

					// Draw a random position within the bounds and only if a it doesn't collide with a wall
					dirtSpot.transform.position = b.center + new Vector3(
						Random.Range(-b.extents.x, b.extents.x),
						Random.Range(-b.extents.y, b.extents.y));
				}
				else
				{
					dirtSpot.transform.position = Globals.TrashPositionMap[_currentFloor][i].position;
				}
			}
		}

		#endregion
	}
}