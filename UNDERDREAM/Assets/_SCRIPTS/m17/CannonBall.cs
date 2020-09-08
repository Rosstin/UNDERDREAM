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

    [Header("SFX")]
    public AudioSource CrashSfx;

    private void Update()
    {
        if (MyCollider.IsTouching(Ground))
        {
            // play splash sfx
            // play splash anim
            // become invis
            this.SetVisible(false);
            this.transform.localPosition = Vector3.zero;
        }
        else if (MyCollider.IsTouching(Player.MyCollider))
        {
            StartCoroutine(HitPlayer());
        }
    }

    private IEnumerator HitPlayer()
    {
        Debug.Log("hit player");
        Player.TakeDamage();
        CrashSfx.Play();
        this.SetVisible(false);
        this.transform.localPosition = Vector3.zero;
        yield return 0;
    }

    public void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }
}
