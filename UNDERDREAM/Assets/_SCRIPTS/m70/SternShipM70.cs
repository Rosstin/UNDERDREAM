﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM70 : MonoBehaviour
{
    [Header("Stern Body Movement")]
    public GameObject SternBody;
    public Float SternFloat;
    public Transform SternRiseDestination;
    public Transform SternGoal;
    public AnimationCurve SternRiseCurve;
    public AnimationCurve SternForwardCurve;
    public float SternRisePeriod;
    public float SternForwardPeriod;
    public BoxCollider2D SternCollider;

    [Header("Ship Jump")]
    public Vector2 JumpForce;

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
    public AudioSource SternChuckle1;
    public AudioSource SternChuckle2;
    public AudioSource SternBigLaugh;

    private bool partTwoStarted = false;
    private bool updateVelocity = true;

    private float elapsed; 
    private float fElapsed; // elapsed for front can
    private float bElapsed; // elapsed for back can
    private float twoElapsed; // elapsed for stern rising

    private Vector3 sternInitialLocalPos;

    private bool sternRisen = false;

    private Vector2 currentKnockback;
    private Vector2 currentV;


    private void Start()
    {
        FrontCannon.MyAnimator.speed = 2f;
        BackCannon.MyAnimator.speed = 2f;
        sternInitialLocalPos = SternBody.transform.localPosition;

        currentKnockback = CannonFireEffectOnStern;
        currentV= BoatVelocity;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        fElapsed += Time.deltaTime;
        bElapsed += Time.deltaTime;

        if (partTwoStarted)
        {
            twoElapsed += Time.deltaTime;

            if (!sternRisen)
            {
                SternFloat.enabled = false;
                SternBody.transform.localPosition = Vector3.Lerp(sternInitialLocalPos, SternRiseDestination.localPosition, 
                    SternRiseCurve.Evaluate(twoElapsed / SternRisePeriod));
                if (twoElapsed > SternRisePeriod)
                {
                    SternChuckle2.Play();
                    sternRisen = true;
                    twoElapsed = 0f;
                }
            }
            else
            {
                SternFloat.enabled = true;
                SternBody.transform.localPosition = Vector3.Lerp(SternRiseDestination.localPosition, SternGoal.localPosition,
                    SternForwardCurve.Evaluate(twoElapsed / SternForwardPeriod));
            }
        }

        if (updateVelocity)
        {
            MyRigidbody.velocity = new Vector2(currentV.x, MyRigidbody.velocity.y);
        }

    }

    public void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        updateVelocity = false;
        MyRigidbody.AddForce(JumpForce);
        yield return new WaitForSeconds(0.2f);
        updateVelocity = true;
    }

    /*
    private IEnumerator JumpCoroutine()
    {
    float animTime = 0f;
    while (animTime < jumpDuration)
    {
        animTime += Time.deltaTime;
        float upProgress = jumpCurve.Evaluate(animTime / jumpDuration);
        this.transform.position = Vector3.Lerp(startPosition.position, upPosition.position, upProgress);
        yield return 0;
    }
    yield return new WaitForSeconds(hangTime);
    animTime = 0f;
    while (animTime < jumpDuration)
    {
        animTime += Time.deltaTime;
        float downProg = jumpCurve.Evaluate(animTime / jumpDuration);
        this.transform.position = Vector3.Lerp(upPosition.position, startPosition.position, downProg);
        yield return 0;
    }
    }
    */

    public void FireFrontCannon()
    {
        StartCoroutine(FireCannon(FrontCannon));
    }

    public void FireBackCannon()
    {
        StartCoroutine(FireCannon(BackCannon));
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
