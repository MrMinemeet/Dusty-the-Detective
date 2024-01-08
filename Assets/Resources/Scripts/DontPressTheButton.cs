using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DontPressTheButton : MonoBehaviour
{
    private const float TIME_TO_NOT_PRESS = 10f;
    private float _timeShown;
    
    public TextMeshProUGUI timeLeftText;
    
    private void Update()
    {
        _timeShown += Time.deltaTime;
        
        // Update time left
        float timeLeft = TIME_TO_NOT_PRESS - _timeShown;
        timeLeftText.text = $"{timeLeft:F1} seconds left";
        
        
        if (_timeShown >= TIME_TO_NOT_PRESS)
        {
            // Trigger successful cleaning
            
            Disable();
        }
    }

    public void OnPressed()
    {
        if (!(_timeShown < TIME_TO_NOT_PRESS)) return;
        Debug.Log("The button that should not be pressed, was pressed!");
        EasterEgg.OpenRR();
            
        Disable();
    }

    /**
     * Disables the minigame
     */
    private void Disable()
    {
        transform.gameObject.SetActive(false);
    }
}
