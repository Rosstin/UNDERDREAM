using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Rigidbody")]
    public Rigidbody rb;

    [Header("Sfx")]
    public AudioSource thock;
    
    private GameObject leftWall;
    private GameObject center;
    private GameObject rightWall;

    public void Shoot(Vector3 shootVector, float speed)
    {
        rb.velocity = shootVector * speed;
    }

    private void Bounce()
    {
        thock.Play();
        Vector3 vel = new Vector3(-rb.velocity.x, rb.velocity.y, 0);
        rb.velocity = vel;
    }
    
    void Update()
    {
        //Debug.Log("rb.velocity " + rb.velocity);
        // also check velocity is increasing in X
        if (rb.velocity.x > 0 && this.transform.position.x > rightWall.transform.position.x)
        {
            Bounce();
        }
        else if (rb.velocity.x < 0 && this.transform.position.x < leftWall.transform.position.x)
        {
            Bounce();
        }
    }

    public void Init(GameObject leftWall, GameObject center, GameObject rightWall)
    {
        this.leftWall = leftWall;
        this.center = center;
        this.rightWall = rightWall;
    }
}
