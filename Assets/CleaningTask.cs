using UnityEngine;

public class CleaningTask : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Player is in the cleaning area");
		}
	}
}