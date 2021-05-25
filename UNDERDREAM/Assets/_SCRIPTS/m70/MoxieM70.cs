using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxieM70 : BaseController
{
    [Header("Biting Stern")]
    public BoxCollider2D SternCollider;
    public BoxCollider2D BiteCollider;
    [SerializeField] private float biteForceUp;
    [SerializeField] private float biteForceBackward;
    [SerializeField] private float biteCooldown;
    [SerializeField] private AudioSource biteSfx;
    [SerializeField] private SternShipM70 sternShip;

    [Header("Shanty Animation")]
    public GameObject ShantyBiteAnimation;
    public GameObject ShantyIdleAnimation;

    [Header("Outlets")]
    public BoxCollider2D GroundCollider;

    [Header("Goal")]
    [SerializeField] private AudioSource snatchSFX;

    [Header("Movement")]
    [SerializeField][Range(1f, 9f)]private float speedMetersPerSecond;
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
    [SerializeField] private GameObject JumpKickAnimation;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;

    public enum MaoxunAnimState70
    {
        Idle,
        Walk,
        JumpKick
    }

    private MaoxunAnimState70 currentMoxieAnimState;
    private float timeSinceLastJump = 0f;
    private bool airborne = false;
    private bool kicking = false;

    public enum ShantyAnimStateM70
    {
        Idle,
        Bite
    }

    private ShantyAnimStateM70 currentShantyAnimState;
    private float timeSinceLastBite = 0f;


    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private new void Start()
    {
        base.Start();

        this.myRigidbody.gravityScale = 1f;
        this.Container.SetActive(true);
    }

    public void ActivateShantyAnimation(ShantyAnimStateM70 anim)
    {
        currentShantyAnimState = anim;
        ShantyIdleAnimation.SetActive(false);
        ShantyBiteAnimation.SetActive(false);
        switch (anim)
        {
            case ShantyAnimStateM70.Bite:
                ShantyBiteAnimation.SetActive(true);
                break;
            case ShantyAnimStateM70.Idle:
                ShantyIdleAnimation.SetActive(true);
                break;
        }
    }

    public void ActivateAnimation(MaoxunAnimState70 anim)
    {
        currentMoxieAnimState = anim;
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        JumpKickAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimState70.Idle:
                IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimState70.Walk:
                WalkAnimation.SetActive(true);
                break;
            case MaoxunAnimState70.JumpKick:
                JumpKickAnimation.SetActive(true);
                break;
        }
    }

    private void SetMobile(bool mobile)
    {
        if (!mobile)
        {
            this.myRigidbody.freezeRotation = true;
            this.myRigidbody.velocity = Vector2.zero;
        }
        else
        {
            this.myRigidbody.freezeRotation = false;
        }
    }

    private void BiteStern()
    {
        timeSinceLastBite = 0f;
        ActivateShantyAnimation(ShantyAnimStateM70.Bite);
        biteSfx.Play();
        this.myRigidbody.AddForce(new Vector2(biteForceBackward, biteForceUp));
        sternShip.GetBit();
    }

    public void Update()
    {
        timeSinceLastJump += Time.deltaTime;
        timeSinceLastBite += Time.deltaTime;

        BaseUpdate();

        bool didSomething = false;

        if (BiteCollider.IsTouching(SternCollider) && timeSinceLastBite > biteCooldown)
        {
            // play shanty's bite animation and bounce backwards
            BiteStern();
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

    private IEnumerator EnableMyColliderInABit()
    {
        yield return new WaitForSeconds(0.5f);
        this.myCollider.enabled = true;
    }

    private void UpdateIdle()
    {
        if (currentMoxieAnimState != MaoxunAnimState70.Idle && !kicking)
        {
            ActivateAnimation(MaoxunAnimState70.Idle);
        }
    }

    private void AttemptKick()
    {
        kicking = true;
        ActivateAnimation(MaoxunAnimState70.JumpKick);
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
        if (!kicking && currentMoxieAnimState != MaoxunAnimState70.Walk)
        {
            ActivateAnimation(MaoxunAnimState70.Walk);
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
