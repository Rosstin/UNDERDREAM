using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //public List<AudioSource> sfxList;


    public AudioSource splat1;
    public AudioSource splat2;
    public AudioSource pop;
    public AudioSource ohno;
    public AudioSource fling;
    public AudioSource tada;


    public void PlayPop()
    {
        pop.Play();
    }

    public void PlayRandomSplatSfx()
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            splat1.Play();
        }
        else
        {
            splat2.Play();
        }

    }
}
