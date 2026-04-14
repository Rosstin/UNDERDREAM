using UnityEngine;

public class Shard : MonoBehaviour
{
    [Header("Renderer")]
    public MeshRenderer meshRenderer;
    
    private TetrisGS.ShardFlavors myFlavor;
    private Vector2Int myShardSize; // ranges from 1x1 to 3x3

    private float scoringOpa = 0.1f;
    private Color normCol;
    
    private Vector2 TopCornerPosition;

    private Vector2 Dimensions;

    private TetrisGS myGamestate = null;

    private CompositeBlock myCompositeBlockParent;

    private ShardPositionData myPosition = new ShardPositionData();
    
    private Vector2Int myLocalSubgridLocation; // if this is -1,-1 it means you're a large shard. otherwise, 1,1 is the center, 0,0 is the top right corn, etc


    public struct ShardPositionData
    {
        public Vector2Int supergridLoc;
        public Vector2Int subgridLoc;

        public override string ToString()
        {
            return "{"+supergridLoc.x+","+supergridLoc.y+"}{"+subgridLoc.x+","+subgridLoc.y+"}";
        }
    }


    private ShardState myShardState;
    public enum ShardState
    {
        Unset,
        Normal,
        Scoring
    }

    public Vector2Int GetMySize()
    {
        return this.myShardSize;
    } 
    
    public void SetShardState(ShardState s)
    {
        switch (s)
        {
            case ShardState.Scoring:
                this.meshRenderer.material.color = new Color(normCol.r,normCol.g,normCol.b,scoringOpa);
                //this.SetScoreMat();
                break;
            case ShardState.Normal:
                this.SetFlavor(myFlavor);
                this.meshRenderer.material.color = normCol;
                break;
            case ShardState.Unset:
                Debug.LogError("shard state unset");
                break;
            
        }
    }



    public void SetSuperGridLoc(Vector2Int sgridl)
    {
        this.myPosition.supergridLoc = sgridl;
    }
    public ShardPositionData GetSuperAndSubgridLocs()
    {
        return myPosition;
    }
    
    public void Init(TetrisGS gamestate, GameObject contentParent, CompositeBlock cBlockParent, TetrisGS.ShardFlavors flav, Vector3 pos, Vector2Int shardSize, Vector2Int localSubgridLoc)
    {
        this.myShardSize = shardSize;
        this.myGamestate = gamestate;
        this.SetFlavor(flav);
        this.transform.SetParent(contentParent.transform);
        this.myCompositeBlockParent = cBlockParent;

        myPosition.supergridLoc= this.myCompositeBlockParent.GetSupergridLoc();
        myPosition.subgridLoc = localSubgridLoc;

        this.normCol = this.meshRenderer.material.color;

        this.SetShardState(Shard.ShardState.Normal);

        
        if (this.myShardSize == new Vector2Int(3, 3))
        {
            this.transform.localScale = new Vector3((1/1f), (1/1f), (1/1f));
        }else if (this.myShardSize == new Vector2Int(1, 1))
        {
            this.transform.localScale = new Vector3((1/3f), (1/3f), (1/3f));
        }
        else
        {
            Debug.Log("need to support other shard sizes! " + shardSize);
        }
        
        this.transform.position = pos;
    }

    public void SetVisible(bool vis)
    {
        this.meshRenderer.gameObject.SetActive(vis);
    }
    
    private void SetFlavor(TetrisGS.ShardFlavors flavor)
    {
        this.myFlavor = flavor;
        meshRenderer.material = this.myGamestate.GetMatForFlavor(flavor);

    }
    private void SetScoreMat()
    {
        meshRenderer.material = this.myGamestate.scoreMat;
    }


    public TetrisGS.ShardFlavors GetFlavor()
    {
        return myFlavor;
    }

    public Vector2Int GetRelativeDirection(Vector2Int a, Vector2Int b)
    {
        Vector2Int dir = a-b;

        return dir;


    }
    
    public void Expand1xNWidthShard(ShardPositionData directionToExpandIn)
    {
        Vector2Int dir = GetRelativeDirection(directionToExpandIn.subgridLoc,this.myLocalSubgridLocation);

        if (dir == CocaineCrazeConstants.UP)
        {
            this.myShardSize += new Vector2Int(0, 1);


            this.myCompositeBlockParent.DeleteShardAt(directionToExpandIn.subgridLoc);
            this.myCompositeBlockParent.PutShardAt(directionToExpandIn.subgridLoc, this);
            
            this.transform.localScale = new Vector3(this.transform.localScale.x,this.transform.localScale.y*2,this.transform.localScale.z);
        }
        else if(dir == CocaineCrazeConstants.DOWN)
        {
            this.myShardSize += new Vector2Int(0, 1);

            this.transform.localScale = new Vector3(this.transform.localScale.x,this.transform.localScale.y*2,this.transform.localScale.z);
        }
        else if(dir == CocaineCrazeConstants.LEFT)
        {
            this.myShardSize += new Vector2Int(1, 0);

            this.transform.localScale = new Vector3(this.transform.localScale.x*2,this.transform.localScale.y,this.transform.localScale.z);

        }
        else if(dir == CocaineCrazeConstants.RIGHT)
        {
            this.myShardSize += new Vector2Int(1, 0);

            this.transform.localScale = new Vector3(this.transform.localScale.x*2,this.transform.localScale.y,this.transform.localScale.z);
        }

        
        
        
        
        
        
    }
}
