using UnityEngine;

public class TetrisRet : MonoBehaviour
{


    private TetrisGS gamestate = null;
    private CompositeBlock myBlock=null;

    public enum TetrisRetState
    {
        Unset,
        Ready,
        Held,
        Fired,
        Landed,
        Scoring,
    }

    private TetrisRetState myState = TetrisRetState.Ready;
    
    public void Init(TetrisGS gs, CompositeBlock block)
    {
        this.gamestate = gs;
        this.myBlock = block;
    }
    
    public void SetBlockPosition(Vector3 pos)
    {
        if (myBlock != null)
        {
            myBlock.transform.position = pos;
        }
    }

    public void GrabBlock()
    {
        this.SetState(TetrisRetState.Held);
    }
    
    public void FireBlock()
    {
        this.SetState(TetrisRetState.Fired);
    }
    
    public bool IsRetHeld()
    {
        return myState == TetrisRetState.Held;
    }


    private void SetState(TetrisRetState state)
    {

        this.myState = state;

        switch (myState)
        {
            case TetrisRetState.Ready:
                break;
            case TetrisRetState.Fired:
                break;
        }
    }

    private void Update()
    {
        UpdateState();
    }

    private void UpdateState(){
        switch (myState)
        {
            case TetrisRetState.Ready:
            case TetrisRetState.Held:
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position));
                break;
            case TetrisRetState.Fired:
                break;
        }

    }

}
