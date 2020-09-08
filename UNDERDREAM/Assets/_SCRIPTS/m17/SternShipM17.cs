using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM17 : MonoBehaviour
{
    [Header("Main Camera Jitter Outlet")]
    public Jitter CamJitter;

    [Header("Cannon Outlets")]
    public SternCannon FrontCannon;
    public SternCannon BackCannon;

    [Header("SFX")]
    public AudioSource CymbalSfx;
    public AudioSource CannonSfx;
    public float CymbalDelay;
    public float CannonDelay;
    public float ScreenShakeDuration;

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
            StartCoroutine(FireCannon(FrontCannon));
        }

        if (bElapsed > BCanPeriod)
        {
            bElapsed = 0f;
            StartCoroutine(FireCannon(BackCannon));
        }
    }

    private IEnumerator FireCannon(SternCannon cannon)
    {
        cannon.MyAnimator.SetTrigger("fire");
        yield return new WaitForSeconds(CymbalDelay);
        CymbalSfx.Play();
        yield return new WaitForSeconds(CannonDelay);
        CannonSfx.Play();
        CamJitter.enabled = true;
        yield return new WaitForSeconds(ScreenShakeDuration);
        CamJitter.enabled = false;
    }
}
