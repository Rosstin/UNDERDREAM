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

    [Header("Animation Outlets")]
    public Animator Awakening;
    public Animator Moving;
    public GameObject SwayLeft;
    public GameObject SwayRight;
    public GameObject SwayMiddle;

    [Header("AR Camera")]
    public Camera ArCamera;

    public enum UjelliState
    {
        Awakening,
        Surprising,
        Swaying
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
        Debug.Log("seconds elapsed: "+secondsElapsed);
        if(secondsElapsed > SurpriseDuration)
        {
            SetState(UjelliState.Swaying);
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

            if (xSway > SwayThreshold)
            {
                currentSway = SwayDirection.Left;
                SwayLeft.gameObject.SetActive(true);
            }
            else if (xSway < -SwayThreshold)
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
            secondsElapsed = 0f;
        }
    }

    // Update is called once per frame
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
        }

        lastCameraPosition = currentCameraPosition;
    }
}
