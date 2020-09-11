using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternM20 : MonoBehaviour
{
    [Header("My Outlets")]
    public AudioSource SternChuckle2;
    public AudioSource SnatchSFX;
    public BoxCollider2D MyCollider;

    [Header("Lemon Outlet")]
    public FlyingLemon Lemon;

    private bool caughtLemon = false;

    void Update()
    {
        if (!caughtLemon &&
            Lemon.MyCollider.IsTouching(this.MyCollider))
        {
            caughtLemon = true;
            Lemon.Freeze();
            SnatchSFX.Play();
            SternChuckle2.Play();
        }        
    }
}
