using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : BaseController
{
    [Header("Hunger")]
    [SerializeField] [Range(0f, 1f)] private float MaxHunger;
    [SerializeField] [Range(0f, 1f)] private float StartingHunger;
    [SerializeField] [Range(0.001f, 0.009f)] private float HungerLossPerSecond;
    [SerializeField] [Range(0.05f, 0.5f)] private float HangerThreshhold;
    [SerializeField] [Range(0f, 9f)] private float hangryDuration;
    [SerializeField] [Range(0f, 2f)] private float hungerBarAppearAfterXSeconds;

    [Header("Movement")]
    [SerializeField] [Range(0.0001f, 0.0009f)] private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject Container;

    [Header("Outlets: Maoxun Animations")]
    [SerializeField] private GameObject IdleAnimation;
    [SerializeField] private GameObject WalkAnimation;
    [SerializeField] private GameObject LookDownAnimation;

    [Header("Outlets: Other Animations")]
    [SerializeField] private GameObject hungerBarAnimation;

    [Header("Outlets: Bounds")]
    [SerializeField] private Transform Feet;
    [SerializeField] private Collider Ground;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Collider myCollider;

    [Header("Outlets: SFX")]
    [SerializeField] private AudioSource hangrySFX;
    [SerializeField] private AudioSource boingSfx;

    public enum MaoxunAnimStateM03
    {
        Idle,
        Walk,
        Hangry
    }
    private MaoxunAnimStateM03 currentState;

    public const float GROUND_ERROR = 0.05f;
    public const float MAX_RAY_DISTANCE = 50f;
    private float distanceToGround = 0f;

    private bool hangry = false;
    private float currentHunger;
    private float hangryTime = 0f;

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

        ActivateAnimation(MaoxunAnimStateM03.Idle);

        currentHunger = StartingHunger;
    }

    public void ActivateAnimation(MaoxunAnimStateM03 anim)
    {
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        LookDownAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimStateM03.Idle:
                IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimStateM03.Walk:
                WalkAnimation.SetActive(true);
                break;
            case MaoxunAnimStateM03.Hangry:
                LookDownAnimation.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        BaseUpdate();

        UpdateGravity();

        UpdateHunger();


        if (!hangry)
        {
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
        }
        else
        {
            UpdateHangry();
        }

    }

    private void UpdateHunger()
    {
        currentHunger = currentHunger - HungerLossPerSecond * Time.deltaTime;

        if (currentHunger < 0f)
        {
            currentHunger = 0f;
        }

        if (currentHunger < HangerThreshhold && !hangry)
        {
            StartHangry();
        }
    }

    private void StartHangry()
    {
        hangry = true;
        hangrySFX.Play();
        ActivateAnimation(MaoxunAnimStateM03.Hangry);
    }

    private void UpdateHangry()
    {
        hangryTime += Time.deltaTime;

        if(hangryTime > hungerBarAppearAfterXSeconds)
        {
            hungerBarAnimation.SetActive(true);
        }

        if (hangryTime > hangryDuration)
        {
            LoadNextScene();
        }
    }

    private void UpdateIdle()
    {
        if (currentState != MaoxunAnimStateM03.Idle)
        {
            ActivateAnimation(MaoxunAnimStateM03.Idle);
            currentState = MaoxunAnimStateM03.Idle;
        }
    }

    private void UpdateGravity()
    {
        if (Ground.Raycast(new Ray(Feet.position, Vector3.down), out RaycastHit hit, MAX_RAY_DISTANCE))
        {
            distanceToGround = hit.distance;
        }
        else {
            distanceToGround = 999f;
        }
    }

    private void UpdateJump()
    {
        if (distanceToGround < GROUND_ERROR)
        {
            ImpartJumpForce();
        }
    }

    private void ImpartJumpForce()
    {
        boingSfx.Play();
        this.myRigidbody.AddForce(new Vector3(-jumpForceForward, jumpForceUp, 0f));
    }

    private void UpdateMoveLeftRight(MoveDirection direction)
    {
        if (currentState != MaoxunAnimStateM03.Walk)
        {
            ActivateAnimation(MaoxunAnimStateM03.Walk);
            currentState = MaoxunAnimStateM03.Walk;
        }

        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        this.transform.localScale = new Vector3(sign, 1, 1);

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (sign * speedMetersPerSecond) / Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }

}
