using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maoxun06 : BaseController
{
    [Header("Goal")]
    [SerializeField]
    private float lemonDistance;
    [SerializeField] private string nextScene;
    [SerializeField] private AudioSource snatchSFX;

    [Header("Movement")]
    [SerializeField] [Range(0.0001f, 0.0009f)] private float speedMetersPerSecond;
    [SerializeField] private float jumpForceUp;
    [SerializeField] [Range(0f, 0.0009f)] private float jumpForceForward;

    [Header("Outlets: Container")]
    [SerializeField] private GameObject Container;

    [Header("Outlets: Maoxun Animations")]
    [SerializeField] private GameObject IdleAnimation;
    [SerializeField] private GameObject WalkAnimation;

    [Header("Outlets: Bounds")]
    [SerializeField] private Transform Feet;
    [SerializeField] private Collider[] Ground;
    [SerializeField] private Collider Target;

    [Header("Outlets: Components")]
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private Collider myCollider;

    public enum MaoxunAnimState06
    {        
		Idle,
        Walk
    }
	private MaoxunAnimState06 currentState;

    public const float GROUND_ERROR = 0.05f;
    public const float MAX_RAY_DISTANCE = 50f;
    private float distanceToGround = 0f;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Start()
    {
		ActivateAnimation(MaoxunAnimState06.Idle);
    }

	public void ActivateAnimation(MaoxunAnimState06 anim)
    {
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        switch (anim)
        {
		case MaoxunAnimState06.Idle:
                IdleAnimation.SetActive(true);
                break;
		case MaoxunAnimState06.Walk:
                WalkAnimation.SetActive(true);
                break;
		}
    }

    public void Update()
    {
        BaseUpdate();

        float distanceToTarget = Vector3.Distance(this.transform.position, Target.transform.position);

        if (distanceToTarget < lemonDistance)
        {
            snatchSFX.Play();
            SceneManager.LoadScene(nextScene);
        }

        // pass thru platforms when jumping 
        if (myRigidbody.velocity.y > 0)
        {
            myCollider.enabled = false;
        }
        else
        {
            myCollider.enabled = true;
        }

        UpdateGravity();
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

            if (!didSomething)
            {
                UpdateIdle();
            }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void UpdateIdle()
    {
        if (currentState != MaoxunAnimState06.Idle)
        {
            ActivateAnimation(MaoxunAnimState06.Idle);
            currentState = MaoxunAnimState06.Idle;
        }
    }

    private void UpdateGravity()
    {
        distanceToGround = 999f;
        foreach(Collider g in Ground)
        {
            if (g.Raycast(new Ray(Feet.position, Vector3.down), out RaycastHit hit, MAX_RAY_DISTANCE))
            {
                if(hit.distance < distanceToGround)
                {
                    distanceToGround = hit.distance;
                }
            }
        }
    }

    private void UpdateJump()
    {
        if (distanceToGround < GROUND_ERROR)
        {
            ImpartJumpForce();
        }
    }

    private void ImpartJumpForce()
    {
        this.myRigidbody.AddForce(new Vector3(-jumpForceForward, jumpForceUp, 0f));
    }

    private void UpdateMoveLeftRight(MoveDirection direction)
    {
        if (currentState != MaoxunAnimState06.Walk)
        {
            ActivateAnimation(MaoxunAnimState06.Walk);
            currentState = MaoxunAnimState06.Walk;
        }

        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        this.transform.localScale = new Vector3(sign* Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (sign * speedMetersPerSecond) / Time.deltaTime,
            this.transform.localPosition.y,
            this.transform.localPosition.z
            );
    }

}
