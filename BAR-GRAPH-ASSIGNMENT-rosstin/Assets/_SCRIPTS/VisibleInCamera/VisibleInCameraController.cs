using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleInCameraController : MonoBehaviour
{

    public Camera targetCamera;
    public Peekaboo targetBox;

    private void Update()
    {
        Vector3 viewportPoint = targetCamera.WorldToViewportPoint(targetBox.transform.position);
        if(viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
        {
            // it's visible
            targetBox.SetVisible(true);
        }
        else
        {
            // it's out of bounds or behind the camera
            targetBox.SetVisible(false);
        }
    }

}
