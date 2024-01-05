using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    private readonly List<Vector2> _path = new();
    private Vector2 _target;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        
        // Get child "PathPositions" object
        Transform pathPositions = transform.Find("PathPositions");
        if (pathPositions == null)
        {
            Debug.LogError("Could not find child \"PathPositions\" object");
            return;
        }
        
        // Get all children of "PathPositions" object
        foreach (Transform child in pathPositions)
        {
            _path.Add(child.position);
        }
        
        // Destroy "PathPositions" object as they are not needed when the game is running
        Destroy(pathPositions.gameObject);
    }
    
    private void Start()
    {
        if (_path.Count == 0)
        {
            Debug.Log("Path is empty. Disabling Movement script");
            this.enabled = false;
            return;
        }
        
        // Move to first position in path
        _target = _path[0];
    }

    private void Update()
    {
        // Move towards target using velocity
        _rigidBody.velocity = (_target - (Vector2) transform.position).normalized * _speed;
        
        
        // If target reached, set new target
        if (Vector2.Distance(transform.position, _target) < 0.1f)
        {
            // If at end of path, go back to start
            _target = _path.IndexOf(_target) == _path.Count - 1 ? 
                _path[0] :
                // Otherwise, go to next position in path
                _path[_path.IndexOf(_target) + 1];
        }
    }
}
