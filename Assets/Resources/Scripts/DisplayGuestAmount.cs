using TMPro;
using UnityEngine;

public class DisplayGuestAmount : MonoBehaviour
{
	private TextMeshProUGUI _guestAmountValueText;

	private void Awake()
	{
		_guestAmountValueText = GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	private void Update()
	{
		_guestAmountValueText.text = Globals.ActiveGuests.Count.ToString();
	}
}