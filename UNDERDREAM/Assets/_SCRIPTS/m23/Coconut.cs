using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D col;

    private void Start()
    {
        rb.gravityScale = 0f;
    }

    public void Drop()
    {
        rb.gravityScale = 1f;
        this.transform.parent = null;
    }

}
