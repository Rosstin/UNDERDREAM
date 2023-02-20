using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float travelTime;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] GameObject movingObject;
    [SerializeField] float startingPercent;
    [SerializeField] bool dontLoop;

    private float elapsedTime = 0;

    private void Start()
    {
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        elapsedTime = (startingPercent / 100f) * travelTime;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float progress = moveCurve.Evaluate(elapsedTime / travelTime);

        movingObject.transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, progress);


        if (!dontLoop && elapsedTime >= travelTime)
        {
            elapsedTime = 0f;
        }


    }
}
