using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxieControllerM71 : BaseController
{
    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] [Range(0f, 2f)] private float speedMetersPerSecond;
    [SerializeField] private Collider2D lemonCollider;
    [SerializeField] private Collider2D myCollider;

    // Update is called once per frame
    void Update()
    {
        if (myCollider.IsTouching(lemonCollider))
        {
            Debug.LogWarning("touching!");
            LoadNextScene();
        }


        if (CommandsHeldThisFrame.ContainsKey(Command.Up))
        {
            UpdateFloat(MoveDirection.Up);
        }
        else if (CommandsHeldThisFrame.ContainsKey(Command.Down))
        {
            UpdateFloat(MoveDirection.Down);

        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Left))
        {
            UpdateFloat(MoveDirection.Left);
        }
        else if (CommandsHeldThisFrame.ContainsKey(Command.Right))
        {
            UpdateFloat(MoveDirection.Right);
        }

    }

    private void UpdateFloat(MoveDirection dir)
    {
        int horizAmount = 0;
        if (dir == MoveDirection.Right)
        {
            horizAmount = 1;
        }
        else if (dir == MoveDirection.Left)
        {
            horizAmount = -1;
        }

        int vertAmount = 0;
        if (dir == MoveDirection.Up)
        {
            vertAmount = 1;
        }
        else if (dir == MoveDirection.Down)
        {
            vertAmount = -1;
        }

        //this.transform.localScale = new Vector3(1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

        //if (horizAmount != 0)
        //{
        //this.transform.localScale = new Vector3(1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        //bigBubble.transform.localScale = new Vector3(-horizAmount*bigBubble.transform.localScale.x, bigBubble.transform.localScale.y, bigBubble.transform.localScale.z);
        //}

        this.transform.localPosition
            = new Vector3(
            this.transform.localPosition.x + (horizAmount * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.y + (vertAmount * speedMetersPerSecond) * Time.deltaTime,
            this.transform.localPosition.z
            );

    }


}
