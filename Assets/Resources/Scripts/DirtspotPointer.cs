/*
 * Script by "Code Monkey" on YouTube with some modifications to fit the project.
 * Tutorial: https://youtu.be/dHzeHh-3bp4
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DirtspotPointer : MonoBehaviour
{
    private readonly List<Pointer> _pointerList = new();
    private Camera _uiCamera;
    
    private const float BORDER_SIZE = 75f;
    
    private void Awake()
    {
        _uiCamera = GameObject.Find("uiCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        // Create arrows for dirt spots of current floor
        foreach(Vector3 targetPos in Globals.TrashPositionMap[SceneManager.GetActiveScene().name]){
            GameObject pointer = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Pointer"), transform);
            pointer.name = "Pointer_" + targetPos;
            _pointerList.Add(new Pointer(targetPos, pointer));
        }
    }

    private void Update()
    {
        foreach (Pointer p in _pointerList)
        {
            p.UpdatePos(_uiCamera);
        }
    }
    
    private static float Vec3ToAngle(Vector3 vector)
    {
        // Normalize the vector
        vector = vector.normalized;
        
        // Calculate the angle in radians using Atan2
        float angleRad = Mathf.Atan2(vector.y, vector.x);

        // Convert radians to degrees
        float angleDeg = angleRad * Mathf.Rad2Deg;

        // Ensure the angle is within [0, 360] range
        if (angleDeg < 0)
        {
            angleDeg += 360f;
        }

        return angleDeg;
    }
    
    private class Pointer
    {
        private Vector3 TargetPosition { get; }
        private RectTransform RectTransform { get; }
        private Image Image { get; }
        
        public Pointer(Vector3 targetPosition, GameObject gameObject)
        {
            TargetPosition = targetPosition;
            RectTransform = gameObject.GetComponent<RectTransform>();
            Image = gameObject.GetComponent<Image>();
        }

        public void UpdatePos(Camera uiCamera)
        {
            Vector3 fromPosition = Camera.main!.transform.position;
            fromPosition.z = 0f;
        
            // Convert direction to angle
            float angle = Vec3ToAngle((TargetPosition - fromPosition).normalized);
            RectTransform.localEulerAngles = new Vector3(0, 0, angle);
        
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(TargetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= BORDER_SIZE || targetPositionScreenPoint.x >= Screen.width - BORDER_SIZE ||
                               targetPositionScreenPoint.y <= BORDER_SIZE || targetPositionScreenPoint.y >= Screen.height - BORDER_SIZE;

            if (isOffScreen)
            {
                // Set image to arrow
                Image.enabled = true;
            
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                // Cap the position to the screen
                cappedTargetScreenPosition.x =  Mathf.Clamp(cappedTargetScreenPosition.x, BORDER_SIZE, Screen.width - BORDER_SIZE);
                cappedTargetScreenPosition.y =  Mathf.Clamp(cappedTargetScreenPosition.y, BORDER_SIZE, Screen.height - BORDER_SIZE);
                
                // FIXME: At the moment, the camera of the "game" part is used, not the UI camera. Which causes the pointer to always be in the bottom left corner.
                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                RectTransform.position = pointerWorldPosition;
                RectTransform.localPosition = new Vector3(RectTransform.localPosition.x, RectTransform.localPosition.y, 0f);
                //Debug.Log("Pointer position: " + RectTransform.localPosition);
            }
            else
            {
                // Set image to none
                Image.enabled = false;
            }
        }
    }
}
