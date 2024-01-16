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
			
			// Invoke trigger to show Key Hint
			Globals.OnShowKeyHint.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_canClean = false;
			
			// Invoke trigger to hide Key Hint
			Globals.OnHideKeyHint.Invoke();
		}
	}

	private void Update()
	{
		// Don't allow cleaning if game is paused
		if (PauseMenu.IsGamePaused) return;

		if (!_canClean || !Input.GetKeyDown(KeyCode.E)) return;

		Debug.Log("Player cleaned spot '" + gameObject.name + "'");
		onCleaned.Invoke();
		// Invoke to hide "interact" key
		Globals.OnHideKeyHint.Invoke();

        Trash trash = Globals.TrashMap[Globals.CurrentFloorName].Find(t => t.Position == this.transform.position);
        Debug.Log(trash.Image.name);
		switch(trash.Image.name)
		{
			case "dirt_spot_wine": Globals.WineStatus = TrashStatus.COLLECTED; break;
			case "dirt_spot_vomit": Globals.VomitStatus = TrashStatus.COLLECTED; break;
			case "dirt_spot_glue": Globals.GlueStatus = TrashStatus.COLLECTED; break;
		}

        Globals.TrashMap[Globals.CurrentFloorName].Remove(trash);

		Destroy(gameObject);
	}
}