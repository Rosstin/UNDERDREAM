using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jitter : MonoBehaviour
{
    [SerializeField] private float jitterPeriod;
    [SerializeField] private Vector3 jitter;

    private float jitterElapsed = 0;
    private Vector3 startingPosition;

    public bool JitterEnabled = true;

    public void SetJitter(bool jitterOn)
    {
        Debug.Log("set jitt " + jitterOn);
        JitterEnabled = jitterOn;
    }

    public void JitterForDuration(float durSeconds)
    {
        StartCoroutine(JitterForDur(durSeconds));
    }

    private IEnumerator JitterForDur(float durSeconds)
    {
        JitterEnabled = true;
        yield return new WaitForSeconds(durSeconds);
        JitterEnabled = false;
    }

    private void Start()
    {
        ResetStartingPosition();
    }

    public void ResetStartingPosition()
    {
        JitterEnabled = false;
        startingPosition = this.gameObject.transform.localPosition;
    }

    void Update()
    {
        if (JitterEnabled)
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

}
