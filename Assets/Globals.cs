using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class Globals
{
	public static int CurrentFloor = 1;

	public static readonly ReadOnlyCollection<string> Floors = new List<string>
	{
		"EmptySceneDummy",
		"Restaurant"
	}.AsReadOnly();
}