/*
 * Script based on tutorial by "Brackeys" on YouTube.
 * https://youtu.be/CE9VOZivb3I
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] private Animator _transition;

	private const float TRANSITION_TIME = 1f;

	private void Awake()
	{
		if (_transition == null)
			_transition = GameObject.Find("Crossfade").GetComponent<Animator>();
	}

	public IEnumerator LoadLevel(String levelName)
	{
		PlayerInput pi = null;
		// Try to get the player
		try
		{
			GameObject player = GameObject.Find("Player");
			// Disable player movement
			pi = player.GetComponent<PlayerInput>();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		
		pi!.enabled = false;
		_transition.SetTrigger("Start");

		yield return new WaitForSeconds(TRANSITION_TIME);
		
		pi!.enabled = true;
		SceneManager.LoadScene(levelName);
	}
}