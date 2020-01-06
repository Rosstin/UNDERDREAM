using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    [SerializeField] private float jitterPeriod;
    [SerializeField] private Vector3 jitter;

    private float jitterElapsed = 0;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = this.gameObject.transform.localPosition;
    }

    void Update()
    {
        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > jitterPeriod)
        {
            jitterElapsed = 0f;
            this.gameObject.transform.localPosition = startingPosition
                + new Vector3(
                    Random.Range(-jitter.x, jitter.x),
                    Random.Range(-jitter.y, jitter.y),
                    Random.Range(-jitter.z, jitter.z));
        }
    }
}
