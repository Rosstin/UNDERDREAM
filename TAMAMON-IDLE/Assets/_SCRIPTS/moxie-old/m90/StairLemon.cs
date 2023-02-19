using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairLemon : MonoBehaviour
{
    [Header("Stairs")]
    public LineRenderer line;
    public float pointTime;
    public float z;

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
    [Range(0.001f, 0.03f)] public float VelocityThreshold;
    [Range(0f, 0.03f)] public float StartBoingTime;

    private float elapsed = 0f;
    private int index = 0;

    private Vector3 oldPos;

    private bool frozen = false;

    // Start is called before the first frame update
    void Start()
    {
        oldPos = this.transform.position;

        Body.AddForce(StartingForce);
        Body.AddTorque(StartingTorque);

        for(int i = 0; i < line.positionCount;i++)
        {
            line.SetPosition(i,
                new Vector3(line.GetPosition(i).x, line.GetPosition(i).y, z));
        }

    }

    private void Update()
    {
        if (!frozen)
        {


        elapsed += Time.deltaTime;

        float progress = elapsed / pointTime;
        this.transform.position = Vector3.Lerp(oldPos, line.GetPosition(index), progress);


        if(elapsed > pointTime)
        {
            oldPos = this.transform.position;
            elapsed = 0f;
            index++;
            //this.transform.position = line.GetPosition(index);
        }


            /*

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
            */
        }
    }

    /// <summary>
    /// Freeze in place
    /// </summary>
    public void Freeze()
    {
        frozen = true;
        Body.velocity = Vector2.zero;
        Body.gravityScale = 0f;
        Body.freezeRotation = true;
    }

    private bool BoingPlaying()
    {
        return Boing1.isPlaying || Boing2.isPlaying;
    }
}
