
/// <summary>
/// An interface for objects that can be moved by the player
/// </summary>
/// 

//HOW TO MOVE THE BAR
// 1. When you gripdown on a bar, it's selected
// 2. Now the position of your beam determines the bar's X
// 3. When you gripup, your beam stops determining the bar's X 
// 4. Put a collider at the back of the graph for capturing the correct X

public interface IMoveable
{
    // todo: events for picking up and dragging the object - look at the example package

    /// <summary>
    /// States for object movement
    /// 1. Unselected: the moveable is normal and fixed in position
    /// 2. Selected: the moveable has been selected and it can be moved left/right
    /// </summary>
    public enum MoveableState
    {
        Unselected, // the moveable is normal and sitting down
        Selected // the bar has been gripped
    }

    /// <summary>
    /// The current state of the moveable
    /// </summary>
    public MoveableState CurrentState { get; set; }

    /// <summary>
    /// Toggle between selected and unselected
    /// </summary>
    public void ToggleState();

}
