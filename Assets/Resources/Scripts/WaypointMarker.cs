using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
    public Color color = Color.magenta;
    [Tooltip("Wait time in seconds at this position")]
    public float waitTime;
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, 0.2f);
    }
#endif
}
