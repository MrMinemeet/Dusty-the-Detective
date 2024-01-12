using TMPro;
using UnityEngine;

public class DontPressTheButton : MonoBehaviour
{
	private const float DEFAULT_TIME_TO_WAIT = 10f;

	public TextMeshProUGUI timeLeftText;
	public Animator animator;
	private static bool _hasBeenPlayed;
	private static bool _runTimer;
	private static float _timeShown;
	private static float _timeToWait;

	private Rigidbody2D _playerRigidbody2D;

	private void Awake()
	{
		_playerRigidbody2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (!_hasBeenPlayed && Globals.LeftoverTrash == 0)
		{
			// Not played yet, and no trash left -> start
			StartMinigame();
		}
		else if (_runTimer)
		{
			_timeShown += Time.deltaTime;

			// Update time left
			var timeLeft = _timeToWait - _timeShown;
			timeLeftText.text = $"{timeLeft:F1} seconds left";

			if (_timeShown >= _timeToWait)
				// Trigger successful cleaning
				Disable();
		}
	}

	private void LateUpdate()
	{
		if (!_runTimer) return;
		
		// Disable movement while minigame is active
		_playerRigidbody2D.velocity = Vector2.zero;
	
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
		Globals.IsMiniGameActive = false;
		animator.Play("hide");
		_runTimer = false;
		Destroy(this);
	}

	private void StartMinigame()
	{
		Debug.Log("Starting DontPressTheButton minigame");
		Globals.IsMiniGameActive = true;
		animator.Play("show");
		_timeToWait = DEFAULT_TIME_TO_WAIT;
		_hasBeenPlayed = true;
		_runTimer = true;
	}
}