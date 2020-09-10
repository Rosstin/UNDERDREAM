using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM17 : MonoBehaviour
{
    [Header("Part Two New Values")]
    public Vector2 PartTwoVelocity;
    public Vector2 PartTwoCannonFireEffectOnStern;

    [Header("Main Camera Jitter Outlet")]
    public Jitter CamJitter;

    [Header("My Rigidbody")]
    public Rigidbody2D MyRigidbody;

    [Header("Cannon Effect On Stern")]
    public Vector2 CannonFireEffectOnStern;

    [Header("Stern Forward Movement")]
    public Vector2 BoatVelocity;

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

    private bool partTwoStarted = false;
    private bool updateVelocity = true;

    private float elapsed; 
    private float fElapsed; // elapsed for front can
    private float bElapsed; // elapsed for back can

    private Vector2 currentKnockback;
    private Vector2 currentV;

    public void StartPartTwo()
    {
        partTwoStarted = true;
        currentV = PartTwoVelocity;
        currentKnockback = PartTwoCannonFireEffectOnStern;
    }

    private void Start()
    {
        // syncopate the cannon shots
        fElapsed = -FCanInitialDelay;
        bElapsed = -BCanInitialDelay;

        currentKnockback = CannonFireEffectOnStern;
        currentV= BoatVelocity;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        fElapsed += Time.deltaTime;
        bElapsed += Time.deltaTime;

        if (updateVelocity)
        {
            MyRigidbody.velocity = currentV;
        }

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
        updateVelocity = false;
        cannon.MyAnimator.SetTrigger("fire");

        cannon.RandomRotation();

        yield return new WaitForSeconds(CymbalDelay);
        CymbalSfx.Play();
        yield return new WaitForSeconds(CannonDelay);
        CannonSfx.Play();
        cannon.Smoke.gameObject.SetActive(true);
        cannon.Smoke.SetTrigger("fire");
        CamJitter.SetJitter(true);
        MyRigidbody.AddForce(currentKnockback);
        cannon.FireBall();
        yield return new WaitForSeconds(ScreenShakeDuration);
        cannon.Smoke.gameObject.SetActive(false);
        CamJitter.SetJitter(false);
        updateVelocity = true;
    }
}
