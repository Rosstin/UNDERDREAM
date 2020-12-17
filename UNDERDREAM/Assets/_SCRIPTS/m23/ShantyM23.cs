using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantyM23 : MonoBehaviour
{
    public SpriteRenderer WithoutBall;
    public SpriteRenderer WithBall;
    public AudioSource CaughtSFX;

    public ShantyText P1Text;
    public ShantyText P2Text;

    public MaoxunM23 Moxie;

    public void GetBall()
    {
        WithBall.enabled = true;
        WithoutBall.enabled = false;
        CaughtSFX.Play();
        P1Text.gameObject.SetActive(false);
        P2Text.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (P2Text.IsFinished())
        {
            Moxie.LoadNextScene();
        }
    }
}
