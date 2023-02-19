using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerM16 : BaseController
{
    [Header("Cymbals")]
    public float CymbalTime;
    public AudioSource CymbalSFX;
    public Camera Cam;
    public Vector3 CamPosition2;
    // pos1 is 0,0,-10

    [Header("Next Scene Time")] public float nextSceneTime;

    private float elapsed;
    private bool playedCymbal = false;
    private bool goingToNext = false;

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        elapsed += Time.deltaTime;

        if (!playedCymbal && elapsed > CymbalTime)
        {
            playedCymbal = true;
            CymbalSFX.Play();
            Cam.gameObject.transform.position = CamPosition2;
        }

        if (!goingToNext && elapsed > nextSceneTime)
        {
            goingToNext = true;
            LoadNextScene();
        }
    }
}
