using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAfterDelay : MonoBehaviour
{
    public AudioSource sfx;
    public float delaySeconds;

    private float elapsed = 0f;
    private bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        elapsed = 0f;
        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (done)
        {
            return;
        }
        elapsed += Time.deltaTime;
        if (elapsed > delaySeconds)
        {
            done = true;
            sfx.Play();
        }
    }
}
