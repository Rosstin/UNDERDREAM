using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    [Header("Renderer")]
    public MeshRenderer meshRenderer;
    
    private TetrisGS.ShardFlavors myFlavor;
    private Vector2Int myShardSize; // ranges from 1x1 to 3x3

    private float scoringOpa = 0.1f;
    private Color normCol;
    
    private TetrisGS myGamestate = null;

    private CompositeBlock myCompositeBlockParent;

    private ArbitrarySizeShardPositionData myPosition = new ArbitrarySizeShardPositionData();

    private const float ONE_THIRD = 1f / 3f;

    public Vector2Int GetTopRightCorner()
    {
        return (this.myPosition.topLeftCornerSubgridPos + new Vector2Int(this.myShardSize.x-1,0));
    }

    public Vector2Int GetBotLeftCorner()
    {
        return (this.myPosition.topLeftCornerSubgridPos + new Vector2Int(0,this.myShardSize.y-1));
    }

    public Vector2Int GetBotRightCorner()
    {
        return (this.myPosition.topLeftCornerSubgridPos + this.myShardSize - Vector2Int.one);
    }

    // given my bounds, list all positions valid for me
    public List<Vector2Int> GetAllSubgridLocations()
    {
        List<Vector2Int> subgridLocs = new List<Vector2Int>();

        
        for (int x = this.myPosition.topLeftCornerSubgridPos.x; x <= GetTopRightCorner().x; x++)
        {
            for (int y = this.myPosition.topLeftCornerSubgridPos.y; y <= GetBotLeftCorner().y; y++)
            {
                var pos =new Vector2Int(x, y);      

                subgridLocs.Add(pos);
            }
        }

        return subgridLocs;
    }
    
    public struct AbsoluteGridPositionData
    {
        public Vector2Int supergridLoc;
        public Vector2Int subgridLoc;
    }
    
    public struct ArbitrarySizeShardPositionData
    {
        public Vector2Int supergridLoc;
        public Vector2Int topLeftCornerSubgridPos;

        public override string ToString()
        {
            string loc= "{" + supergridLoc.x + "," + supergridLoc.y + "}";
            
            loc += " ("+topLeftCornerSubgridPos.x + "," + topLeftCornerSubgridPos.y + ")";
            
            return loc;
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
    public ArbitrarySizeShardPositionData GetSuperAndSubgridLocs()
    {
        return myPosition;
    }

    public void SetSuperAndSubgridLocs(ArbitrarySizeShardPositionData s)
    {
        this.myPosition = s;
    }
    
    public void Init(TetrisGS gamestate, GameObject contentParent, CompositeBlock cBlockParent, TetrisGS.ShardFlavors flav, Vector3 pos, Vector2Int shardSize, Vector2Int localSubgridLoc)
    {
        this.myShardSize = shardSize;
        this.myGamestate = gamestate;
        this.SetFlavor(flav);
        this.transform.SetParent(contentParent.transform);
        this.myCompositeBlockParent = cBlockParent;

        myPosition.supergridLoc= this.myCompositeBlockParent.GetSupergridLoc();

        myPosition.topLeftCornerSubgridPos = localSubgridLoc;

        this.normCol = this.meshRenderer.material.color;

        this.SetShardState(Shard.ShardState.Normal);

        ScaleForMySize(this.myShardSize);
        
        this.transform.position = pos;
    }

    private void ScaleForMySize(Vector2Int shardSize)
    {
        this.transform.localScale =
            new Vector3(ONE_THIRD * shardSize.x, ONE_THIRD * shardSize.y, ONE_THIRD);
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
    
    public void Expand1xNWidthShard(ArbitrarySizeShardPositionData shardPositionToExpandTo)
    {
        
        
        
        Vector2Int dir = GetRelativeDirection(this.myPosition.topLeftCornerSubgridPos, shardPositionToExpandTo.topLeftCornerSubgridPos);
        
        this.myShardSize += new Vector2Int(Mathf.Abs(dir.x),Mathf.Abs(dir.y));

        this.myCompositeBlockParent.DeleteShardAt(shardPositionToExpandTo.topLeftCornerSubgridPos);
        
        this.ScaleForMySize(this.myShardSize);

        // if you're expanding up or left, your topleft position changes. otherwise it doesnt
        if (dir == Vector2Int.up || dir == Vector2Int.right)
        {
            this.myPosition.topLeftCornerSubgridPos += dir;
        }

        var pos = this.myCompositeBlockParent.GetPositionForIndex(this.myPosition.topLeftCornerSubgridPos.x,
            this.myPosition.topLeftCornerSubgridPos.y, this.myShardSize);
        
        this.transform.position = pos;

        
        
        
    }

}
