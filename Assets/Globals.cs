using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class Globals
{
	public static int CurrentFloor = 0;
	public static ReadOnlyCollection<string> Floors = new List<string>()
	{
		"EmptySceneDummy",
		"SampleScene"
	}.AsReadOnly();
}
