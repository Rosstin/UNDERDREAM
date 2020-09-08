using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoatController : BaseController
{

    [Header("Jump")]
    [SerializeField] [Range(0.0001f, 0.0009f)] private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpSpeedMargin;
    [SerializeField] private AudioSource boingSfx;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject container;

    [Header("Outlets: Bounds")]
    [SerializeField]  private Collider target;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    public BoxCollider2D MyCollider;

    private float timeSinceLastJump = 0f;

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
    public void TakeDamage()
    {
        Debug.LogWarning("player take damage");
    }

    private void Update()
    {
        BaseUpdate();

        timeSinceLastJump += Time.deltaTime;

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
