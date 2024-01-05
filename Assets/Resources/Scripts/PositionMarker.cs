using UnityEngine;

public class PositionMarker : MonoBehaviour
{
    public Color color = Color.magenta;
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, 0.2f);
    }
#endif
}
