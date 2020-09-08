using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM17 : MonoBehaviour
{
    [Header("Main Camera Jitter Outlet")]
    public Jitter CamJitter;

    [Header("My Rigidbody")]
    public Rigidbody2D MyRigidbody;

    [Header("Cannon Effect On Stern")] public Vector2 CannonFireEffectOnStern;

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
        cannon.Smoke.gameObject.SetActive(true);
        cannon.Smoke.SetTrigger("fire");
        CamJitter.enabled = true;
        MyRigidbody.AddForce(CannonFireEffectOnStern);
        yield return new WaitForSeconds(ScreenShakeDuration);
        cannon.Smoke.gameObject.SetActive(false);
        CamJitter.enabled = false;
    }
}
