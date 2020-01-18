using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UjelliController : MonoBehaviour
{
    [Header("Awakening Configurations")]
    public float SpeedThreshold;
    public int FramesCooldown;

    [Header("Moving Configurations")]
    public float SurpriseDuration;

    [Header("Swaying Configurations")]
    public float SwayThreshold;
    public float SwayCooldown;
    public float SwayDuration;

    [Header("Normal Configurations")]
    public GameObject Lemon;

    [Header("Eating Configurations")]
    public float LemonMoveSpeed;
    public float LemonClosenessThreshhold;
    public float LemonScaledownPerFrame;
    public float EatingSuckDelay;
    public float LemonScaleThreshhold;

    [Header("Gulp Configurations")]
    public float GulpDuration;

    [Header("Animation Outlets")]
    public Animator Awakening;
    public Animator Moving;
    public GameObject SwayLeft;
    public GameObject SwayRight;
    public GameObject SwayMiddle;
    public GameObject Normal;
    public GameObject Eating;
    public GameObject Suck;
    public Animator Gulp;

    [Header("AR Camera")]
    public Camera ArCamera;

    public enum UjelliState
    {
        Awakening,
        Surprising,
        Swaying,
        Normal,
        Eating,
        Gulp
    }

    private UjelliState myState;

    private Vector3 currentCameraPosition;
    private Vector3 lastCameraPosition;

    private int framesElapsed = 0;
    private float secondsElapsed = 0f;

    #region SwayState
    private SwayDirection currentSway;
    private SwayDirection lastSway;
    public enum SwayDirection
    {
        Left,
        Right,
        Middle
    }
    private float timeInSwayState=0f;
    #endregion

    void Start()
    {
        Debug.Log("UJELLI CONTROLLER START");
        SetState(UjelliState.Awakening);
        lastCameraPosition = ArCamera.transform.position;
    }

    public void SetState(UjelliState state)
    {
        framesElapsed = 0;
        secondsElapsed = 0f;
        Awakening.gameObject.SetActive(false);
        Moving.gameObject.SetActive(false);
        SwayLeft.gameObject.SetActive(false);
        SwayRight.gameObject.SetActive(false);
        SwayMiddle.gameObject.SetActive(false);
        Normal.gameObject.SetActive(false);
        Eating.gameObject.SetActive(false);
        Suck.gameObject.SetActive(false);
        Gulp.gameObject.SetActive(false);

        switch (state)
        {
            case UjelliState.Awakening:
                Debug.LogWarning("AWAKENING");
                myState = UjelliState.Awakening;
                Awakening.gameObject.SetActive(true);
                break;
            case UjelliState.Surprising:
                Debug.LogWarning("SURPRISING");
                myState = UjelliState.Surprising;
                Moving.gameObject.SetActive(true);
                break;
            case UjelliState.Swaying:
                Debug.LogWarning("SWAYING");
                myState = UjelliState.Swaying;
                SwayMiddle.gameObject.SetActive(true);
                break;
            case UjelliState.Normal:
                Debug.LogWarning("NORMAL");
                myState = UjelliState.Normal;
                Normal.gameObject.SetActive(true);
                break;
            case UjelliState.Eating:
                Debug.LogWarning("EATING");
                myState = UjelliState.Eating;
                Eating.gameObject.SetActive(true);
                Lemon.transform.SetParent(this.transform);
                break;
            case UjelliState.Gulp:
                Debug.LogWarning("GULP");
                myState = UjelliState.Gulp;
                Lemon.gameObject.SetActive(false);
                Gulp.gameObject.SetActive(true);
                break;
        }

    }

    private void UpdateAwakening(float cameraMoveSpeed)
    {
        if (cameraMoveSpeed > SpeedThreshold && framesElapsed > FramesCooldown)
        {
            Debug.Log("broke speed threshhold: " + cameraMoveSpeed);

            SetState(UjelliState.Surprising);
        }
    }

    private void UpdateSurprising()
    {
        if(secondsElapsed > SurpriseDuration)
        {
            SetState(UjelliState.Swaying);
        }
    }

    private void UpdateNormal()
    {
        if (Lemon.activeSelf)
        {
            SetState(UjelliState.Eating);
        }
    }

    private void UpdateEating()
    {
        if(secondsElapsed > EatingSuckDelay)
        {
            Suck.SetActive(true);
            Eating.SetActive(false);
            // suck in the lemon
            float step = LemonMoveSpeed * Time.deltaTime;
            Lemon.transform.position = Vector3.MoveTowards(Lemon.transform.position, this.transform.position, step);

            if (Vector3.Distance(Lemon.transform.position, this.transform.position) < LemonClosenessThreshhold)
            {
                Lemon.transform.localScale *= LemonScaledownPerFrame;
            }

            if(Lemon.transform.localScale.x < LemonScaleThreshhold)
            {
                SetState(UjelliState.Gulp);
            }


        }
    }

    private void UpdateSwaying(Vector3 curPos, Vector3 lastPos)
    {
        if(secondsElapsed > SwayCooldown)
        {
            SwayLeft.gameObject.SetActive(false);
            SwayRight.gameObject.SetActive(false);
            SwayMiddle.gameObject.SetActive(false);

            float xSway = curPos.x - lastPos.x;

            if (xSway < SwayThreshold)
            {
                currentSway = SwayDirection.Left;
                SwayLeft.gameObject.SetActive(true);
            }
            else if (xSway > -SwayThreshold)
            {
                currentSway = SwayDirection.Right;
                SwayRight.gameObject.SetActive(true);
            }
            else
            {
                currentSway = SwayDirection.Middle;
                SwayMiddle.gameObject.SetActive(true);
            }

            lastSway = currentSway;
            timeInSwayState += secondsElapsed;
            secondsElapsed = 0f;
        }

        if (timeInSwayState > SwayDuration)
        {
            SetState(UjelliState.Normal);
        }
    }

    private void UpdateGulp()
    {
        if(secondsElapsed > GulpDuration)
        {
            //TODO - GO TO SLEEPY STATE
        }
    }

    void Update()
    {
        framesElapsed++;
        secondsElapsed += Time.deltaTime;

        currentCameraPosition = ArCamera.transform.position;
        float cameraMoveSpeed = Vector3.Distance(currentCameraPosition, lastCameraPosition) / Time.deltaTime;

        switch (myState)
        {
            case UjelliState.Awakening:
                UpdateAwakening(cameraMoveSpeed);
                break;
            case UjelliState.Surprising:
                UpdateSurprising();
                break;
            case UjelliState.Swaying:
                UpdateSwaying(currentCameraPosition, lastCameraPosition);
                break;
            case UjelliState.Normal:
                UpdateNormal();
                break;
            case UjelliState.Eating:
                UpdateEating();
                break;
        }

        lastCameraPosition = currentCameraPosition;
    }
}
