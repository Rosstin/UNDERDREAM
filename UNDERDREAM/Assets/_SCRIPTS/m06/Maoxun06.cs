using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maoxun06 : BaseController
{
    [Header("Goal")]
    [SerializeField] [Range(0.3f, 0.7f)] private float lemonDistance;
    [SerializeField] private AudioSource snatchSFX;

    [Header("Movement")]
    [SerializeField]
    [Range(0.0001f, 0.0009f)]
    private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpSpeedMargin;

    [Header("Boing SFX")]
    [SerializeField] private AudioSource boingSfx;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject Container;

    [Header("Outlets: Maoxun Animations")]
    [SerializeField] private GameObject IdleAnimation;
    [SerializeField] private GameObject WalkAnimation;

    [Header("Outlets: Bounds")]
    [SerializeField] private Collider Target;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;

    public enum MaoxunAnimState06
    {
        Idle,
        Walk
    }
    private MaoxunAnimState06 currentState;

    private float timeSinceLastJump = 0f;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    new private void Start()
    {
        base.Start();
        ActivateAnimation(MaoxunAnimState06.Idle);
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

        float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);

        if (distanceToTarget < lemonDistance)
        {
            snatchSFX.Play();
            LoadNextScene();
        }

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

        if (!didSomething)
        {
            UpdateIdle();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        if ( timeSinceLastJump > jumpCooldown
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
            this.transform.localPosition.x + (sign * speedMetersPerSecond) / Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }

}
