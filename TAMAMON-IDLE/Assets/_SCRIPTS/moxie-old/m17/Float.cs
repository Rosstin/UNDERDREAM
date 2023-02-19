using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [Header("Animation")]
    public Vector2 FloatAmounts;
    public Vector2 FloatPeriods;

    private Vector3 startLocalPosition;
    private float currentTime=0f;

    private void Start()
    {
        startLocalPosition = this.transform.localPosition;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        this.gameObject.transform.localPosition =
            startLocalPosition
            + new Vector3(
            Mathf.Sin(currentTime/FloatPeriods.x) * FloatAmounts.x,
            Mathf.Cos(currentTime/FloatPeriods.y) * FloatAmounts.y,
            0
            );
    }
}
