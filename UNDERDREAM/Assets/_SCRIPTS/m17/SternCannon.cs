using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternCannon : MonoBehaviour
{
    [Header("Outlets")]
    public Animator MyAnimator;
    public CannonBall Ball;
    public Animator Smoke;

    [Header("Rotation Bounds")]
    public Transform[] FireRotations; //list of potential fire rots

    private Quaternion initialLocalRotation;

    private int currentFireRotation = 0;

    private void Awake()
    {
        Smoke.gameObject.SetActive(false);
        Ball.gameObject.SetActive(false);
        initialLocalRotation = this.transform.localRotation;
    }

    public void RandomRotation()
    {
        var randomRotationIndex = Random.Range(0, FireRotations.Length);
        currentFireRotation = randomRotationIndex;
        this.transform.localRotation = FireRotations[randomRotationIndex].localRotation;
    }
}
