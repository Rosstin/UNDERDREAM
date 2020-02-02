using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuctTapeController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;
    public StretchyTape Tape;
    public WinRocket Rocket;

    [Header("Outlets - Stick Targets")]
    public ScaffoldingController Scaffolding;
    public LaunchpadBController LaunchpadB;
    public JellyCatController Cat;

    [Header("Configurables")]
    public float ClosenessThreshholdPixels;
    public AnimationCurve CombineCurve;
    public float WinAnimationDuration;

    public enum StickTo
    {
        None,
        Scaffolding,
        LaunchpadB,
        Cat,
    }

    private bool win = false;
    private StickTo curSticking = StickTo.None;
    private float winTime = 0f;

    private Vector3 launchpadInitialPos;
    private Vector3 scaffoldingInitialPos;
    private Vector3 midPoint;

    void Start()
    {
        launchpadInitialPos = LaunchpadB.transform.localPosition;
        scaffoldingInitialPos = Scaffolding.transform.localPosition;
        midPoint = Vector3.Lerp(launchpadInitialPos, scaffoldingInitialPos, 0.5f);
    }

    void OnEnable()
    {
        Tape.SetVisibility(true);
    }

    void OnDisable()
    {
        Tape.SetVisibility(false);
    }

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

        bool closeToLaunchpadB = false;
        bool closeToScaffolding = false;
        bool closeToCat = false;
        if (!LaunchpadB.InBox())
        {
            closeToLaunchpadB = Mathf.Abs(Vector2.Distance(screenPointOfLaunchpadB, screenPointOfSelf)) < ClosenessThreshholdPixels;
        }
        if (!Scaffolding.InBox())
        {
            closeToScaffolding = Mathf.Abs(Vector2.Distance(screenPointOfScaffolding, screenPointOfSelf)) < ClosenessThreshholdPixels;
        }
        if (!Cat.InBox())
        {
            closeToCat = Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfSelf)) < ClosenessThreshholdPixels;
        }

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
            winTime += Time.deltaTime;

            LaunchpadB.transform.localPosition = Vector3.Lerp(LaunchpadB.transform.localPosition, midPoint, CombineCurve.Evaluate(winTime/WinAnimationDuration));
            Scaffolding.transform.localPosition = Vector3.Lerp(Scaffolding.transform.localPosition, midPoint, CombineCurve.Evaluate(winTime / WinAnimationDuration));

            Vector2 screenPointOfLaunchpadB = ArCamera.WorldToScreenPoint(LaunchpadB.transform.position);
            Vector2 screenPointOfScaffolding = ArCamera.WorldToScreenPoint(Scaffolding.transform.position);

            if (Mathf.Abs(Vector2.Distance(screenPointOfLaunchpadB, screenPointOfScaffolding)) < ClosenessThreshholdPixels)
            {
                Rocket.gameObject.SetActive(true);
                Scaffolding.gameObject.SetActive(false);
                LaunchpadB.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                Tape.gameObject.SetActive(false);
                Cat.gameObject.SetActive(false);
            }

            Tape.DrawTapeBetween(LaunchpadB.transform.position, Scaffolding.transform.position);
        }
    }
}
