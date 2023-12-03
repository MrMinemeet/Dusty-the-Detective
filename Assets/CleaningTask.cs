using UnityEngine;

public class CleaningTask : MonoBehaviour
{
	private bool _canClean = false;

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
		if (_canClean && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("Player cleaned spot '" + gameObject.name + "'");
			Destroy(gameObject);
		}
	}
}