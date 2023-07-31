using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BarGraphAssignment
{
    /// <summary>
    /// A point which could lie anywhere on the graph in any X,Y coordinate.
    /// Exists to elucidate requirements for IMoveable interface, not currently implemented.
    /// For instance, the point can move up and down, not just left and right
    /// </summary>
    public class Point : XRSimpleInteractable, IMoveable
    {
        public IMoveable.MoveableState CurrentState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void OnSelect()
        {
            throw new System.NotImplementedException();
        }

        public void OnUnselect()
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentPosInstantly(Vector3 newPos)
        {
            throw new System.NotImplementedException();
        }

        public void SetDestinationLocalPos(Vector3 destination)
        {
            throw new System.NotImplementedException();
        }

    }
}