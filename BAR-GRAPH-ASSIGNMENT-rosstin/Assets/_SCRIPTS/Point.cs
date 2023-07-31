using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Point : XRSimpleInteractable, IMoveable
{
    public IMoveable.MoveableState CurrentState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void SetCurrentPosInstantly(Vector3 newPos)
    {
        throw new System.NotImplementedException();
    }

    public void SetDestinationLocalPos(Vector3 destination)
    {
        throw new System.NotImplementedException();
    }

}
