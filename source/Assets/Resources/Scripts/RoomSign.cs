using UnityEngine;

public class RoomSign : MonoBehaviour
{

    private GameObject _nameBubble;

    private void Awake()
    {
        _nameBubble = transform.Find("NameBubble").gameObject;
    }
    
    private void Start()
    {
        // Disable name bubble per default
        _nameBubble.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _nameBubble.gameObject.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            _nameBubble.gameObject.SetActive(false);
        }
    }
}
