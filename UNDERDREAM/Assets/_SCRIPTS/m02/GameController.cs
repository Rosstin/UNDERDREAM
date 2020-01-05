using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float SpeedMetersPerSecond;
    public GameObject Container;

    public enum MaoxunAnimStateM03
    {
        Idle,
        Walk,
        LookDown
    }

    public GameObject IdleAnimation;
    public GameObject WalkAnimation;

    public Transform LeftScreenBound;
    public Transform RightScreenBound;
    public Transform LowerScreenBound;

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private MaoxunAnimStateM03 currentState;

    private void Start()
    {

    }

    public void ActivateAnimation(MaoxunAnimStateM03 anim)
    {
        IdleAnimation.SetActive(false);
        WalkAnimation.SetActive(false);
        switch (anim)
        {
            case MaoxunAnimStateM03.Idle:
                IdleAnimation.SetActive(true);
                break;
            case MaoxunAnimStateM03.Walk:
                WalkAnimation.SetActive(true);
                break;
            case MaoxunAnimStateM03.LookDown:
                break;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) //ptodo change tojump
        {
            UpdateMove(MoveDirection.Up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            UpdateMove(MoveDirection.Down);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            UpdateMove(MoveDirection.Left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            UpdateMove(MoveDirection.Right);
        }
        else
        {
            UpdateIdle();
        }

    }

    private void UpdateIdle()
    {
        if(currentState != MaoxunAnimStateM03.Idle)
        {
            ActivateAnimation(MaoxunAnimStateM03.Idle);
            currentState = MaoxunAnimStateM03.Idle;
        }
    }

    private void UpdateMove(MoveDirection direction)
    {
        /*
        if (Container.transform.localPosition.y < LowerScreenBound.localPosition.y)
        {
            Container.transform.localPosition =
                new Vector3(
                RightScreenBound.localPosition.x,
                Container.transform.localPosition.y,
                Container.transform.localPosition.z
                    );
        }*/


        switch (direction)
        {
            case MoveDirection.Up:
                UpdateJump();
                break;
            case MoveDirection.Down:
                break;
            case MoveDirection.Left:
                UpdateMoveLeftRight(direction);
                break;
            case MoveDirection.Right:
                UpdateMoveLeftRight(direction);
                break;
        }
    }

    private void UpdateJump() {

    }

    private void UpdateMoveLeftRight(MoveDirection direction)
    {
        if(currentState != MaoxunAnimStateM03.Walk)
        {
            ActivateAnimation(MaoxunAnimStateM03.Walk);
            currentState = MaoxunAnimStateM03.Walk;
        }

        int sign = -1;
        if (direction == MoveDirection.Right)
        {
            sign = 1;
        }

        Container.transform.localScale = new Vector3(sign, 1, 1);

        Container.transform.localPosition
            = new Vector3(
            Container.transform.localPosition.x + (sign * SpeedMetersPerSecond) / Time.deltaTime,
            Container.transform.localPosition.y,
            Container.transform.localPosition.z
            );

        if(Container.transform.localPosition.x > RightScreenBound.localPosition.x)
        {
            Container.transform.localPosition =
                new Vector3(
                RightScreenBound.localPosition.x,
                Container.transform.localPosition.y,
                Container.transform.localPosition.z
                    );
        }

        if (Container.transform.localPosition.x < LeftScreenBound.localPosition.x)
        {
            Container.transform.localPosition =
                new Vector3(
                LeftScreenBound.localPosition.x,
                Container.transform.localPosition.y,
                Container.transform.localPosition.z
                    );
        }

    }

}
