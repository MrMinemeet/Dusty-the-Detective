/*
 * Script based on Indie Nuggets' video "Unity Nuggets: How to Add Target Indicator Arrow in 2D Game"
 * Tutorial: https://youtu.be/U1SdjGUFSAI
 */

using UnityEngine;

public class DirtspotPointer : MonoBehaviour
{
	private const float HIDE_DISTANCE = 2.5f;
	public Vector3 target;

    // Update is called once per frame
    void Update()
    {
	    // Hide if too close
	    if (Vector2.Distance(transform.position, target) < HIDE_DISTANCE)
	    {
		    SetChildrenActive(false);
		    return;
	    }
	    
	    SetChildrenActive(true);
	    Vector3 direction = target - transform.position;
	    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void SetChildrenActive(bool value)
    {
	    foreach(Transform child in transform)
		    child.gameObject.SetActive(value);
    }
}
