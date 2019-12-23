using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudloop : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private float travelTime;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] GameObject cloud;
    [SerializeField] float startingPercent;

    private float elapsedTime=0;

    private void Start()
    {
        leftEdge.gameObject.SetActive(false);
        rightEdge.gameObject.SetActive(false);

        elapsedTime = (startingPercent / 100f) * travelTime;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        cloud.transform.position = Vector3.Lerp(leftEdge.transform.position, rightEdge.transform.position, elapsedTime/travelTime);


        if(elapsedTime >= travelTime)
        {
            elapsedTime = 0f;
        }


    }
}
