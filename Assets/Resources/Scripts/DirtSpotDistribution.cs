using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class DirtSpotDistributor : MonoBehaviour
{
	[SerializeField] public Tilemap tileMap;
	private TilemapCollider2D _tilemapCollider2D;
	private String _currentFloor;

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
		"MainTileMapUnity_351", "MainTileMapUnity_425", "MainTileMapUnity_426", "MainTileMapUnity_427",
		"MainTileMapUnity_428", "MainTileMapUnity_429", "MainTileMapUnity_430", "MainTileMapUnity_431",
		"MainTileMapUnity_470", "MainTileMapUnity_471", "MainTileMapUnity_472", "MainTileMapUnity_473",
		"MainTileMapUnity_474", "MainTileMapUnity_475", "MainTileMapUnity_512", "MainTileMapUnity_513",
		"MainTileMapUnity_514", "MainTileMapUnity_515", "MainTileMapUnity_516", "MainTileMapUnity_517",
		"MainTileMapUnity_518", "MainTileMapUnity_550", "MainTileMapUnity_551", "MainTileMapUnity_552",
		"MainTileMapUnity_553", "MainTileMapUnity_554", "MainTileMapUnity_584", "MainTileMapUnity_585",
		"MainTileMapUnity_586", "MainTileMapUnity_587"
	};

	private void Awake()
	{
		if (tileMap == null)
		{
			tileMap = GameObject.Find("Floor").GetComponent<Tilemap>();
		}

		_tilemapCollider2D = tileMap.GetComponent<TilemapCollider2D>();
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
			GameObject dirtSpotResource = Resources.Load<GameObject>("Prefabs/Dirtspots/DirtSpot");

			// Place dirt spots
			for (int i = 0; i < Globals.MAX_TRASH_PER_FLOOR; ++i)
			{
				// Create gameeobject from prefab
				GameObject dirtSpot = Instantiate(dirtSpotResource, transform);
				dirtSpot.name = "DirtSpot_" + i;
				dirtSpot.transform.localScale = new Vector3(0.5f, 0.5f, 1);
				dirtSpot.GetComponent<SpriteRenderer>().sprite = value[i];


				// If the dirt spot is null, place it in a random position
				if (Globals.TrashPositionMap[_currentFloor][i].Equals(Vector3.negativeInfinity))
				{
					Bounds b = tileMap.localBounds;
					b.extents = new Vector3(b.extents.x - 1.5f, b.extents.y - 1.5f, b.extents.z);

					TileBase tileAtPosition;
					Vector3 position;
					// Find a position that is a tile and not
					do
					{
						// Draw a random position within the bounds and only if a it doesn't collide with a wall
						position = b.center + new Vector3(
							Random.Range(-b.extents.x, b.extents.x),
							Random.Range(-b.extents.y, b.extents.y));

						// Get tile under position
						tileAtPosition = tileMap.GetTile(tileMap.WorldToCell(position));
					} while (
						// Don't place dirt if there is no tile
						tileAtPosition == null ||
						// The tile is on the "ignore" list
						TilesToNotPlaceOn.Contains(tileAtPosition.name) ||
						// The tile has a collider which the position overlaps with
						_tilemapCollider2D.OverlapPoint(new Vector2(position.x, position.y))
					);

					dirtSpot.transform.position = position;
					// Store position
					Globals.TrashPositionMap[_currentFloor][i] = dirtSpot.transform.position;
				}
				else
				{
					dirtSpot.transform.position = Globals.TrashPositionMap[_currentFloor][i];
				}
			}
		}

		#endregion
	}
}