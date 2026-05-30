using UnityEngine;

public class TetrisRet : MonoBehaviour
{
    [Header("Outlets")] 
    public GameObject preview;
    public GameObject refSpot;

    [Header("Params")]
    public float FALL_PERIOD_S;

    private TetrisGS gamestate = null;
    private CompositeBlock myBlock=null;

    private Vector2Int lastRetCoord;

    private Vector2Int startFallCoord;
    private Vector2Int fallingDestCoord;

    private Vector3 fallWorldStartPos;
    private Vector3 fallWorldEndPos;
    
    public enum TetrisRetState
    {
        Unset,
        Ready,
        Held,
        Fired,
        Scoring,
    }

    private TetrisRetState myState = TetrisRetState.Ready;
    private float firedElapsed = 0f;

    public void Init(TetrisGS gs, CompositeBlock block)
    {
        this.gamestate = gs;
        this.myBlock = block;
    }

    public void SetNewBlock(CompositeBlock newBlock)
    {
        Destroy(this.myBlock.gameObject);
        this.myBlock = newBlock;
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
        gamestate.PlayGrabSfx();
        
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
            case TetrisRetState.Scoring:
                StartCoroutine(this.gamestate.colorGrid.ScoreBlock(myBlock, fallingDestCoord));
                break;
            case TetrisRetState.Ready:
                break;
            case TetrisRetState.Fired:

                this.myBlock.SetJitter(Vector3.zero);
                this.firedElapsed = 0f;
                
                gamestate.PlayFireSfx();

                this.startFallCoord = this.lastRetCoord;
                
                this.fallWorldStartPos = this.gamestate.colorGrid.SnapZToBack(this.gamestate.colorGrid.SupergridToWorld(startFallCoord));
                this.fallWorldEndPos = this.gamestate.colorGrid.SnapZToBack(this.gamestate.colorGrid.SupergridToWorld(fallingDestCoord));

                
                break;
        }
    }

    private void Update()
    {
        UpdateState();
    }

    public void UpdateRetPos(Vector2Int c)
    {

        this.lastRetCoord = c;
        
        // check if position is in range - ignore if not 
        bool inrange=this.gamestate.colorGrid.IsInRange(c);


        if (inrange)
        {
            this.transform.position = this.gamestate.colorGrid.SnapZToBack(this.gamestate.colorGrid.SupergridToWorld(c));
            this.lastRetCoord = c;
        }
        else
        {
            // not in range - ignore
        }
    }
    
    private void SetPreviewPos(Vector2Int coord)
    {
        this.fallingDestCoord = this.GetRestingCoordForBlockfall(coord);

        var wc = this.gamestate.colorGrid.SupergridToWorld(coord);

        this.preview.transform.position = this.gamestate.colorGrid.SnapZToBack(wc);

    }

    private Vector2Int GetRestingCoordForBlockfall(Vector2Int c)
    {
        return this.gamestate.blockContainer.GetRestingCoordForBlockFallFrom(c);
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


                Vector2Int snappedCoord = gamestate.colorGrid.WorldToSupergrid(this.transform.position);
                Vector3 snapped = gamestate.colorGrid.SupergridToWorld(snappedCoord);
                snapped = this.gamestate.colorGrid.SnapZToBack(snapped);
                
                SetBlockPosition(snapped, jitter);

                Vector2Int fallCoord =  this.GetRestingCoordForBlockfall(snappedCoord);
                
                SetPreviewPos(fallCoord);
                
                
                break;
            case TetrisRetState.Fired:
                SetBlockPosition(gamestate.SnapToGrid(this.transform.position), Vector3.zero);

                firedElapsed += Time.deltaTime;

                myBlock.transform.position =
                    Vector3.Lerp(fallWorldStartPos, fallWorldEndPos, firedElapsed / FALL_PERIOD_S);
                
                
                
                if (firedElapsed > FALL_PERIOD_S)
                {
                    gamestate.PlayTinkSfx();

                    myBlock.transform.position = fallWorldEndPos;
                    
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
        CompositeBlock cb = this.gamestate.colorGrid.GenerateBlock();
        cb.SetSuperGridLoc(new Vector2Int(0,0)); // set a location so consolidation works
        cb.Consolidate();
        cb.Consolidate();

        this.SetNewBlock(cb);
        this.SetState(TetrisRetState.Ready);
    }
}
