using System.Collections;
using System.Collections.Generic;
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

    public void FireBlock()
    {
        this.SetState(TetrisRetState.Fired);
    }

    private void SetState(TetrisRetState state)
    {
        switch (myState)
        {
            case TetrisRetState.Ready:
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position));
                break;
            case TetrisRetState.Fired:
                break;
        }
    }

    private void Update()
    {
        switch (myState)
        {
            case TetrisRetState.Ready:
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position));
                break;
            case TetrisRetState.Fired:
                break;
        }
    }

    private void UpdateState()
    {
        
    }
    
}
