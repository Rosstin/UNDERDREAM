using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternCannon : MonoBehaviour
{
    [Header("Outlets")]
    public Animator MyAnimator;
    public CannonBall Ball;
    public Animator Smoke;

    private void Awake()
    {
        Smoke.gameObject.SetActive(false);
        Ball.gameObject.SetActive(false);
    }
}
