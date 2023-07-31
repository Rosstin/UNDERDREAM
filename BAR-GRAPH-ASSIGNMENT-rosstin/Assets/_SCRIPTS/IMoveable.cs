
using UnityEngine;
/// <summary>
/// An interface for objects that can be moved by the player
/// </summary>
public interface IMoveable
{
    /// <summary>
    /// States for object movement
    /// 1. Unselected: the moveable is normal and fixed in position
    /// 2. Selected: the moveable has been selected and it can be moved left/right
    /// </summary>
    public enum MoveableState
    {
        Unselected, // the moveable is normal and sitting down
        Selected // the moveable has been selected and will travel with the user's ray
    }

    /// <summary>
    /// The current state of the moveable. It can be selected or unselected
    /// </summary>
    public MoveableState CurrentState { get; set; }

    /// <summary>
    /// Set the destination for this Moveable. It will travel there on its own time while unselected
    /// This method should be a no-op if the object is in motion
    /// </summary>
    /// <param name="destination"></param>
    public void SetDestinationLocalPos(Vector3 destination);

}
