using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLemon : MonoBehaviour
{
    [Header("Outlets")]
    public Rigidbody2D Body;
    public Collider2D MyCollider;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    [Header("SFX")]
    public AudioSource Boing1;
    public AudioSource Boing2;
    [Range(0f,1f)] public float Boing1Prob;

    [Header("Velocity Threshold")]
    [Range(0.001f, 0.03f)] public float VelocityThreshold; 
    [Range(0f, 0.03f)] public float StartBoingTime;

    private float elapsed=0f;

    // Start is called before the first frame update
    void Start()
    {
        Body.AddForce(StartingForce);
        Body.AddTorque(StartingTorque);
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
    }

    private bool BoingPlaying()
    {
        return Boing1.isPlaying || Boing2.isPlaying;
    }

}
