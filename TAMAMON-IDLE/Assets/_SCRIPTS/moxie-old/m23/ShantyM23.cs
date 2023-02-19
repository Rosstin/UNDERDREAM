using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantyM23 : MonoBehaviour
{
    public MaoxunM23 Moxie;

    public SpriteRenderer WithoutBall;
    public SpriteRenderer WithBall;
    public AudioSource CaughtSFX;

    public ShantyText P1Text;
    public ShantyText P2Text;

    public Camera Cam1;
    public Camera Cam2;

    public GameObject YesBubble;
    public GameObject NoBubble;

    public GameObject PartTwoContainer;

    private void Start()
    {
        P1Text.gameObject.SetActive(true);
        Cam1.gameObject.SetActive(true);
        PartTwoContainer.SetActive(false);
        YesBubble.gameObject.SetActive(false);
        NoBubble.gameObject.SetActive(false);
    }

    public void GetBall()
    {
        PartTwoContainer.SetActive(true);
        WithBall.enabled = true;
        WithoutBall.enabled = false;
        CaughtSFX.Play();
        P1Text.gameObject.SetActive(false);
        P2Text.gameObject.SetActive(true);
        Cam1.gameObject.SetActive(false);
        Cam2.gameObject.SetActive(true);

        Moxie.StartPart2();
    }

    private void Update()
    {
        if(P2Text.CurrentShout > 3)
        {
            YesBubble.gameObject.SetActive(true);
            NoBubble.gameObject.SetActive(true);
        }
    }

}
