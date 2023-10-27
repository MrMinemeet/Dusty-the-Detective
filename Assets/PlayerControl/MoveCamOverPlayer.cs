using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamOverPlayer : MonoBehaviour
{
    [Tooltip("If the camera is not set, the main camera will be used.")]
    public Camera playerCam;
    
    // TODO: Make borders that the camera won't move over
    void Start() {
        // If no camera is set, use the main camera by default
        if (playerCam == null)
        {
            playerCam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move Camera over player with X and Y
        var position = transform.position;
        playerCam.transform.position = new Vector3(position.x, position.y, playerCam.transform.position.z);
    }
}
