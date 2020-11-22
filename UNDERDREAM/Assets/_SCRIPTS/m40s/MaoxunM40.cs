using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaoxunM40 : MonoBehaviour
{
    [Header("Outlets")]
    public BoxCollider2D GroundCollider;

    [Header("Goal")]
    [SerializeField]
    private AudioSource snatchSFX;

    [Header("Movement")]
    [SerializeField]
    [Range(0f, 0.0009f)]
    private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpSpeedMargin;

    [Header("Boing SFX")]
    [SerializeField]
    private AudioSource boingSfx;

    [Header("Outlets: Container")]
    [SerializeField]
    private GameObject Container;

    [Header("Outlets: Maoxun Animations")]
    [SerializeField]
    private GameObject IdleAnimation;
    [SerializeField] private GameObject WalkAnimation;
    [SerializeField] private GameObject JumpKickAnimation;

    [Header("Outlets: Components")]
    [SerializeField]
    private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;

    public enum MaoxunAnimState23
    {
        Idle,
        Walk,
        JumpKick
    }

    private MaoxunAnimState23 currentState;

    private float timeSinceLastJump = 0f;

    private bool airborne = false;

    private bool kicking = false;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private new void Start()
    {
        ActivateAnimation(MaoxunAnimState23.Idle);
    }

    public void ActivateAnimation(MaoxunAnimState23 anim)
    {
        currentState = anim;
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        JumpKickAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimState23.Idle:
                IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimState23.Walk:
                WalkAnimation.SetActive(true);
                break;
            case MaoxunAnimState23.JumpKick:
                JumpKickAnimation.SetActive(true);
                break;
        }
    }

    public void Update()
    {
        timeSinceLastJump += Time.deltaTime;

        var didSomething = false;

        if (this.myCollider.IsTouching(GroundCollider))
        {
            airborne = false;
            kicking = false;
        }
        else
        {
            didSomething = true;
            airborne = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            didSomething = true;

            if (airborne)
            {
                AttemptKick();
            }
            else
            {
                AttemptJump();
            }
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



    }

    private void UpdateIdle()
    {
        if (currentState != MaoxunAnimState23.Idle && !kicking)
        {
            ActivateAnimation(MaoxunAnimState23.Idle);
        }
    }

    private void AttemptKick()
    {
        kicking = true;
        ActivateAnimation(MaoxunAnimState23.JumpKick);
    }

    private void AttemptJump()
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
        if (!kicking && currentState != MaoxunAnimState23.Walk)
        {
            ActivateAnimation(MaoxunAnimState23.Walk);
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
