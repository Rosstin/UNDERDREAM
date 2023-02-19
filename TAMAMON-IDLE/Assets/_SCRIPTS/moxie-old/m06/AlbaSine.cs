using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbaSine : MonoBehaviour
{
    #region inspector
    [Header("Movement Values")]
    [SerializeField]
    [Range(0f, 1f)]
    private float xValue;
    [SerializeField] [Range(0f, 1f)] private float yValue;

    [Header("Periods")]
    [SerializeField]
    [Range(0f, 5f)]
    private float xPeriod;
    [SerializeField] [Range(0f, 5f)] private float yPeriod;

#endregion

    private Vector3 _startPosition;

    private float currentTimeX = 0f;
    private float currentTimeY = 0f;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        currentTimeX += Time.deltaTime;
        currentTimeY += Time.deltaTime;

        transform.position = _startPosition
                + new Vector3(
                    Mathf.Sin(currentTimeX/xPeriod) * xValue,
                    Mathf.Cos(currentTimeY/yPeriod) * yValue,
                    0.0f
            );
    }
}
