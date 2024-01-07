using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static FloorType;

public class WalksoundManager : MonoBehaviour
{
	// If a new floor type is added, then set the matching sound in the "Walksound"-prefab's inspector
	public List<KeyValuePair> floorSounds = new()
	{
		new KeyValuePair { key = Wood, val = null },
		new KeyValuePair { key = CeramicTile, val = null },
		new KeyValuePair { key = Carpet, val = null }
	};

	private readonly Dictionary<FloorType, AudioClip> _floorSounds = new();
	private readonly List<Tilemap> _tilemaps = new();
	private AudioSource _audioSource;
	private Rigidbody2D _rigidbody2D;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_rigidbody2D = GetComponentInParent<Rigidbody2D>();

		// Convert list to an actual dictionary
		foreach (var kvp in floorSounds)
		{
			if (kvp.val == null) Debug.LogError($"Audio clip of '{kvp.key}' in '{transform.parent.name}' is null");
			_floorSounds.Add(kvp.key, kvp.val);
		}

		// Get all tilemaps
		var grid = GameObject.Find("Grid");
		foreach (var obj in grid.GetComponentsInChildren<Transform>().Select(t => t.gameObject))
		{
			var tileMap = obj.GetComponent<Tilemap>();
			if (tileMap != null)
				_tilemaps.Add(tileMap);
		}
	}

	private void FixedUpdate()
	{
		if (_rigidbody2D.velocity == Vector2.zero)
		{
			// Object is not moving, stop walking sound by disabling the audio source
			_audioSource.enabled = false;
			return;
		}

		// Object is moving, enable audio source and play the correct sound depending on the floor type
		var position = transform.position - new Vector3(0, 0.5f, 0); // Add offset to feet
		// Get all tiles at the position 
		var tiles = _tilemaps
			.FindAll(tm => tm.GetTile(tm.WorldToCell(position)) != null)
			.ConvertAll(tm => tm.GetTile(tm.WorldToCell(position)));
		_audioSource.enabled = true;
		
		var newClip = _floorSounds[GetFloorTypeFrom(tiles)];
		if (_audioSource.clip == newClip) return; // Skip if the clip is already playing
		
		_audioSource.clip = newClip;
		_audioSource.Play();
	}

	private FloorType GetFloorTypeFrom(List<TileBase> tbList)
	{
		FloorType floorType = Wood;
		foreach (var tb in tbList)
			if (WoodFloor.Contains(tb.name)) floorType = Wood;
			else if (CarpetFloor.Contains(tb.name)) return Carpet;
			else floorType = CeramicTile;

		return floorType;
	}

	#region ListsForFloorTypes

	// This is not beautiful nor efficient nor should it be done like this, but the tiles are not sorted and I don't want to sort them
	private static readonly string[] WoodFloor =
	{
		"MainTileMapUnity_0", "MainTileMapUnity_7", "MainTileMapUnity_8", "MainTileMapUnity_9",
		"MainTileMapUnity_43", "MainTileMapUnity_44", "MainTileMapUnity_45", "MainTileMapUnity_61",
		"MainTileMapUnity_62", "MainTileMapUnity_63", "MainTileMapUnity_64", "MainTileMapUnity_65",
		"MainTileMapUnity_83", "MainTileMapUnity_84", "MainTileMapUnity_85", "MainTileMapUnity_102",
		"MainTileMapUnity_103", "MainTileMapUnity_104", "MainTileMapUnity_105", "MainTileMapUnity_106",
		"MainTileMapUnity_107", "MainTileMapUnity_108", "MainTileMapUnity_109", "MainTileMapUnity_110",
		"MainTileMapUnity_111", "MainTileMapUnity_112", "MainTileMapUnity_113", "MainTileMapUnity_114",
		"MainTileMapUnity_115", "MainTileMapUnity_146", "MainTileMapUnity_147", "MainTileMapUnity_148",
		"MainTileMapUnity_149", "MainTileMapUnity_150", "MainTileMapUnity_151", "MainTileMapUnity_152",
		"MainTileMapUnity_153", "MainTileMapUnity_154", "MainTileMapUnity_155", "MainTileMapUnity_156",
		"MainTileMapUnity_193", "MainTileMapUnity_194", "MainTileMapUnity_195", "MainTileMapUnity_196",
		"MainTileMapUnity_197", "MainTileMapUnity_198", "MainTileMapUnity_200", "MainTileMapUnity_201",
		"MainTileMapUnity_202", "MainTileMapUnity_240", "MainTileMapUnity_241", "MainTileMapUnity_242",
		"MainTileMapUnity_284", "MainTileMapUnity_285", "MainTileMapUnity_286", "MainTileMapUnity_547",
		"MainTileMapUnity_548", "MainTileMapUnity_549", "MainTileMapUnity_571", "MainTileMapUnity_572",
		"MainTileMapUnity_573", "MainTileMapUnity_574", "MainTileMapUnity_575", "MainTileMapUnity_576",
		"MainTileMapUnity_579", "MainTileMapUnity_157", "MainTileMapUnity_581", "MainTileMapUnity_582",
		"MainTileMapUnity_583", "MainTileMapUnity_603", "MainTileMapUnity_604", "MainTileMapUnity_605",
		"MainTileMapUnity_606", "MainTileMapUnity_607", "MainTileMapUnity_608", "MainTileMapUnity_609",
		"MainTileMapUnity_610", "MainTileMapUnity_611", "MainTileMapUnity_612", "MainTileMapUnity_613",
		"MainTileMapUnity_614", "MainTileMapUnity_615", "MainTileMapUnity_616", "MainTileMapUnity_622",
		"MainTileMapUnity_623", "MainTileMapUnity_624", "MainTileMapUnity_639", "MainTileMapUnity_640",
		"MainTileMapUnity_641", "MainTileMapUnity_642", "MainTileMapUnity_643", "MainTileMapUnity_644",
		"MainTileMapUnity_645", "MainTileMapUnity_646", "MainTileMapUnity_647", "MainTileMapUnity_648",
		"MainTileMapUnity_649", "MainTileMapUnity_650", "MainTileMapUnity_651", "MainTileMapUnity_652",
		"MainTileMapUnity_653", "MainTileMapUnity_653", "MainTileMapUnity_654", "MainTileMapUnity_655",
		"MainTileMapUnity_656", "MainTileMapUnity_657", "MainTileMapUnity_658", "MainTileMapUnity_659",
		"MainTileMapUnity_660", "MainTileMapUnity_661", "MainTileMapUnity_662", "MainTileMapUnity_663",
		"MainTileMapUnity_664", "MainTileMapUnity_665", "MainTileMapUnity_666", "MainTileMapUnity_682",
		"MainTileMapUnity_683", "MainTileMapUnity_684", "MainTileMapUnity_685", "MainTileMapUnity_686",
		"MainTileMapUnity_687", "MainTileMapUnity_688", "MainTileMapUnity_689", "MainTileMapUnity_702",
		"MainTileMapUnity_703", "MainTileMapUnity_704", "MainTileMapUnity_705", "MainTileMapUnity_706",
		"MainTileMapUnity_707", "MainTileMapUnity_708", "MainTileMapUnity_709", "MainTileMapUnity_710",
		"MainTileMapUnity_711", "MainTileMapUnity_712", "MainTileMapUnity_713", "MainTileMapUnity_714",
		"MainTileMapUnity_715", "MainTileMapUnity_733", "MainTileMapUnity_734", "MainTileMapUnity_735",
		"MainTileMapUnity_736", "MainTileMapUnity_737", "MainTileMapUnity_738", "MainTileMapUnity_739",
		"MainTileMapUnity_740", "MainTileMapUnity_741", "MainTileMapUnity_742", "MainTileMapUnity_743",
		"MainTileMapUnity_744", "MainTileMapUnity_765", "MainTileMapUnity_766", "MainTileMapUnity_767",
		"MainTileMapUnity_768", "MainTileMapUnity_769", "MainTileMapUnity_770"
	};

	private static readonly string[] CarpetFloor =
	{
		"MainTileMapUnity_66", "MainTileMapUnity_67", "MainTileMapUnity_68", "MainTileMapUnity_69",
		"MainTileMapUnity_116", "MainTileMapUnity_117", "MainTileMapUnity_118", "MainTileMapUnity_119",
		"MainTileMapUnity_158", "MainTileMapUnity_159", "MainTileMapUnity_160", "MainTileMapUnity_161"
	};

	#endregion
}

[Serializable]
public enum FloorType
{
	Wood,
	CeramicTile,
	Carpet
}

// This is required as Unity does not support Dictionary serialization in the inspector out of the box
[Serializable]
public class KeyValuePair
{
	public FloorType key;
	public AudioClip val;
}