using UnityEngine;

public class TetrisRet : MonoBehaviour
{
    [Header("Outlets")] 
    public GameObject preview;

    [Header("Params")]
    public float DESCEND_SPEED_METERS_PER_SECOND;

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

    private void SetPreviewPos()
    {
        this.preview.transform.position = this.GetRestingPosForBlockFallFrom(this.transform.position);
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
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position));
                break;
            case TetrisRetState.Held:
                
                // add jitter
                float JITTER_AMOUNT = 0.05f;
                Vector3 jitter = new Vector3(UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT),
                    UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT), UnityEngine.Random.Range(-JITTER_AMOUNT, JITTER_AMOUNT));


                Vector3 snappedPos = gamestate.SnapToGrid(this.transform.position);
                
                SetBlockPosition(snappedPos + jitter);

                SetPreviewPos();
                
                
                break;
            case TetrisRetState.Fired:

                float moveTime = Time.deltaTime;
                float moveDistance = DESCEND_SPEED_METERS_PER_SECOND* moveTime;

                
                
                

                myBlock.transform.position = myBlock.transform.position + new Vector3(0f,-moveDistance,0f);

                
                
                float yRestingPos = GetLowestYForColumn(this.gamestate.blockContainer.GetCoordForPos(myBlock.transform.position).x);
                
                Debug.Log("Y RESTING PS " + yRestingPos);
                
                if (myBlock.transform.position.y <= yRestingPos)
                {
                    myBlock.transform.position = 
                        new Vector3(
                            myBlock.transform.position.x,
                            yRestingPos,
                            myBlock.transform.position.z
                            );
                    
                    SetState(TetrisRetState.Landed);
                }
                
                
                break;
        }

    }

    private float GetLowestYForColumn(int colX)
    {
        return this.gamestate.blockContainer.GetLowestYForColumn(colX);
    }
}
