using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM17 : MonoBehaviour
{
    [Header("Main Camera Jitter Outlet")]
    public Jitter CamJitter;

    [Header("Cannon Outlets")]
    public Animator FrontCannon;
    public Animator BackCannon;
    //public SternCannon FrontSternCannon;
    //public SternCannon BackSternCannon;

    [Header("SFX")]
    public AudioSource CymbalSfx;
    public AudioSource CannonSfx;
    public float CymbalDelay;
    public float CannonDelay;

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
            //FireCannon(FrontCannon);
        }

        if (bElapsed > BCanPeriod)
        {
            bElapsed = 0f;
            //FireCannon(BackCannon);
        }
    }

    private IEnumerator FireFrontCannon()
    {
        FrontCannon.SetTrigger("fire");
        yield return new WaitForSeconds(CymbalDelay);
        CymbalSfx.Play();
        yield return new WaitForSeconds(CannonDelay);
        CannonSfx.Play();

    }
}
