using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaoxunM23 : BaseController
{
    [Header("Start")]
    public GameObject ScareSprite;
    public float ScareTime;

    [Header("Outlets")]
    public BoxCollider2D GroundCollider;

    [Header("Movement")]
    [SerializeField] [Range(1f, 9f)] private float speedMetersPerSecond;
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
    [SerializeField] private BoxCollider2D footCollider;

    [Header("Outer: Tree Collider")]
    [SerializeField] private Island island;

    [Header("Coconut Struck Stuff")]
    public AudioSource SandHitSFX;
    public GameObject BonkedMoxie;
    public GameObject Sandpile;
    public Transform BonkedLocation;

    [Header("Coconut Struck Stuff: Wiggle")]
    public float WiggleCooldown;
    public AudioSource PopOutSfx;
    public Vector3 WiggleOffset;
    public int EscapeWiggles;

    [Header("Coconut Struck Stuff: Escape")]
    public Vector2 EscapeForce;
    public float EscapeTorque;
    public float EscapeCooldown;

    [Header("Coconut Struck Stuff: Nut")]
    public Quaternion InitialRotation;
    public GameObject AlbaStarsAnimation;

    [Header("Ball Stuff")]
    public BeachBall Ball;

    [Header("Part Two")]
    public Transform PartTwoHorizontalPosition;
    public float PartTwoJumpForceUp;
    public BoxCollider2D TreeCollider;
    public BoxCollider2D YesCollider;
    public BoxCollider2D NoCollider;

    [Header("Part Three")]
    public Camera YesCamera;
    public Camera NoCamera;
    public Camera Cam1;
    public Camera Cam2;
    public AudioSource CannonSfx;

    public enum MaoxunAnimState23
    {
        Idle,
        Walk,
        JumpKick
    }

    private MaoxunAnimState23 currentState;

    private float timeSinceLastJump = 0f;

    private bool doneScared = false;
    private float scareElapsed = 0f;

    private bool airborne = false;

    private bool kicking = false;

    // related to coconut strike
    private int numWiggles = 0;
    private bool struckSand = false;
    private float wiggleElapsed = 0f;
    private float escapeElapsed = 0f;
    private bool escaped = false;
    private bool landed = false;
    private bool struck = false;
    private bool currentlyFlippingOut = false;
    private bool released;

    private bool part2 = false;
    private bool part3 = false;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public void StartYes()
    {
        part3 = true;
        Time.timeScale = 0.1f;
        CannonSfx.Play();
        YesCamera.gameObject.SetActive(true);
    NoCamera.gameObject.SetActive(false); 
     Cam1.gameObject.SetActive(false); 
    Cam2.gameObject.SetActive(false); 

}

public void StartNo()
    {
        part3 = true;
        Time.timeScale = 0.1f;
        CannonSfx.Play();
        YesCamera.gameObject.SetActive(false);
        NoCamera.gameObject.SetActive(true);
        Cam1.gameObject.SetActive(false);
        Cam2.gameObject.SetActive(false);
    }

    public void StartPart2()
    {
        part2 = true;
        speedMetersPerSecond = 0f;
        this.transform.position = new Vector3(PartTwoHorizontalPosition.position.x, this.transform.position.y, this.transform.position.z);
        jumpForceUp = PartTwoJumpForceUp;
        TreeCollider.enabled = false;
    }

    private new void Start()
    {
        base.Start();
        ActivateAnimation(MaoxunAnimState23.Idle);
        BonkedMoxie.SetActive(false);
        Sandpile.SetActive(false);

        AlbaStarsAnimation.gameObject.SetActive(false);

        InitialRotation = this.transform.rotation;

        this.myRigidbody.gravityScale = 0f;
        this.Container.SetActive(false);
        ScareSprite.gameObject.SetActive(true);
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
                if(!part2)
                    WalkAnimation.SetActive(true);
                else
                    IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimState23.JumpKick:
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

    public void GetBonked()
    {
        AlbaStarsAnimation.gameObject.SetActive(true);
        SandHitSFX.Play();
        this.myRigidbody.gravityScale = 0;
        SetMobile(false);
        escaped = false;
        numWiggles = 0;
        wiggleElapsed = 0;
        struckSand = true;
        currentlyFlippingOut = false;

        this.myCollider.enabled = false;
        Container.gameObject.SetActive(false);
        Sandpile.gameObject.SetActive(true);
        BonkedMoxie.gameObject.SetActive(true);

        this.gameObject.transform.position = BonkedLocation.transform.position;
    }

    public void Update()
    {
        timeSinceLastJump += Time.deltaTime;

        BaseUpdate();


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
                        Container.gameObject.SetActive(true);
                        BonkedMoxie.SetActive(false);
                        Sandpile.SetActive(false);

                        AlbaStarsAnimation.gameObject.SetActive(false);

                        currentlyFlippingOut = true;

                        PopOutSfx.Play();
                        escaped = true;
                        this.myRigidbody.gravityScale = 1;

                        // enable this in a little bit
                        ActivateAnimation(MaoxunAnimState23.Idle);
                        StartCoroutine(EnableMyColliderInABit());

                        myRigidbody.AddForce(EscapeForce);
                        myRigidbody.AddTorque(EscapeTorque);
                    }
                }
            }
        }
        else if (currentlyFlippingOut)
        {
            // wait until you land then give back control to the player and right moxie
            if (myCollider.IsTouching(GroundCollider))
            {
                // right her
                this.transform.rotation = InitialRotation;
                SetMobile(true);
                currentlyFlippingOut = false;
            }
        }
        else
        {
            scareElapsed += Time.deltaTime;

            if (!doneScared && scareElapsed > ScareTime)
            {
                doneScared = true;
                ScareSprite.gameObject.SetActive(false);
                this.Container.gameObject.SetActive(true);
                this.myRigidbody.gravityScale = 1f;
            }

            // if ur airborne and hit space, jumpkick
            if (doneScared && !currentlyFlippingOut)
            {
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

                if (kicking
                    && footCollider.IsTouching(island.TreeCollider)
                    && island.IsReady()
                    )
                {
                    int nutIndex = island.ShakeTree();
                    if (nutIndex > 5 && !released)
                    {
                        released = true;
                        Ball.Release();
                        //LoadNextScene();
                    }
                    //kicking = false;
                    // push her away a little bit
                }

                if(!part3 &&part2 && kicking && footCollider.IsTouching(YesCollider))
                {
                    StartYes();
                }else if(!part3 &&part2 && kicking && footCollider.IsTouching(NoCollider))
                {
                    StartNo();
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
        }




    }

    private IEnumerator EnableMyColliderInABit()
    {
        yield return new WaitForSeconds(0.5f);
        this.myCollider.enabled = true;
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
            this.transform.localPosition.x + (sign * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }

}
