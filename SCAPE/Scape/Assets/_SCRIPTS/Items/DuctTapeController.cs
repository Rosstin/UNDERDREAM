using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuctTapeController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;
    public LaunchpadBController LaunchpadB;
    public StretchyTape Tape;

    [Header("Configurables")]
    public float ClosenessThreshholdPixels;

    private bool stickToLaunchpad = false;

    void Update()
    {
        if (!stickToLaunchpad)
        {
            Vector2 screenPointOfSelf = ArCamera.WorldToScreenPoint(this.transform.position);
            Vector2 screenPointOfLaunchpad = ArCamera.WorldToScreenPoint(LaunchpadB.transform.position);
            if (Mathf.Abs(Vector2.Distance(screenPointOfLaunchpad, screenPointOfSelf)) < ClosenessThreshholdPixels)
            {
                stickToLaunchpad = true;
            }
        }
        else 
        {
            Tape.DrawTapeBetween(this.transform.position, LaunchpadB.transform.position);
        }

    }
}
