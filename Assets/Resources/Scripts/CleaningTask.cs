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
		Globals.trashMap[Globals.CurrentFloorName].Remove(
			Globals.trashMap[Globals.CurrentFloorName].Find(t => t.Position == this.transform.position));

		Destroy(gameObject);
	}
}