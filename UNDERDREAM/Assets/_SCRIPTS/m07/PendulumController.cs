using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : BaseController
{
    [Header("Outlets")]
    public GameObject Container;
    public GameObject Head;

    [Header("Motion")]
    public AnimationCurve Curve;
    public float Period;
    public float StartingMaxAngle;
    public float MaxAngleIncreasePerSecond;
    public float PeriodDecreasePerSecond;
    public float HeadOffset;

    private float maxAngle;
    private float currentTime;
    private int sign = 1;
    private Vector3 headStartPosition;

    void Start()
    {
        headStartPosition = Head.transform.localPosition;
        maxAngle = StartingMaxAngle;
    }

    public void Update()
    {
        BaseUpdate();
        if (sign == 1)
        {
            if (currentTime < Period)
            {
                currentTime += sign * Time.deltaTime;

                Container.transform.localRotation = new Quaternion(Container.transform.localRotation.x, Container.transform.localRotation.y,
                    maxAngle * Curve.Evaluate(Mathf.Abs(currentTime) / Period),
                    sign*Container.transform.localRotation.w
                    );
            }
            else
            {
                sign *= -1;
            }

        }
        else if (sign == -1)
        {
            if (currentTime > -Period)
            {
                currentTime += sign * Time.deltaTime;
                Container.transform.localRotation = new Quaternion(Container.transform.localRotation.x, Container.transform.localRotation.y,
                    maxAngle * Curve.Evaluate(Mathf.Abs(currentTime) / Period),
                    Container.transform.localRotation.w
                    );
            }
            else
            {
                sign *= -1;
            }
        }




        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Head.transform.localPosition = new Vector3(headStartPosition.x-HeadOffset, headStartPosition.y, headStartPosition.z);
            maxAngle += MaxAngleIncreasePerSecond * Time.deltaTime;
            Period -= PeriodDecreasePerSecond * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Head.transform.localPosition = new Vector3(headStartPosition.x + HeadOffset, headStartPosition.y, headStartPosition.z);
            maxAngle += MaxAngleIncreasePerSecond * Time.deltaTime;
            Period -= PeriodDecreasePerSecond * Time.deltaTime;
        }



    }
}
