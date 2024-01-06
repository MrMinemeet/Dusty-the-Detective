using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayGuestAmount : MonoBehaviour
{
	private TextMeshProUGUI _guestAmountValueText;

	private void Awake()
	{
		_guestAmountValueText = GetComponent<TextMeshProUGUI>();
	}

	private void LateUpdate()
	{
		// Set text to amounts of guests that are still active
		_guestAmountValueText.text = Globals.ActiveGuestMap
			.Count(guest => guest.Value)
			.ToString();
	}
}