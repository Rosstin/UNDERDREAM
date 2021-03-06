﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maoxun08 : BaseController
{
    [Header("Goal")]
    [SerializeField] [Range(0.1f, 2f)] private float lemonDistance;
    [SerializeField] private AudioSource snatchSFX;

    [Header("Movement")]
    [SerializeField] [Range(1f, 9f)] private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject Container;

    [Header("Outlets: Maoxun Animations")]
    [SerializeField] private GameObject IdleAnimation;
    [SerializeField] private GameObject WalkAnimation;
    [SerializeField] private GameObject Scarf;

    [Header("Outlets: Bounds")]
    [SerializeField] private BoxCollider2D Target;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField] private Collider2D lemonGrabCollider;

    [Header("Outlet: Boing SFX")]
    [SerializeField] private AudioSource boingSfx;

    public enum MaoxunAnimState06
    {
        Idle,
        Walk
    }
    private MaoxunAnimState06 currentState;

    private float timeSinceLastJump = 0f;

    private bool visible;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private float elapsed = 0f;

    new private void Start()
    {
        base.Start();
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;

        if (!visible)
        {
            this.IdleAnimation.SetActive(visible);
            this.WalkAnimation.SetActive(visible);
            this.Scarf.SetActive(visible);
            this.lemonGrabCollider.enabled = false;
        }
        else
        {
            this.lemonGrabCollider.enabled = true;
            this.Scarf.SetActive(visible);
            this.IdleAnimation.SetActive(visible);
        }
    }

    public void ActivateAnimation(MaoxunAnimState06 anim)
    {
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimState06.Idle:
                IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimState06.Walk:
                WalkAnimation.SetActive(true);
                break;
        }
    }

    public void Update()
    {
        timeSinceLastJump += Time.deltaTime;

        BaseUpdate();

        if (!visible) return;

        float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);
        if (distanceToTarget < lemonDistance || Target.IsTouching(this.lemonGrabCollider))
        {
            snatchSFX.Play();
            LoadNextScene();
        }

        bool didSomething = false;
        if (CommandsHeldThisFrame.ContainsKey(Command.Up) || CommandsHeldThisFrame.ContainsKey(Command.Fire))
        {
            didSomething = true;
            UpdateJump();
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Left))
        {
            didSomething = true;
            UpdateMoveLeftRight(MoveDirection.Left);
        }
        else if (CommandsHeldThisFrame.ContainsKey(Command.Right))
        {
            didSomething = true;
            UpdateMoveLeftRight(MoveDirection.Right);
        }

        if (!didSomething)
        {
            UpdateIdle();
        }
    }

    private void UpdateIdle()
    {
        if (currentState != MaoxunAnimState06.Idle)
        {
            ActivateAnimation(MaoxunAnimState06.Idle);
            currentState = MaoxunAnimState06.Idle;
        }
    }

    private void UpdateJump()
    {
        if ( timeSinceLastJump > jumpCooldown)
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
        if (currentState != MaoxunAnimState06.Walk)
        {
            ActivateAnimation(MaoxunAnimState06.Walk);
            currentState = MaoxunAnimState06.Walk;
        }

        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        this.transform.localScale = new Vector3(sign * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (sign * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }

}
