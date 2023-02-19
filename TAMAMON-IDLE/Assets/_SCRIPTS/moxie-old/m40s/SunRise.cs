using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRise : MonoBehaviour
{
    [SerializeField] private Transform bottomEdge;
    [SerializeField] private Transform topEdge;
    [SerializeField] private float travelTime;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] GameObject sun;
    [SerializeField] float startingPercent;

    private float elapsedTime = 0;

    private void Start()
    {
        bottomEdge.gameObject.SetActive(false);
        topEdge.gameObject.SetActive(false);

        elapsedTime = (startingPercent / 100f) * travelTime;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        sun.transform.position = Vector3.Lerp(bottomEdge.transform.position, topEdge.transform.position, elapsedTime / travelTime);


        if (elapsedTime >= travelTime)
        {
            elapsedTime = 0f;
        }


    }
}
