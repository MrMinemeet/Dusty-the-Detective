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
	private const int MAX_TRASH_PER_FLOOR = 1;
	
	public static TrashStatus vomitStatus;
	public static TrashStatus wineStatus;
	public static TrashStatus glueStatus;
	
	//bools for correct assuming which npc was guilty, used by DialogueManager
	public static bool vomitCorrect;
	public static bool spilledWineCorrect;
	public static bool glueCorrect;
	//bool for animating the correct guilt dialogue
	public static bool showGuiltDialogue;

	// Total time in seconds of the game running
	public static double TotalTimeRunning;

	/**
	 * Provides the amount of trash left in total
	 */
	public static int LeftoverTrash => trashMap.Values.SelectMany(trash => trash).Count();
	public static int CurrentFloor => Floors.IndexOf(CurrentFloorName);
	public static string CurrentFloorName => SceneManager.GetActiveScene().name;
	

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

	private static readonly string[] TrashToUse = { "wine", "glue", "vomit" };

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

		// Load all dirt spot sprites
		Sprite[] sprites = Resources.LoadAll<Sprite>("DirtSpots/dirt_spots");
		sprites = sprites
			.Where(s => TrashToUse.Any(name => s.name.Contains(name))).ToArray();
		
		if (sprites.Length == 0)
		{
			Debug.LogError("Could not load/select dirt spot sprites");
			return;
		}

		int trashCounter = 0;
		foreach (var floor in Floors)
		{
			List<Trash> trashList = new();
			for (int i = 0; i < MAX_TRASH_PER_FLOOR; i++)
			{
				trashList.Add(new Trash(Vector3.negativeInfinity, sprites[trashCounter++]));
			}
			trashMap.Add(floor, trashList);
		}
		#endregion
	}
	
	public static void ResetGlobals()
	{
		// Trash status
		vomitStatus = TrashStatus.ACTIVE;
		wineStatus = TrashStatus.ACTIVE;
		glueStatus = TrashStatus.ACTIVE;
		
		// Flags
		vomitCorrect = false;
		spilledWineCorrect = false;
		glueCorrect = false;
		showGuiltDialogue = false;
		
		// Other values
		TotalTimeRunning = 0f;

		ReadOnlyCollection<string> keys = ActiveGuestMap.Keys.ToList().AsReadOnly();
		foreach (string key in keys)
			ActiveGuestMap[key] = true;
		
		// Reset trash map
		trashMap.Clear();
		
		Initialize();
	}
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


// Variables to store whether a dirt spot is cleaned and disposed
public enum TrashStatus
{
	ACTIVE, COLLECTED, DISPOSED
}