using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    [Header("Seek Shanty")]
    public MoxieM70 Shanty;
    [SerializeField] [Range(0f, 10f)] private float lerpAmountPerSecond;
    [SerializeField] [Range(0f, 1f)] private float lerpIncreasePerSecond;
    public Transform SeekSpot;
    public float CollidePeriod;

    [Header("Outlets")]
    public Rigidbody2D Body;
    public Collider2D MyCollider;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    [Header("Velocity Threshold")]
    [Range(0.001f, 0.03f)]
    public float VelocityThreshold;

    [Header("Click Sound")]
    public AudioSource ClickSFX;

    private float elapsed = 0f;
    private bool finished = false;

    public void Start()
    {
    }

    public void Fly()
    {
        if (!finished)
        {
            this.gameObject.SetActive(true);
            finished = true;
            this.transform.parent = null;
            Body.AddForce(StartingForce);
            Body.AddTorque(StartingTorque);
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        lerpAmountPerSecond += Time.deltaTime * lerpIncreasePerSecond;

        this.transform.position = Vector3.Lerp(this.transform.position, SeekSpot.transform.position, lerpAmountPerSecond*Time.deltaTime);

        if (MyCollider.IsTouching(Shanty.BiteCollider) || elapsed > CollidePeriod)
        {
            // play click sound todo
            ClickSFX.Play();
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

}
