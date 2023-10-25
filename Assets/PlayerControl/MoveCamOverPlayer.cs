using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamOverPlayer : MonoBehaviour
{
    public Camera mainCamera;
    
    // TODO: Make borders that the camera won't move over

    // Update is called once per frame
    void Update()
    {
        // Move Camera over player with X and Y
        var position = transform.position;
        mainCamera.transform.position = new Vector3(position.x, position.y, mainCamera.transform.position.z);
    }
}
