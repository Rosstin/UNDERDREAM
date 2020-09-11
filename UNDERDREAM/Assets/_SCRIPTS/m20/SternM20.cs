using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternM20 : MonoBehaviour
{
    [Header("My Outlets")]
    public AudioSource SternChuckle2;
    public AudioSource SnatchSFX;
    public BoxCollider2D MyCollider;
    public Float MyFloat;

    [Header("Stern Leaves")]
    public GameObject SternBody;
    public Transform SternGonePosition;
    public AnimationCurve SternGoneCurve;
    public float SternGonePeriod;

    [Header("Lemon Outlet")]
    public FlyingLemon Lemon;

    private bool caughtLemon = false;
    private bool left = false;

    private float leaveElapsed = 0f;
    private Vector3 sternInitialPos;

    public bool IsSternGone()
    {
        return left;
    }

    private void Start()
    {
        sternInitialPos= SternBody.transform.localPosition;
    }

    private void Update()
    {
        if (!caughtLemon &&
            Lemon.MyCollider.IsTouching(this.MyCollider))
        {
            caughtLemon = true;
            MyFloat.enabled = false;
            Lemon.Freeze();
            SnatchSFX.Play();
            SternChuckle2.Play();
            Lemon.transform.parent = SternBody.transform;
        }

        if (caughtLemon && !left)
        {
            leaveElapsed += Time.deltaTime;

            SternBody.transform.localPosition =
                Vector3.Lerp(sternInitialPos, SternGonePosition.localPosition,
                    SternGoneCurve.Evaluate(
                        leaveElapsed / SternGonePeriod
                    )
                );

            if (leaveElapsed > SternGonePeriod)
            {
                left = true;
            }

        }
    }
}
