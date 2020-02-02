using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuctTapeController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;
    public StretchyTape Tape;

    [Header("Outlets - Stick Targets")]
    public LaunchpadBController LaunchpadB;
    public JellyCatController Cat;

    [Header("Configurables")]
    public float ClosenessThreshholdPixels;

    public enum StickTo
    {
        None,
        LaunchpadA,
        LaunchpadB,
        Cat,
    }

    private StickTo curSticking = StickTo.None;

    void Update()
    {
        CheckStickables();
        UpdateSticking();
    }

    private void CheckStickables()
    {
        Vector2 screenPointOfSelf = ArCamera.WorldToScreenPoint(this.transform.position);

        // stick to launchpadB if close enough
        Vector2 screenPointOfLaunchpadB = ArCamera.WorldToScreenPoint(LaunchpadB.transform.position);
        float launchpadBDistance = Mathf.Abs(Vector2.Distance(screenPointOfLaunchpadB, screenPointOfSelf));

        if (launchpadBDistance < ClosenessThreshholdPixels)
        {
            Tape.gameObject.SetActive(true);
            curSticking = StickTo.LaunchpadB;
        }

        // however, the cat has priority, so stick to the cat instead if it's close enough
        Vector2 screenPointOfCat = ArCamera.WorldToScreenPoint(Cat.transform.position);
        if (Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfSelf)) < ClosenessThreshholdPixels)
        {
            Tape.gameObject.SetActive(true);
            curSticking = StickTo.Cat;
        }

    }

    private void UpdateSticking()
    {
        switch (curSticking)
        {
            case StickTo.LaunchpadB:
                Tape.DrawTapeBetween(this.transform.position, LaunchpadB.transform.position);
                break;
            case StickTo.Cat:
                Tape.DrawTapeBetween(this.transform.position, Cat.transform.position);
                break;
        }
    }
}
