using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Object = System.Object;

public static class Globals
{
	public const int MAX_TRASH_PER_FLOOR = 2;

	public static int CurrentFloor = 1;

	public static readonly ReadOnlyCollection<string> Floors = new List<string>
	{
		"Lobby",
		"Hallway",
		"Restaurant",
	}.AsReadOnly();

	// Holds the position for each dirt spot on each the floors
	public static readonly Dictionary<String, List<Vector3>> TrashPositionMap = new();

	// Holds the sprite for each dirt spot on each the floors
	public static readonly Dictionary<String, List<Sprite>> TrashSpriteMap = new();

	// This method is called at the start of the program, before the first scene is loaded.
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize()
	{
		#region TrashCreation
		Debug.Log("Initializing Trash List...");

		// Load all dirt spot sprites (using a "Object[]", as casting to "Sprite[]" causes it to be null, casting individual elements works though)
		Sprite[] sprites = Resources.LoadAll<Sprite>("DirtSpots/dirt_spots");
		if (sprites == null)
		{
			Debug.LogError("Could not load dirt spot sprites");
			return;
		}

		foreach (var floor in Floors)
		{
			List<Sprite> trashSpriteList = new List<Sprite>();
			List<Vector3> trashPositionList = new List<Vector3>();
			for (int i = 0; i < MAX_TRASH_PER_FLOOR; i++)
			{
				trashSpriteList.Add(sprites[UnityEngine.Random.Range(0, sprites.Length)]);
				trashPositionList.Add(Vector3.negativeInfinity); // Vector3 can't be null, so use negative infinity instead
			}
			TrashSpriteMap.Add(floor, trashSpriteList);
			TrashPositionMap.Add(floor, trashPositionList);
		}
		#endregion
	}
}