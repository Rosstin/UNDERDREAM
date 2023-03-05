using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TamaEvent : MonoBehaviour
{
    [Header("Background Scroller")]
    public BackgroundScroller BgScroller;
    [Header("Preview Object Timing")]
    public float PreviewObjectMovePeriod;
    public float PreviewObjectStopPeriod;
    [Header("Tama")]
    public Tama01 Tama;

    public virtual GameObject PreviewObject { get; }
    public virtual GameObject PreviewObjectStartPos { get; }
    public virtual GameObject PreviewObjectEndPos { get; }


    public abstract bool EventCanFire();

    public enum PreviewObjectState
    {
        MOVING,
        STOPPED,
        CUTSCENE,
        DONE
    }

    PreviewObjectState previewObjectState = PreviewObjectState.DONE;

    private float elapsedPreviewMoving = 0f;
    private float elapsedPreviewStopped = 0f;

    private void Reset()
    {
        PreviewObject.SetActive(false);
        previewObjectState = PreviewObjectState.DONE;
        elapsedPreviewMoving = 0f;
        elapsedPreviewStopped = 0f;
        Tama.StartAnimation(Tama01.Tama01AnimationState.WALK);
        BgScroller.EnableScrolling(true);
    }

    private void Awake()
    {
        Reset();
        PreviewObjectStartPos.transform.gameObject.SetActive(false);
        PreviewObjectEndPos.transform.gameObject.SetActive(false);
    }

    public void StartPreviewObject()
    {
        PreviewObject.gameObject.transform.position = PreviewObjectStartPos.transform.position;
        PreviewObject.SetActive(true);
        previewObjectState = PreviewObjectState.MOVING;
        elapsedPreviewMoving = 0f;
    }

    public void Update()
    {
        switch (previewObjectState)
        {
            case PreviewObjectState.MOVING:
                if (elapsedPreviewMoving < PreviewObjectMovePeriod)
                {
                    elapsedPreviewMoving += Time.deltaTime;
                    PreviewObject.gameObject.transform.position
                        = Vector3.Lerp(PreviewObjectStartPos.transform.position, PreviewObjectEndPos.transform.position, elapsedPreviewMoving / PreviewObjectMovePeriod);
                }
                else
                {
                    StartStoppedState();
                }
                break;
            case PreviewObjectState.STOPPED:
                if(elapsedPreviewStopped < PreviewObjectStopPeriod)
                {
                    elapsedPreviewStopped += Time.deltaTime;
                }
                else
                {
                    StartCutsceneState();
                }
                break;
            default:
                break;
        }
    }

    private void StartStoppedState()
    {
        Debug.LogWarning("start stopped state");
        previewObjectState = PreviewObjectState.STOPPED;
        Tama.StartAnimation(Tama01.Tama01AnimationState.IDLE);

        BgScroller.EnableScrolling(false);
    }

    private void StartCutsceneState()
    {
        Debug.LogWarning("StartCutsceneState");

        previewObjectState = PreviewObjectState.CUTSCENE;
        EndPreview();
    }

    private void EndPreview()
    {
        Debug.LogWarning("end preview");
        Reset();
    }

    public void Play()
    {
        StartPreviewObject();
    }

}
