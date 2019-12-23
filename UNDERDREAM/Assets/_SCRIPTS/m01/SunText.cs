using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunText : MonoBehaviour
{
    [SerializeField] private List<string> sunShouts;
    [SerializeField] private float shoutDuration;
    [SerializeField] private TMPro.TextMeshPro textOut;

    public Vector3 Jitter;
    public float JitterPeriod;

    private float jitterElapsed = 0f;
    private float elapsed = 0f;
    private int currentShout = 0;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.gameObject.transform.localPosition;
        textOut.text = sunShouts[currentShout];
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > JitterPeriod)
        {
            jitterElapsed = 0f;
            this.gameObject.transform.localPosition = startingPosition
    + new Vector3(
    Random.Range(-Jitter.x, Jitter.x),
    Random.Range(-Jitter.y, Jitter.y),
    Random.Range(-Jitter.z, Jitter.z));

        }

        if (elapsed > shoutDuration)
        {
            elapsed = 0f;
            currentShout++;
            if (currentShout >= sunShouts.Count)
            {
                currentShout = 0;
            }

            textOut.text = sunShouts[currentShout];
        }
    }
}
