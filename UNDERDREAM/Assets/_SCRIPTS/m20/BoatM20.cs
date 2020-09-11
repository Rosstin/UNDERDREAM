using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BoatM20 : MonoBehaviour
{
    public GameObject Container;
    public AudioSource DrownSFX;
    public Vector3 DrownPosition;
    public float DrownPeriod;
    public AnimationCurve DrownCurve;

    private float drownElapsed = 0f;

    private Vector3 initialLocalP;

    private bool drown = false;
    private bool finishedDrowning = false;
    public void Drown()
    {
        drown = true;
        DrownSFX.Play();
    }

    private void Start()
    {
        initialLocalP = Container.transform.localPosition;
    }
    private void Update()
    {
        if (drown)
        {
            drownElapsed += Time.deltaTime;

            Container.transform.localPosition
                =
                Vector3.Lerp(
                    initialLocalP,
                    DrownPosition,
                    DrownCurve.Evaluate(drownElapsed / DrownPeriod)


                );


        }
    }
}
