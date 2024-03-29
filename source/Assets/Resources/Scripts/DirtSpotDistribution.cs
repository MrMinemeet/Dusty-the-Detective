using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DirtSpotDistributor : MonoBehaviour
{
	// Holds tiles, that are not a full tile and also don't have a full tile collider. These tiles are not allowed to have dirt spots on them.
	// Not very beautiful, but from what I saw, there is no proper way to check if the part of the tile has "drawing" on it
	private static readonly string[] TilesToNotPlaceOn =
	{
		"MainTileMapUnity_34", "MainTileMapUnity_35", "MainTileMapUnity_36", "MainTileMapUnity_37",
		"MainTileMapUnity_38", "MainTileMapUnity_39", "MainTileMapUnity_40", "MainTileMapUnity_41",
		"MainTileMapUnity_42", "MainTileMapUnity_74", "MainTileMapUnity_75", "MainTileMapUnity_76",
		"MainTileMapUnity_77", "MainTileMapUnity_78", "MainTileMapUnity_79", "MainTileMapUnity_80",
		"MainTileMapUnity_81", "MainTileMapUnity_82", "MainTileMapUnity_124", "MainTileMapUnity_125",
		"MainTileMapUnity_126", "MainTileMapUnity_127", "MainTileMapUnity_128", "MainTileMapUnity_129",
		"MainTileMapUnity_130", "MainTileMapUnity_131", "MainTileMapUnity_132", "MainTileMapUnity_166",
		"MainTileMapUnity_167", "MainTileMapUnity_168", "MainTileMapUnity_169", "MainTileMapUnity_170",
		"MainTileMapUnity_171", "MainTileMapUnity_172", "MainTileMapUnity_173", "MainTileMapUnity_174",
		"MainTileMapUnity_175", "MainTileMapUnity_176", "MainTileMapUnity_177", "MainTileMapUnity_177",
		"MainTileMapUnity_207", "MainTileMapUnity_208", "MainTileMapUnity_209", "MainTileMapUnity_210",
		"MainTileMapUnity_211", "MainTileMapUnity_212", "MainTileMapUnity_213", "MainTileMapUnity_214",
		"MainTileMapUnity_215", "MainTileMapUnity_247", "MainTileMapUnity_248", "MainTileMapUnity_249",
		"MainTileMapUnity_250", "MainTileMapUnity_251", "MainTileMapUnity_252", "MainTileMapUnity_253",
		"MainTileMapUnity_254", "MainTileMapUnity_255", "MainTileMapUnity_291", "MainTileMapUnity_292",
		"MainTileMapUnity_293", "MainTileMapUnity_294", "MainTileMapUnity_295", "MainTileMapUnity_296",
		"MainTileMapUnity_297", "MainTileMapUnity_298", "MainTileMapUnity_299", "MainTileMapUnity_339",
		"MainTileMapUnity_340", "MainTileMapUnity_341", "MainTileMapUnity_342", "MainTileMapUnity_343",
		"MainTileMapUnity_344", "MainTileMapUnity_345", "MainTileMapUnity_346", "MainTileMapUnity_347",
		"MainTileMapUnity_348", "MainTileMapUnity_349", "MainTileMapUnity_350", "MainTileMapUnity_351",
		"MainTileMapUnity_339", "MainTileMapUnity_340", "MainTileMapUnity_341", "MainTileMapUnity_342",
		"MainTileMapUnity_343", "MainTileMapUnity_344", "MainTileMapUnity_345", "MainTileMapUnity_346",
		"MainTileMapUnity_347", "MainTileMapUnity_348", "MainTileMapUnity_349", "MainTileMapUnity_350",
		"MainTileMapUnity_351", "MainTileMapUnity_379", "MainTileMapUnity_380", "MainTileMapUnity_381",
		"MainTileMapUnity_382", "MainTileMapUnity_425", "MainTileMapUnity_426", "MainTileMapUnity_427",
		"MainTileMapUnity_428", "MainTileMapUnity_429", "MainTileMapUnity_430", "MainTileMapUnity_431",
		"MainTileMapUnity_470", "MainTileMapUnity_471", "MainTileMapUnity_472", "MainTileMapUnity_473",
		"MainTileMapUnity_474", "MainTileMapUnity_475", "MainTileMapUnity_512", "MainTileMapUnity_513",
		"MainTileMapUnity_514", "MainTileMapUnity_515", "MainTileMapUnity_516", "MainTileMapUnity_517",
		"MainTileMapUnity_518", "MainTileMapUnity_550", "MainTileMapUnity_551", "MainTileMapUnity_552",
		"MainTileMapUnity_553", "MainTileMapUnity_554", "MainTileMapUnity_584", "MainTileMapUnity_585",
		"MainTileMapUnity_586", "MainTileMapUnity_587"
	};

	[SerializeField] public GameObject grid;
	private string _currentFloor;
	private readonly List<Collider2D> _collider2Ds = new();
	private readonly List<Tilemap> _tilemaps = new();

	private void Awake()
	{
		#region GetTilemaps
		if (grid == null) grid = GameObject.Find("Grid");

		// Get children of grid
		List<GameObject> children = grid.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToList();

		foreach (GameObject obj in children)
		{
			Tilemap tileMap = obj.GetComponent<Tilemap>();
			if (tileMap == null) continue; // no tilemap found
			
			CompositeCollider2D c2d = tileMap.GetComponent<CompositeCollider2D>();
			if (c2d == null) continue; // no CompositeCollider found
			
			_tilemaps.Add(tileMap);
			_collider2Ds.Add(c2d);
		}
		_currentFloor = SceneManager.GetActiveScene().name;
		Debug.Log("Current Floor: " + _currentFloor);
		#endregion

		#region DirtPlacement

		if (Globals.TrashMap.TryGetValue(_currentFloor, out List<Trash> value))
		{
			Debug.Log("Placing Dirt Spots...");
			GameObject dirtSpotResource = Resources.Load<GameObject>("Prefabs/Dirtspots/DirtSpot");

			// Place dirt spots
			foreach (var t in value)
			{
				// Create gameObject from prefab
				GameObject dirtSpot = Instantiate(dirtSpotResource, transform);
				dirtSpot.name = $"DirtSpot_{t.Position}";
				dirtSpot.transform.localScale = new Vector3(0.5f, 0.5f, 1);
				dirtSpot.GetComponent<SpriteRenderer>().sprite = t.Image;
				dirtSpot.GetComponent<SpriteRenderer>().sortingOrder = -1;


				// If the dirt spot is null, place it in a random position
				if (!t.Position.Equals(Vector3.negativeInfinity))
				{
					// Spot already known
					dirtSpot.transform.position = t.Position;
					continue;
				}

				var b = _tilemaps[0].localBounds;
					b.extents = new Vector3(b.extents.x - 1.5f, b.extents.y - 1.5f, b.extents.z);

					TileBase tileAtPosition;
					Tile.ColliderType tileColliderType = Tile.ColliderType.None;
					// Find a position that is a tile and not
					do
					{
						// Draw a random position within the bounds and only if a it doesn't collide with a wall
						Vector3 position = b.center + new Vector3(
							Random.Range(-b.extents.x, b.extents.x),
							Random.Range(-b.extents.y, b.extents.y));

						// Get tile under position
						tileAtPosition = _tilemaps[0].GetTile(_tilemaps[0].WorldToCell(position));

						// Set position to the calculated position
						dirtSpot.transform.position = position;

						if (tileAtPosition == null) continue;
						
						TileData td = new TileData();
						tileAtPosition.GetTileData(
							new Vector3Int((int)position.x, (int)position.y, (int)position.x), _tilemaps[0],
							ref td);
						tileColliderType = td.colliderType;
					} while (
						// Don't place dirt if there is no tile
						tileAtPosition == null ||
						// If current tile collider type is NONE (there is no collider) for that tile
						tileColliderType == Tile.ColliderType.None ||
						// The tile is on the "ignore" list
						TilesToNotPlaceOn.Contains(tileAtPosition.name) ||
						// The tile has a collider which the position overlaps with
						_collider2Ds.Any(c => c.OverlapPoint(dirtSpot.transform.position))
					);

					// Store position
					t.Position = dirtSpot.transform.position;

					// Rename
					dirtSpot.name = "DirtSpot_" + t.Position;
			}
		}

		#endregion
	}
}