using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBall : MonoBehaviour
{
    [Header("Self Outlets")]
    public Rigidbody2D rb;
    public CircleCollider2D col;

    [Header("Release Motion")]
    public float Torque;
    public Vector2 Force;
    public float Gravity;

    [Header("Shanty")]
    public BoxCollider2D ShantyCollider;
    public ShantyM23 Shanty;

    [Header("Moxie")]
    public MaoxunM23 Moxie;

    [Header("Caught Pos")]
    public Transform CaughtPos;

    public void Release()
    {
        rb.AddForce(Force);
        rb.AddTorque(Torque);
        rb.gravityScale = Gravity;
        col.enabled = true;
    }

    void Start()
    {
        col.enabled = false;
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (this.col.IsTouching(ShantyCollider))
        {
            Shanty.GetBall();
            this.transform.position = CaughtPos.position;
            this.rb.freezeRotation = true;
            this.rb.gravityScale = 0f;
            this.col.enabled = false;
            this.rb.velocity = Vector2.zero;

            //Moxie.LoadNextScene();
        }
    }
    

}
