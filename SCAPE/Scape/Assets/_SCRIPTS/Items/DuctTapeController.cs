using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuctTapeController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;
    public StretchyTape Tape;

    [Header("Outlets - Stick Targets")]
    public ScaffoldingController Scaffolding;
    public LaunchpadBController LaunchpadB;
    public JellyCatController Cat;

    [Header("Configurables")]
    public float ClosenessThreshholdPixels;

    public enum StickTo
    {
        None,
        Scaffolding,
        LaunchpadB,
        Cat,
    }

    private bool win = false;

    private StickTo curSticking = StickTo.None;

    void Update()
    {
        CheckStickables();
        UpdateSticking();
    }

    private void CheckStickables()
    {
        Vector2 screenPointOfSelf = ArCamera.WorldToScreenPoint(this.transform.position);
        Vector2 screenPointOfLaunchpadB = ArCamera.WorldToScreenPoint(LaunchpadB.transform.position);
        Vector2 screenPointOfCat = ArCamera.WorldToScreenPoint(Cat.transform.position);
        Vector2 screenPointOfScaffolding = ArCamera.WorldToScreenPoint(Scaffolding.transform.position);

        bool closeToLaunchpadB = Mathf.Abs(Vector2.Distance(screenPointOfLaunchpadB, screenPointOfSelf)) < ClosenessThreshholdPixels;
        bool closeToScaffolding = Mathf.Abs(Vector2.Distance(screenPointOfScaffolding, screenPointOfSelf)) < ClosenessThreshholdPixels;
        bool closeToCat = Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfSelf)) < ClosenessThreshholdPixels; ;


        if (closeToCat)
        {
            curSticking = StickTo.Cat;
        }
        else if (closeToLaunchpadB)
        {
            if (curSticking == StickTo.Scaffolding)
            {
                win = true;
            }
            else
            {
                curSticking = StickTo.LaunchpadB;
            }
        }
        else if (closeToScaffolding)
        {
            if (curSticking == StickTo.LaunchpadB)
            {
                win = true;
            }
            else
            {
                curSticking = StickTo.Scaffolding;
            }
        }
    }

    private void UpdateSticking()
    {
        if (!win)
        {
            switch (curSticking)
            {
                case StickTo.LaunchpadB:
                    Tape.DrawTapeBetween(this.transform.position, LaunchpadB.transform.position);
                    break;
                case StickTo.Cat:
                    Tape.DrawTapeBetween(this.transform.position, Cat.transform.position);
                    break;
                case StickTo.Scaffolding:
                    Tape.DrawTapeBetween(this.transform.position, Scaffolding.transform.position);
                    break;
            }
        }
        else
        {
            Tape.DrawTapeBetween(LaunchpadB.transform.position, Scaffolding.transform.position);
        }
    }
}
