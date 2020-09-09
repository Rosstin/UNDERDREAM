﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoatController : BaseController
{
    [Header("Scene Time")]
    [SerializeField] [Range(20f, 100f)] private float SceneTime;

    [Header("Damage")]
    public GameObject[] DamageSprites;
    public Vector2 KnockbackForceCannon;
    public Vector2 KnockbackForceSternTouch;
    public float DamageCooldownTime;
    public Jitter CamJitter;
    public AudioSource CrashSfx;
    [Range(0f, 2f)] public float CamJitterDuration;

    [Header("Jump")]
    [SerializeField] [Range(0.0001f, 0.0009f)] private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpSpeedMargin;
    [SerializeField] private AudioSource boingSfx;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject container;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    public BoxCollider2D MyCollider;

    [Header("Outlets: Stern")]
    public BoxCollider2D SternCollider;

    private float timeSinceLastJump = 0f;
    private float elapsed =0f;
    private int takenDamage = 0;
    private float invincibilityCooldownElapsed=0f;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// take damage and get more beat up
    /// </summary>
    public void TakeDamage(Vector2 knockbackForce)
    {
        if (invincibilityCooldownElapsed > DamageCooldownTime)
        {
            invincibilityCooldownElapsed = 0f;
            takenDamage++;

            if (takenDamage >= DamageSprites.Length)
            {
                LoadHintScene();
            }
            else // actually take the damage
            {
                CrashSfx.Play();
                CamJitter.JitterForDuration(CamJitterDuration);
                this.myRigidbody.AddForce(knockbackForce);
                DamageSprites[takenDamage - 1].gameObject.SetActive(false);
                DamageSprites[takenDamage].gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("still invincible");
        }

    }

    private void Start()
    {
        base.Start();
        DamageSprites[0].gameObject.SetActive(true);
    }

    private void Update()
    {
        BaseUpdate();

        timeSinceLastJump += Time.deltaTime;
        elapsed += Time.deltaTime;
        invincibilityCooldownElapsed += Time.deltaTime;

        if (elapsed > SceneTime)
        {
            LoadNextScene();
        }

        if (this.MyCollider.IsTouching(SternCollider))
        {
            TakeDamage(KnockbackForceSternTouch);
        }

        BaseUpdate();

        bool didSomething = false;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            didSomething = true;
            UpdateJump();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            didSomething = true;
            UpdateMoveLeftRight(MoveDirection.Left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            didSomething = true;
            UpdateMoveLeftRight(MoveDirection.Right);
        }

    }

    private void UpdateJump()
    {
        if (timeSinceLastJump > jumpCooldown
            && Mathf.Abs(this.myRigidbody.velocity.y) < jumpSpeedMargin
        )
        {
            timeSinceLastJump = 0f;
            ImpartJumpForce();
        }
    }

    private void ImpartJumpForce()
    {
        boingSfx.Play();
        this.myRigidbody.AddForce(new Vector2(-jumpForceForward, jumpForceUp));
    }

    private void UpdateMoveLeftRight(MoveDirection direction)
    {
        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        this.transform.localPosition
            = new Vector3(
                this.transform.localPosition.x + (sign * speedMetersPerSecond) / Time.deltaTime,
                this.transform.localPosition.y,
                this.transform.localPosition.z
            );
    }


}
