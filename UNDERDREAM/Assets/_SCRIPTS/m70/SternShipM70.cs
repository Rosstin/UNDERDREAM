using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternShipM70 : MonoBehaviour
{
    [Header("Missile Launcher")]
    [SerializeField] private Transform fullOpenTopPosition;
    [SerializeField] private Transform fullClosedTopPosition;
    [SerializeField] private GameObject shipTop;
    [SerializeField] private float opennessDecayRatioPerSecond;
    [SerializeField] private float crankAmount;

    [Header("Damage Scars")]
    [SerializeField] private Animator damageAnimation;
    [SerializeField][Range(3, 11)] private int numDamageFrames;
    [SerializeField] private MoxieM70 player;
    [SerializeField][Range(0,1)] private float smallSmokeAppearHealth;
    [SerializeField] private GameObject smallSmoke;
    [SerializeField] [Range(0, 1)] private float bigSmokeAppearHealth;
    [SerializeField] private GameObject bigSmoke;
    [SerializeField] private SternPanic sternPanic;

    [Header("Stern Body Movement")]
    public GameObject SternBody;
    public BoxCollider2D SternCollider;

    [Header("Ship Jump")]
    public Vector2 JumpForce;

    [Header("Main Camera Jitter Outlet")]
    public Jitter CamJitter;

    [Header("My Rigidbody")]
    public Rigidbody2D MyRigidbody;

    [Header("Cannon Effect On Stern")]
    public Vector2 CannonFireEffectOnStern;

    [Header("Stern Forward Movement")]
    public Vector2 BoatVelocity;

    [Header("Cannon Outlets")]
    public SternCannon FrontCannon;
    public SternCannon BackCannon;

    [Header("SFX")]
    public AudioSource CymbalSfx;
    public AudioSource CannonSfx;
    public float CymbalDelay;
    public float CannonDelay;
    public float ScreenShakeDuration;
    public AudioSource SternChuckle1;
    public AudioSource SternChuckle2;
    public AudioSource SternBigLaugh;

    private bool partTwoStarted = false;
    private bool updateVelocity = true;

    private float elapsed; 
    private float fElapsed; // elapsed for front can
    private float bElapsed; // elapsed for back can
    private float twoElapsed; // elapsed for stern rising

    private Vector3 sternInitialLocalPos;

    private bool sternRisen = false;

    private Vector2 currentKnockback;
    private Vector2 currentV;

    private float topOpennessRatio;

    private int damageIndex = 0;

    private void Start()
    {
        FrontCannon.MyAnimator.speed = 2f;
        BackCannon.MyAnimator.speed = 2f;
        sternInitialLocalPos = SternBody.transform.localPosition;

        currentKnockback = CannonFireEffectOnStern;
        currentV= BoatVelocity;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        fElapsed += Time.deltaTime;
        bElapsed += Time.deltaTime;

        // decay top cranking
        topOpennessRatio -= opennessDecayRatioPerSecond * Time.deltaTime;
        // clamp the min
        if(topOpennessRatio < 0)
        {
            topOpennessRatio = 0;
        }

        shipTop.transform.localPosition = Vector3.LerpUnclamped(fullClosedTopPosition.localPosition, fullOpenTopPosition.localPosition, topOpennessRatio);

        if (updateVelocity)
        {
            MyRigidbody.velocity = new Vector2(currentV.x, MyRigidbody.velocity.y);
        }
    }


    public void GetBit()
    {
        damageIndex++;
        damageAnimation.speed = 0f;

        float damageRatio = ((float)damageIndex) / (((float)numDamageFrames) * 1f);

        damageAnimation.Play("ship-bottom", 0, damageRatio);

        if(damageRatio >= smallSmokeAppearHealth && damageRatio < bigSmokeAppearHealth)
        {
            smallSmoke.gameObject.SetActive(true);
        }
        else if(damageRatio >= bigSmokeAppearHealth)
        {
            bigSmoke.gameObject.SetActive(true);
            smallSmoke.gameObject.SetActive(false);
        }
        else
        {
            smallSmoke.gameObject.SetActive(false);
            bigSmoke.gameObject.SetActive(false);
        }

        if (damageRatio >= 1f)
        {
            sternPanic.LoseHat();
            //player.LoadNextScene();
        }
    }

    public void CrankTop()
    {
        // play crank sfx
        // play stern anim
        OpenTopBy(crankAmount);
    }

    public void OpenTopBy(float additionalRatio)
    {
        topOpennessRatio += additionalRatio;
    }

    public void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        updateVelocity = false;
        MyRigidbody.AddForce(JumpForce);
        yield return new WaitForSeconds(0.2f);
        updateVelocity = true;
    }

    public void FireFrontCannon()
    {
        StartCoroutine(FireCannon(FrontCannon));
    }

    public void FireBackCannon()
    {
        StartCoroutine(FireCannon(BackCannon));
    }

    private IEnumerator FireCannon(SternCannon cannon)
    {
        updateVelocity = false;
        cannon.MyAnimator.SetTrigger("fire");

        cannon.RandomRotation();

        yield return new WaitForSeconds(CymbalDelay);
        CymbalSfx.Play();
        yield return new WaitForSeconds(CannonDelay);
        CannonSfx.Play();
        cannon.Smoke.gameObject.SetActive(true);
        cannon.Smoke.SetTrigger("fire");
        CamJitter.SetJitter(true);
        MyRigidbody.AddForce(currentKnockback);
        cannon.FireBall();
        yield return new WaitForSeconds(ScreenShakeDuration);
        cannon.Smoke.gameObject.SetActive(false);
        CamJitter.SetJitter(false);
        updateVelocity = true;
    }
}
