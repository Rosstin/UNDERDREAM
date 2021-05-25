using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallM70 : CannonBall
{
    [Header("Outlets - Scene")]
     public MoxieM70 PlayerM70;

    private void Update()
    {
        if (MyCollider.IsTouching(PlayerM70.GroundCollider))
        {
            StartCoroutine(SplashWater());
        }
        else if (MyCollider.IsTouching(PlayerM70.BiteCollider))
        {
            StartCoroutine(HitPlayer());
        }
    }

    private IEnumerator SplashWater()
    {
        SplashEffect.gameObject.SetActive(true);
        SplashEffect.gameObject.transform.position = this.gameObject.transform.position + SplashEffectOffset;
        SplashEffect.SetTrigger("splash");
        Splash.Play();
        this.SetVisible(false);
        yield return 0;
    }

    private IEnumerator HitPlayer()
    {
        PlayerM70.TakeDamage();
        this.SetVisible(false);
        yield return 0;
    }
}
