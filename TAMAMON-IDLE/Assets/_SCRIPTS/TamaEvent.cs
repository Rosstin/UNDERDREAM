using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TamaEvent : MonoBehaviour
{
    [Header("Background Scroller")]
    public BackgroundScroller BgScroller;
    [Header("How Long It Takes the Preview Object To Arrive")]
    public float PreviewObjectPeriod;

    public virtual GameObject PreviewObject { get; }
    public virtual GameObject PreviewObjectStartPos { get; }
    public virtual GameObject PreviewObjectEndPos { get; }


    public abstract bool EventCanFire();

    private bool previewObjectMoving = false;

    private float elapsed = 0f;

    private void Reset()
    {
        PreviewObject.SetActive(false);
        previewObjectMoving = false;
        elapsed = 0f;
    }

    private void Awake()
    {
        Reset();
    }

    public void StartPreviewObject()
    {
        PreviewObject.gameObject.transform.position = PreviewObjectStartPos.transform.position;
        PreviewObject.SetActive(true);
        previewObjectMoving = true;
        elapsed = 0f;
    }

    public void Update()
    {
        if (previewObjectMoving && elapsed < PreviewObjectPeriod)
        {
            elapsed += Time.deltaTime;
            PreviewObject.gameObject.transform.position 
                = Vector3.Lerp(PreviewObjectStartPos.transform.position, PreviewObjectEndPos.transform.position, elapsed/PreviewObjectPeriod);
        }

        if(elapsed >= PreviewObjectPeriod)
        {
            EndPreview();
        }
    }

    private void EndPreview()
    {
        Reset();
    }


    public void Play()
    {
        StartPreviewObject();
    }

}
