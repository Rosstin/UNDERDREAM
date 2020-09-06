using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerM16 : BaseController
{
    [Header("Cymbals")]
    public float CymbalTime;
    public AudioSource CymbalSFX;

    private float elapsed;
    private bool playedCymbal = false;

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        elapsed += Time.deltaTime;

        if (!playedCymbal && elapsed > CymbalTime)
        {
            playedCymbal = true;
            CymbalSFX.Play();
        }
    }
}
