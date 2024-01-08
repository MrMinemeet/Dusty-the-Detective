using System;
using TMPro;
using UnityEngine;

public class DontPressTheButton : MonoBehaviour
{
	private const float DEFAULT_TIME_TO_WAIT = 10f;

	public TextMeshProUGUI timeLeftText;
	public Animator animator;
	private bool _hasBeenPlayed;
	private float _timeShown;

	private float _timeToWait;

	private void Update()
	{
		if (!_hasBeenPlayed && Globals.LeftoverTrash == 1)
			StartMinigame();
		else if (Globals.LeftoverTrash != 1 || !_hasBeenPlayed)
			return;


		_timeShown += Time.deltaTime;

		// Update time left
		var timeLeft = _timeToWait - _timeShown;
		timeLeftText.text = $"{timeLeft:F1} seconds left";


		if (_timeShown >= _timeToWait)
			// Trigger successful cleaning
			Disable();
	}

	public void OnPressed()
	{
		if (!(_timeShown < _timeToWait)) return; // Time was already over and button should hide
		
		Debug.Log("The button that should not be pressed, was pressed!");
		EasterEgg.OpenRR();

		_timeToWait += 5f;
	}

	/**
	 * Disables the minigame
	 */
	private void Disable()
	{
		Debug.Log("Disabling DontPressTheButton minigame");
		animator.Play("hide");
		Destroy(this);
	}

	private void StartMinigame()
	{
		Debug.Log("Starting DontPressTheButton minigame");
		animator.Play("show");
		_timeToWait = DEFAULT_TIME_TO_WAIT;
		_hasBeenPlayed = true;
	}
}