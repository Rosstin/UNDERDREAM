using UnityEngine;

public class TetrisRet : MonoBehaviour
{
    [Header("Outlets")] 
    public GameObject preview;

    [Header("Params")]
    public float DESCEND_SPEED_METERS_PER_SECOND;

    private TetrisGS gamestate = null;
    private CompositeBlock myBlock=null;

    private float latestDestY = -1f;
    
    public enum TetrisRetState
    {
        Unset,
        Ready,
        Held,
        Fired,
        Scoring,
    }

    private TetrisRetState myState = TetrisRetState.Ready;
    
    public void Init(TetrisGS gs, CompositeBlock block)
    {
        this.gamestate = gs;
        this.myBlock = block;
        
        
    }
    
    public void SetBlockPosition(Vector3 pos, Vector3 jitter)
    {
        if (myBlock != null)
        {
            myBlock.SetPos(pos, jitter);
        }
    }

    public void GrabBlock()
    {
        gamestate.PlayTinkSfx();
        
        this.SetState(TetrisRetState.Held);
    }
    
    public void FireBlock()
    {
        gamestate.PlayFireSfx();
        
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
            case TetrisRetState.Scoring:
                // determine which blocks to destroy
                // need a destroy effect

                // look for adjacencies
                
                // look at edges and corners basically

                StartCoroutine(this.gamestate.colorGrid.ScoreBlock(myBlock));
                
                
                
                
                break;
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

    private void SetPreviewPos()
    {
        this.preview.transform.position = this.GetRestingPosForBlockFallFrom(this.transform.position);
        this.latestDestY = this.preview.transform.position.y;
    }

    private Vector3 GetRestingPosForBlockFallFrom(Vector3 pos)
    {
        Vector3 snappedPos = gamestate.SnapToGrid(pos);

        return this.gamestate.blockContainer.GetRestingPosForBlockFallFrom(snappedPos);

    }

    private void UpdateState(){
        switch (myState)
        {
            case TetrisRetState.Ready:
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position), Vector3.zero);
                break;
            case TetrisRetState.Held:
                
                // add jitter
                float JITTER_AMOUNT = 0.05f;
                Vector3 jitter = new Vector3(UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT),
                    UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT), UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT));


                Vector3 snappedPos = gamestate.SnapToGrid(this.transform.position);
                
                SetBlockPosition(snappedPos, jitter);

                SetPreviewPos();
                
                
                break;
            case TetrisRetState.Fired:

                float moveTime = Time.deltaTime;
                float moveDistance = DESCEND_SPEED_METERS_PER_SECOND* moveTime;

                myBlock.SetPos((myBlock.transform.position + new Vector3(0f,-moveDistance,0f)), Vector3.zero);

                if (myBlock.transform.position.y <= latestDestY)
                {
                    gamestate.PlayTinkSfx();

                    myBlock.transform.position = 
                        new Vector3(
                            myBlock.transform.position.x,
                            latestDestY,
                            myBlock.transform.position.z
                            );
                    
                    SetState(TetrisRetState.Scoring);
                }
                
                
                break;
        }

    }
    
    public bool CanGrab()
    {
        if (this.myState == TetrisRetState.Ready)
        {
            return true;
        }
        if (this.myState == TetrisRetState.Fired || this.myState == TetrisRetState.Scoring)
        {
            return false;
        }

        return true;
    }

    public void FinishedScoring()
    {
        this.SetState(TetrisRetState.Ready);
    }
}
