using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Outlets")]
    public CircleCollider2D MyCollider;
    public Rigidbody2D MyRigidBody;
    public BoxCollider2D Ground;

    private void Update()
    {
        if (MyCollider.IsTouching(Ground))
        {
            // play splash sfx
            // play splash anim
            // become invis
            this.gameObject.SetActive(false);
        }
    }
}
