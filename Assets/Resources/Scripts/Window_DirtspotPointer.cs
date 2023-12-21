/*
 * Script by "Code Monkey" on YouTube with some modifications to fit the project.
 * Tutorial: https://youtu.be/dHzeHh-3bp4
 */

using UnityEngine;
using UnityEngine.UI;

public class Window_DirtspotPointer : MonoBehaviour
{
    private Vector3 _targetPosition = new Vector3(0, 0, 0);
    private RectTransform _pointerRectTransform;
    private Image _pointerImage;
    
    private const float BORDER_SIZE = 75f;
    
    private void Awake()
    {
        _pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        _pointerImage = transform.Find("Pointer").GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 toPosition = _targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        
        // Convert direction to angle
        float angle = Vec3ToAngle(dir);
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= BORDER_SIZE || targetPositionScreenPoint.x >= Screen.width - BORDER_SIZE ||
                           targetPositionScreenPoint.y <= BORDER_SIZE || targetPositionScreenPoint.y >= Screen.height - BORDER_SIZE;

        if (isOffScreen)
        {
            // Set image to arrow
            _pointerImage.enabled = true;
            
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            // Cap the position to the screen
            cappedTargetScreenPosition.x =  Mathf.Clamp(cappedTargetScreenPosition.x, BORDER_SIZE, Screen.width - BORDER_SIZE);
            cappedTargetScreenPosition.y =  Mathf.Clamp(cappedTargetScreenPosition.y, BORDER_SIZE, Screen.height - BORDER_SIZE);
            
            /*
             // FIXME: At the moment, the camera of the "game" part is used, not the UI camera. Which causes the pointer to always be in the bottom left corner.
            Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            _pointerRectTransform.position = pointerWorldPosition;
            _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x, _pointerRectTransform.localPosition.y, 0f);
            */
            
        }
        else
        {
            // Set image to none
            _pointerImage.enabled = false;
        }
    }
    
    private float Vec3ToAngle(Vector3 vector)
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
}
