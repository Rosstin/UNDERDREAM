using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Outlets - Personal")]
    public CircleCollider2D MyCollider;
    public Rigidbody2D MyRigidBody;
    public SpriteRenderer MySpriteRenderer;

    [Header("Outlets - Scene")]
    public BoxCollider2D Ground;
    public BoatController Player;

    [Header("VFX")]
    public Animator SplashEffect;
    public Vector3 SplashEffectOffset;

    [Header("SFX")]
    public AudioSource CrashSfx;
    public AudioSource Splash;

    private void Update()
    {
        if (MyCollider.IsTouching(Ground))
        {
            StartCoroutine(SplashWater());
        }
        else if (MyCollider.IsTouching(Player.MyCollider))
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
        Player.TakeDamage();
        CrashSfx.Play();
        this.SetVisible(false);
        yield return 0;
    }

    public void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }
}
