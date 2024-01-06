using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public static class Globals
{
	// Times in seconds for guests range of leaving
	public const int MIN_TIME_UNTIL_LEAVE = 10 * 60;
	public const int MAX_TIME_UNTIL_LEAVE = 20 * 60;

	// Total time in seconds of the game running
	public static double TotalTimeRunning = 0;

	public static readonly Dictionary<string, float> TimeUntilGuestLeaves = new()
	{
		{ "Activist", Random.Range(MIN_TIME_UNTIL_LEAVE, MAX_TIME_UNTIL_LEAVE) },
		{ "Artist", Random.Range(MIN_TIME_UNTIL_LEAVE, MAX_TIME_UNTIL_LEAVE) },
		{ "Child", Random.Range(MIN_TIME_UNTIL_LEAVE, MAX_TIME_UNTIL_LEAVE) },
		{ "Student", Random.Range(MIN_TIME_UNTIL_LEAVE, MAX_TIME_UNTIL_LEAVE) },
		{ "Teacher", Random.Range(MIN_TIME_UNTIL_LEAVE, MAX_TIME_UNTIL_LEAVE) }
	};
	
	// Holds the names of all guests and their respective status if they left the hotel
	public static readonly Dictionary<string, bool> ActiveGuestMap = new()
	{
		{ "Activist", true },
		{ "Artist", true },
		{ "Child", true },
		{ "Student", true },
		{ "Teacher", true }
	};

	private const int MAX_TRASH_PER_FLOOR = 2;
	public static int CurrentFloor => Floors.IndexOf(CurrentFloorName);
	public static string CurrentFloorName => SceneManager.GetActiveScene().name;

	public static readonly ReadOnlyCollection<string> Floors = new List<string>
	{
		"Lobby",
		"Hallway",
		"Restaurant",
	}.AsReadOnly();

	public static readonly Dictionary<String, List<Trash>> trashMap = new();

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
			List<Trash> trashList = new();
			for (int i = 0; i < MAX_TRASH_PER_FLOOR; i++)
			{
				trashList.Add(new Trash(Vector3.negativeInfinity, sprites[Random.Range(0, sprites.Length)]));
			}
			trashMap.Add(floor, trashList);
		}
		#endregion
	}

	/**
	 * Provides the amount of trash left in total
	 */
	public static int LeftoverTrash => trashMap.Values.SelectMany(trash => trash).Count();
}

public record Trash
{
	public Vector3 Position;
	public readonly Sprite Image;
	
	public Trash(Vector3 position, Sprite image)
	{
		Position = position;
		Image = image;
	}
}