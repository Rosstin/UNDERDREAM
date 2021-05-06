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

    public Camera Cam1;
    public Camera Cam2;

    public MaoxunM23 Moxie;

    private void Start()
    {
        P1Text.gameObject.SetActive(true);
        P2Text.gameObject.SetActive(false);
        Cam1.gameObject.SetActive(true);
        Cam2.gameObject.SetActive(false);
    }

    public void GetBall()
    {
        WithBall.enabled = true;
        WithoutBall.enabled = false;
        CaughtSFX.Play();
        P1Text.gameObject.SetActive(false);
        P2Text.gameObject.SetActive(true);
        Cam1.gameObject.SetActive(false);
        Cam2.gameObject.SetActive(true);
    }

}
