using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM17 : MonoBehaviour
{
    [Header("Cannon Outlets")]
    public Animator FrontCannon;
    public Animator BackCannon;
    public AudioSource CannonSfx;
    public AudioSource CymbalSfx;

    [Header("Front Cannon")]
    public float FCanPeriod;
    public float FCanInitialDelay;

    [Header("Back Cannon")]
    public float BCanPeriod;
    public float BCanInitialDelay;

    private float fElapsed;
    private float bElapsed;

    private void Start()
    {
        // syncopate the cannon shots
        fElapsed = -FCanInitialDelay;
        bElapsed = -BCanInitialDelay;
    }

    private void Update()
    {
        fElapsed += Time.deltaTime;
        bElapsed += Time.deltaTime;

        if (fElapsed > FCanPeriod)
        {
            fElapsed = 0f;
            FrontCannon.Play("cannon");
            CannonSfx.Play();
            CymbalSfx.Play();
        }

        if (bElapsed > BCanPeriod)
        {
            bElapsed = 0f;
            BackCannon.Play("cannon");
            CannonSfx.Play();
            CymbalSfx.Play();
        }
    }
}
