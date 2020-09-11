using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maoxun20 : BaseController
{
    [Header("Initial Things")]
    public Vector2 StartForce;
    public float StartTorque;
    public BoxCollider2D SandCollider;
    public AudioSource SandHitSFX;
    public BoatM20 MyBoatM20;

    [Header("Wiggle")]
    public float WiggleCooldown;
    public AudioSource PopOutSfx;
    public Vector3 WiggleOffset;
    public int EscapeWiggles;

    [Header("Escape")]
    public BoxCollider2D ColliderStand;
    public Vector2 EscapeForce;
    public float EscapeTorque;
    public float NextSceneTime;
    public float EscapeCooldown;

    [Header("Exit Condition")]
    public SternM20 Stern;

    [Header("Movement")]
    [SerializeField] [Range(0.0001f, 0.0009f)] private float speedMetersPerSecond;
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

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;

    public enum MaoxunAnimState06
    {
        Idle,
        Walk
    }

    private int numWiggles=0;

    private bool struckSand=false;
    private bool toldBoatToDrown = false;

    private MaoxunAnimState06 currentState;

    private float timeSinceLastJump = 0f;

    private float wiggleElapsed=0f;
    private float escapeElapsed = 0f;
    private bool escaped = false;

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
        this.myRigidbody.AddForce(StartForce);
        this.myRigidbody.AddTorque(StartTorque);
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

        if (!struckSand &&
            this.myCollider.IsTouching(SandCollider))
        {
            struckSand = true;
            SandHitSFX.Play();
            this.myRigidbody.gravityScale = 0;
            this.myRigidbody.freezeRotation = true;
            this.myRigidbody.velocity = Vector2.zero;
        }

        if (!toldBoatToDrown&& struckSand)
        {
            if (Stern.IsSternGone())
            {
                toldBoatToDrown = true;
                MyBoatM20.Drown();
            }
        }

        if (struckSand && !escaped)
        {
            wiggleElapsed += Time.deltaTime;
            if (
                Input.GetKeyDown(KeyCode.Space)
                ||
                Input.GetKeyDown(KeyCode.LeftArrow)
                ||
                Input.GetKeyDown(KeyCode.RightArrow)
                ||
                Input.GetKeyDown(KeyCode.UpArrow)
                ||
                Input.GetKeyDown(KeyCode.DownArrow)
            )
            {

                if (wiggleElapsed > WiggleCooldown)
                {
                    //wiggle
                    numWiggles++;
                    SandHitSFX.Play();
                    wiggleElapsed = 0f;
                    this.transform.localPosition += WiggleOffset;
                    if (numWiggles > EscapeWiggles)
                    {
                        PopOutSfx.Play();
                        escaped = true;

                        this.myRigidbody.gravityScale = 1;
                        this.myRigidbody.freezeRotation = false;
                        myRigidbody.AddForce(EscapeForce);
                        myRigidbody.AddTorque(EscapeTorque);
                    }
                }
            }

        }

        if (escaped)
        {
            escapeElapsed += Time.deltaTime;
            if (escapeElapsed > EscapeCooldown && myCollider.IsTouching(ColliderStand))
            {
                SandHitSFX.Play();
                this.myRigidbody.gravityScale = 0;
                this.myRigidbody.freezeRotation = true;
                this.myRigidbody.velocity = Vector2.zero;

                // wait then load next scene
                LoadNextScene(NextSceneTime);

            }
        }

        var didSomething = false;
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
