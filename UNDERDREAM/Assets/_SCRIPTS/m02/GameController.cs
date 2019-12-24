using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float SpeedMetersPerSecond;
    public GameObject Container;

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

    private void Start()
    {

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

        if (Input.GetKey(KeyCode.DownArrow))
        {
            UpdateMove(MoveDirection.Down);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            UpdateMove(MoveDirection.Left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            UpdateMove(MoveDirection.Right);
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
