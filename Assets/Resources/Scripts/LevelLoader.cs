using System;
using System.Collections;
using UnityEngine;
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
		_transition.SetTrigger("Start");

		yield return new WaitForSeconds(TRANSITION_TIME);

		SceneManager.LoadScene(levelName);
	}
}