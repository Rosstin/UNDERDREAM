using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingMine : Hurdle
{
    [Header("Motion")]
    public AnimationCurve Curve;
    public float Period;
    public float StartingMaxAngle;

    private float currentTime;
    private Vector3 headStartPosition;

    private void Update()
    {
        if (currentTime < Period)
        {
            currentTime += 1 * Time.deltaTime;

            spriteRenderer.transform.localRotation = new Quaternion(spriteRenderer.transform.localRotation.x, spriteRenderer.transform.localRotation.y,
                StartingMaxAngle - 2f * (StartingMaxAngle * Curve.Evaluate(Mathf.Abs(currentTime) / Period)),
                1 * spriteRenderer.transform.localRotation.w
                );
        }
        else
        {
            currentTime = -Period;
        }
    }

}
