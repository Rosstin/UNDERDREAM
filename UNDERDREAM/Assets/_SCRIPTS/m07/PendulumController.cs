using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumController : BaseController
{
    [Header("Scene End Conditions")]
    public float SlipAngle;
    public AudioSource SlipSound;

    [Header("Outlets")]
    public GameObject Container;
    public GameObject Head;

    [Header("Motion")]
    public AnimationCurve Curve;
    public float Period;
    public float StartingMaxAngle;
    public float MaxAngleMinimum;
    public float MaxAngleIncreasePerSecond;
    public float PeriodDecreasePerSecond;
    public float HeadOffset;

    [Header("Debug Out")]
    public TMPro.TextMeshProUGUI DebugText;

    private bool slipped = false;
    private float maxAngle;
    private float currentTime;
    private Vector3 headStartPosition;

    new private void Start()
    {
        base.Start();
        headStartPosition = Head.transform.localPosition;
        maxAngle = StartingMaxAngle;
    }

    public void Update()
    {
        BaseUpdate();

        if (!slipped)
        {
            if (maxAngle > SlipAngle 
                &&
                Container.transform.localRotation.z > SlipAngle
                )
            {
                slipped = true;
                SlipSound.Play();
                LoadNextScene();
            }
            else
            {
                UpdateSwingMotion();
            }
        }
        else
        {
        }

    }


    private void UpdateSwingMotion()
    {
        if (currentTime < Period)
        {
            currentTime += 1 * Time.deltaTime;

            Container.transform.localRotation = new Quaternion(Container.transform.localRotation.x, Container.transform.localRotation.y,
                maxAngle - 2f*(maxAngle * Curve.Evaluate(Mathf.Abs(currentTime) / Period)),
                1 * Container.transform.localRotation.w
                );
        }
        else
        {
            currentTime = -Period;
        }

        bool helpingSwing = false;
        bool hurtingSwing = false;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Head.transform.localPosition = new Vector3(headStartPosition.x - HeadOffset, headStartPosition.y, headStartPosition.z);
            if (currentTime > 0)
            {
                helpingSwing = true;
            }
            else
            {
                hurtingSwing = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Head.transform.localPosition = new Vector3(headStartPosition.x + HeadOffset, headStartPosition.y, headStartPosition.z);
            if(currentTime < 0)
            {
                helpingSwing = true;
            }
            else
            {
                hurtingSwing = true;
            }
        }
        else
        {
            Head.transform.localPosition = new Vector3(headStartPosition.x + 0f, headStartPosition.y, headStartPosition.z);
        }

        if (helpingSwing)
        {
            DebugText.text = "helping";
            maxAngle += MaxAngleIncreasePerSecond * Time.deltaTime;
            Period -= PeriodDecreasePerSecond * Time.deltaTime;
        }

        if (hurtingSwing)
        {
            DebugText.text = "hurting";
            maxAngle -= MaxAngleIncreasePerSecond * Time.deltaTime;
            Period += PeriodDecreasePerSecond * Time.deltaTime;
        }

        if(maxAngle < MaxAngleMinimum)
        {
            maxAngle = MaxAngleMinimum;
        }

    }

}
