using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuctTapeController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;
    public LaunchpadBController LaunchpadB;

    [Header("Configurables")]
    public float ClosenessThreshholdPixels;

    void Update()
    {
        Vector2 screenPointOfSelf = ArCamera.WorldToScreenPoint(this.transform.position);
        Vector2 screenPointOfLaunchpad = ArCamera.WorldToScreenPoint(LaunchpadB.transform.position);
        if (Mathf.Abs(Vector2.Distance(screenPointOfLaunchpad, screenPointOfSelf)) < ClosenessThreshholdPixels)
        {
            //stick the tape
        }
    }
}
