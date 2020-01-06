using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxLoop : MonoBehaviour
{
    [SerializeField] private float period;
    [SerializeField] private AudioSource sfx;

    private float currentTime = 0f;

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > period)
        {
            currentTime = 0f;
            sfx.Play();
        }
    }
}
