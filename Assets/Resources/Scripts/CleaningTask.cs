using UnityEngine;
using UnityEngine.Events;

public class CleaningTask : MonoBehaviour
{
	private bool _canClean;
	public UnityEvent onCleaned;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_canClean = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_canClean = false;
		}
	}

	private void Update()
	{
		// Don't allow cleaning if game is paused
		if (PauseMenu.IsGamePaused) return;

		if (!_canClean || !Input.GetKeyDown(KeyCode.E)) return;

		Debug.Log("Player cleaned spot '" + gameObject.name + "'");
		onCleaned.Invoke();

        Trash trash = Globals.trashMap[Globals.CurrentFloorName].Find(t => t.Position == this.transform.position);
        Debug.Log(trash.Image.name);
		switch(trash.Image.name)
		{
			case "dirt_spot_wine": Globals.wineStatus = Globals.TrashStatus.COLLECTED; break;
			case "dirt_spot_vomit": Globals.vomitStatus = Globals.TrashStatus.COLLECTED; break;
			case "dirt_spot_glue": Globals.glueStatus = Globals.TrashStatus.COLLECTED; break;
		}

        Globals.trashMap[Globals.CurrentFloorName].Remove(trash);

		Destroy(gameObject);
	}
}