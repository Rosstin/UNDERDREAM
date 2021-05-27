using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [Header("Seek Shanty")]
    public MoxieM70 Shanty;
    [SerializeField] [Range(0f, 10f)] private float lerpAmountPerSecond;

    [Header("Outlets")]
    public Rigidbody2D Body;
    public Collider2D MyCollider;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    [Header("SFX")]
    public AudioSource Boing1;
    public AudioSource Boing2;
    [Range(0f, 1f)] public float Boing1Prob;

    [Header("Velocity Threshold")]
    [Range(0.001f, 0.03f)]
    public float VelocityThreshold;
    [Range(0f, 0.03f)] public float StartBoingTime;

    private float elapsed = 0f;

    private bool finished = false;

    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Fly()
    {
        if (!finished)
        {
            finished = true;
            Debug.LogWarning("hat Fly");
            this.transform.parent = null;
            Body.AddForce(StartingForce);
            Body.AddTorque(StartingTorque);
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > StartBoingTime
            && MyCollider.IsTouchingLayers(LayerMask.NameToLayer("Ground"))
            && !BoingPlaying()
            && Body.velocity.magnitude > VelocityThreshold)
        {
            if (Random.Range(0f, 1f) > Boing1Prob)
            {
                Boing1.Play();
            }
            else
            {
                Boing2.Play();
            }
        }

        this.transform.position = Vector3.Lerp(this.transform.position, Shanty.transform.position, lerpAmountPerSecond*Time.deltaTime);

        if (MyCollider.IsTouching(Shanty.BiteCollider))
        {
            // play click sound todo
            Debug.LogWarning("shanty go saiyan from hat");
            this.gameObject.SetActive(false);
            Shanty.ShantyGoesSaiyan();
        }
    }

    /// <summary>
    /// Freeze in place
    /// </summary>
    public void Freeze()
    {
        Body.velocity = Vector2.zero;
        Body.gravityScale = 0f;
        Body.freezeRotation = true;
    }

    private bool BoingPlaying()
    {
        return Boing1.isPlaying || Boing2.isPlaying;
    }
}
