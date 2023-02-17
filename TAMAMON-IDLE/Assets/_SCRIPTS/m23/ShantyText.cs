using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantyText : MonoBehaviour
{
    [Header("Shouts (also please set the bool for each shout)")]
    public List<string> Shouts;
    public List<bool> IsShortBig;

    [Header("Short Shouts (small bubble, big text)")]
    public TMPro.TextMeshPro ShortBigTextOut;

    [Header("Long Shouts (big bubble, small text)")]
    public TMPro.TextMeshPro LongSmallTextOut;

    [Header("Shout Duration")]
    public float ShoutDuration;

    [Header("Outlets")]
    public GameObject ShortBigBubble;
    public GameObject LongSmallBubble;

    public Vector3 Jitter;
    public float JitterPeriod;

    private bool looped=false;

    private float jitterElapsed = 0f;
    private float elapsed = 0f;
    private int currentShout = -1;

    private Vector3 startingPosition;

    public int CurrentShout => currentShout;

    public bool IsFinished() {
        return looped;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.gameObject.transform.localPosition;
        elapsed = ShoutDuration;
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

        // new shout
        if (elapsed > ShoutDuration)
        {
            elapsed = 0f;
            currentShout++;
            if (currentShout >= Shouts.Count)
            {
                currentShout = 0;
                looped = true;
            }

            // big short text like HI!
            if (IsShortBig[currentShout])
            {
                ShortBigTextOut.gameObject.SetActive(true);
                ShortBigBubble.SetActive(true);
                LongSmallTextOut.gameObject.SetActive(false);
                LongSmallBubble.SetActive(false);

                ShortBigTextOut.text = Shouts[currentShout];
            }
            // small long text like CAN YOU GET MY BALL?
            else
            {
                ShortBigTextOut.gameObject.SetActive(false);
                ShortBigBubble.SetActive(false);
                LongSmallTextOut.gameObject.SetActive(true);
                LongSmallBubble.SetActive(true);

                LongSmallTextOut.text = Shouts[currentShout];
            }
        }
    }
}