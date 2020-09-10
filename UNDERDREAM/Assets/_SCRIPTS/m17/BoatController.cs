using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoatController : BaseController
{
    [Header("Part Two")]
    [Range(40f, 44f)] public float PartTwoTime; // about 42 seconds or so into Bumblebee2.wav
    [Range(70f, 100f)] public float PartThreeTime; // about 80 seconds or so in Bumblebee2.wav
    public Vector3 CamPosP2;
    public Camera MainCam;
    public GameObject Old;
    public GameObject New;
    public BoxCollider2D OldGroundCollider;
    public BoxCollider2D NewGroundCollider;
    public Vector3 LowerAmountP2;

    [Header("Stern")]
    public SternShipM17 Stern;

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

    [Header("Outlets")]
    [SerializeField] private GameObject container;
    [SerializeField] private Rigidbody2D myRigidbody;
    public BoxCollider2D MyCollider;
    public BoxCollider2D SternCollider;
    public FlyingLemon Lemon;

    private float timeSinceLastJump = 0f;
    private float elapsed =0f;
    private int takenDamage = 0;
    private float invincibilityCooldownElapsed=0f;

    private bool startP2 = false;
    private bool startP3 = false;

    private Vector3 lemonInitialLocalPosition;

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

    public Collider2D GetCurrentGroundCollider()
    {
        if (!startP2)
        {
            return OldGroundCollider;
        }
        else
        {
            return NewGroundCollider;
        }
    }

    private void Start()
    {
        Old.gameObject.SetActive(true);
        New.gameObject.SetActive(false);
        lemonInitialLocalPosition = Lemon.gameObject.transform.localPosition;

        base.Start();
        DamageSprites[0].gameObject.SetActive(true);
    }

    private void Update()
    {
        BaseUpdate();

        timeSinceLastJump += Time.deltaTime;
        elapsed += Time.deltaTime;
        invincibilityCooldownElapsed += Time.deltaTime;

        if (!startP2 && elapsed > PartTwoTime)
        {
            // zoom out, start spawning sharks
            startP2 = true;
            MainCam.gameObject.transform.position = CamPosP2;
            CamJitter.ResetStartingPosition();

            Old.gameObject.SetActive(false);
            New.gameObject.SetActive(true);

            this.gameObject.transform.position += LowerAmountP2;
            SternCollider.gameObject.transform.position += LowerAmountP2;
            Stern.StartPartTwo();
            Lemon.gameObject.transform.localPosition = lemonInitialLocalPosition;
        }

        if (!startP3 && elapsed > PartThreeTime)
        {
            startP3 = true;
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

        if (startP2)
        {
            // do specific new updates for things like the sharks
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
