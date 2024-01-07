using UnityEngine;
using UnityEngine.Serialization;

public class WaypointMarker : MonoBehaviour
{
    public Color color = Color.magenta;
    [Tooltip("Wait time in seconds at this position")]
    public float waitTime;

    [FormerlySerializedAs("_audioClip")] [Tooltip("This sound is played when this waypoint is reached")]
    public AudioClip audioClip;
    [Range(0f, 1f)] [Tooltip("Volume of the sound played when this waypoint is reached")]
    public float volume = 1f;
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, 0.2f);
    }
#endif
}
