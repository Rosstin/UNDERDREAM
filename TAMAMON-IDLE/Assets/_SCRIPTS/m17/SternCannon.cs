using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternCannon : MonoBehaviour
{
    [Header("Outlets")]
    public Animator MyAnimator;
    public CannonBall Ball;
    public Animator Smoke;
    public Transform FireOrigin;

    [Header("Rotation Bounds")]
    public Transform[] FireRotations; //list of potential fire rots

    [Header("Fire Force")]
    public float[] FireForces; // list of force powers for each rot

    private Quaternion initialLocalRotation;

    private int currentFireRotation = 0;

    private void Awake()
    {
        Smoke.gameObject.SetActive(false);
        Ball.SetVisible(false);
        initialLocalRotation = this.transform.localRotation;
    }

    public void RandomRotation()
    {
        var randomRotationIndex = Random.Range(0, FireRotations.Length);
        currentFireRotation = randomRotationIndex;
        this.transform.localRotation = FireRotations[randomRotationIndex].localRotation;
    }

    public void FireBall()
    {
        Ball.SetVisible(true);
        Ball.gameObject.transform.localPosition = FireOrigin.transform.localPosition;
        Ball.MyRigidBody.AddForce(FireOrigin.transform.forward * FireForces[currentFireRotation]);
    }
}
