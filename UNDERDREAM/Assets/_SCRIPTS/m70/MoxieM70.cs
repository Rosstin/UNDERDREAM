using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxieM70 : BaseController
{
    [Header("Super")]
    [SerializeField]private GameObject bigBubble;
    [SerializeField]private float etherealStartTime;
    [SerializeField] private float marginSeconds;
    [SerializeField] private GameMasterM70 gameMaster;
    [SerializeField] private BoxCollider2D laser;
    [SerializeField] private float gunDestroyTime;

    [Header("Damage")]
    [SerializeField] private float damageCooldown;
    [SerializeField] private Vector2 knockbackForceCannonball;

    [Header("Biting Stern")]
    public BoxCollider2D SternCollider;
    public BoxCollider2D BiteCollider;
    [SerializeField] private float biteForceUp;
    [SerializeField] private float biteForceBackward;
    [SerializeField] private float biteCooldown;
    [SerializeField] private AudioSource biteSfx;
    [SerializeField] private SternShipM70 sternShip;
    [SerializeField] [Range(0.1f, 5f)] private float biteTime;

    [Header("Shanty Animation")]
    public GameObject ShantyBiteAnimation;
    public GameObject ShantyIdleAnimation;
    [SerializeField] private GameObject SuperClosedMouthAnimation;

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
    [SerializeField] private GameObject RideAnimation;
    [SerializeField] private GameObject PanicAnimation;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D myCollider;

    public enum MaoxunAnimState70
    {
        Ride,
        Panic
    }

    private MaoxunAnimState70 currentMoxieAnimState;
    private float timeSinceLastJump = 0f;
    private bool airborne = false;
    private bool kicking = false;
    private float invincibilityCooldownElapsed = 0f;

    private float saiyanElapsed = 0f;

    private bool shantyIsSaiyan = false;

    public enum ShantyAnimStateM70
    {
        Idle,
        Bite,
        SuperClosed,
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

        bigBubble.gameObject.SetActive(false);
        ActivateAnimation(MaoxunAnimState70.Ride);
        ActivateShantyAnimation(ShantyAnimStateM70.Idle);

        this.myRigidbody.gravityScale = 1f;
        this.Container.SetActive(true);
    }

    public void ShantyGoesSaiyan()
    {
        if(shantyIsSaiyan == false)
        {
            bigBubble.gameObject.SetActive(true);
            shantyIsSaiyan = true;
            this.myRigidbody.gravityScale = 0f;
            ActivateAnimation(MaoxunAnimState70.Panic);
            ActivateShantyAnimation(ShantyAnimStateM70.SuperClosed);
            ZeroForce();

            gameMaster.SkipNextUpdate();

            BiteCollider.enabled = false;

            // skip to the ethereal part
            if (Mathf.Abs(Data.GetSabreSong().time - etherealStartTime) > marginSeconds)
            {
                
                Data.GetSabreSong().time = (etherealStartTime);
            }

        }
    }

    public void ActivateShantyAnimation(ShantyAnimStateM70 anim)
    {
        currentShantyAnimState = anim;
        ShantyIdleAnimation.SetActive(false);
        SuperClosedMouthAnimation.SetActive(false);
        ShantyBiteAnimation.SetActive(false);
        switch (anim)
        {
            case ShantyAnimStateM70.Bite:
                ShantyBiteAnimation.SetActive(true);
                break;
            case ShantyAnimStateM70.Idle:
                ShantyIdleAnimation.SetActive(true);
                break;
            case ShantyAnimStateM70.SuperClosed:
                SuperClosedMouthAnimation.SetActive(true);
                break;
        }
    }

    public void ActivateAnimation(MaoxunAnimState70 anim)
    {
        currentMoxieAnimState = anim;
        RideAnimation.SetActive(false);
        PanicAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimState70.Ride:
                RideAnimation.SetActive(true);
                break;
            case MaoxunAnimState70.Panic:
                PanicAnimation.SetActive(true);
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

    /// <summary>
    /// take damage and get more beat up
    /// </summary>
    public void TakeDamage()
    {
        if (invincibilityCooldownElapsed > damageCooldown)
        {
            invincibilityCooldownElapsed = 0f;

            //CrashSfx.Play();
            //CamJitter.JitterForDuration(CamJitterDuration);
            this.myRigidbody.AddForce(knockbackForceCannonball);
        }
        else
        {
            Debug.Log("still invincible");
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
        BaseUpdate();

        if (shantyIsSaiyan)
        {
            saiyanElapsed += Time.deltaTime;
            if( saiyanElapsed > gunDestroyTime &&
            laser.IsTouching(SternCollider)){
                //todo explosions
                LoadNextScene();
            }


            if (CommandsHeldThisFrame.ContainsKey(Command.Up))
            {
                UpdateFloat(MoveDirection.Up);
            }else if (CommandsHeldThisFrame.ContainsKey(Command.Down))
            {
                UpdateFloat(MoveDirection.Down);

            }

            if (CommandsHeldThisFrame.ContainsKey(Command.Left))
            {
                UpdateFloat(MoveDirection.Left);
            }
            else if (CommandsHeldThisFrame.ContainsKey(Command.Right))
            {
                UpdateFloat(MoveDirection.Right);
            }
        }
        else { 


        invincibilityCooldownElapsed += Time.deltaTime;
    timeSinceLastJump += Time.deltaTime;
        timeSinceLastBite += Time.deltaTime;

        if(!shantyIsSaiyan&& timeSinceLastBite> biteTime)
        {
            ActivateShantyAnimation(ShantyAnimStateM70.Idle);
        }


        bool didSomething = false;

        if (!shantyIsSaiyan&& BiteCollider.IsTouching(SternCollider) && timeSinceLastBite > biteCooldown)
        {
            // play shanty's bite animation and bounce backwards
            BiteStern();
        }

        if (CommandsStartedThisFrame.ContainsKey(Command.Up) || CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {
            didSomething = true;

                AttemptJump();
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

    }
    }

    private IEnumerator EnableMyColliderInABit()
    {
        yield return new WaitForSeconds(0.5f);
        this.myCollider.enabled = true;
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
    private void ZeroForce()
    {
        this.myRigidbody.velocity = Vector3.zero;
        this.myRigidbody.angularVelocity = 0;
    }

    private void UpdateFloat(MoveDirection dir)
    {
        ZeroForce();
        int horizAmount = 0;
        if (dir == MoveDirection.Right)
        {
            horizAmount = 1;
        }else if(dir == MoveDirection.Left)
        {
            horizAmount = -1;
        }

        int vertAmount = 0;
        if (dir == MoveDirection.Up )
        {
            vertAmount = 1;
        }else if(dir == MoveDirection.Down)
        {
            vertAmount = -1;
        }

        //this.transform.localScale = new Vector3(1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        //if (horizAmount != 0)
        //{
        //this.transform.localScale = new Vector3(1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        //bigBubble.transform.localScale = new Vector3(-horizAmount*bigBubble.transform.localScale.x, bigBubble.transform.localScale.y, bigBubble.transform.localScale.z);
        //}

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (horizAmount * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.y + (vertAmount*speedMetersPerSecond)*Time.deltaTime,
            this.transform.localPosition.z
            );

    }

    private void UpdateMoveLeftRight(MoveDirection direction)
    {
        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        //this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (sign * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }
}
