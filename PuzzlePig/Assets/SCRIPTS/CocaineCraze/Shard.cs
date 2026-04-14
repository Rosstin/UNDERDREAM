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
    
    private Vector2 TopCornerPosition;

    private Vector2 Dimensions;

    private TetrisGS myGamestate = null;

    private CompositeBlock myCompositeBlockParent;

    private ArbitrarySizeShardPositionData myPosition = new ArbitrarySizeShardPositionData();
    
    private Vector2Int myLocalSubgridLocation; // if this is -1,-1 it means you're a large shard. otherwise, 1,1 is the center, 0,0 is the top right corn, etc

    private const float ONE_THIRD = 1f / 3f;

    public struct AbsoluteGridPositionData
    {
        public Vector2Int supergridLoc;
        public Vector2Int subgridLoc;
    }
    
    public struct ArbitrarySizeShardPositionData
    {
        public Vector2Int supergridLoc;
        public List<Vector2Int> subgridLocations;

        public override string ToString()
        {
            string loc= "{" + supergridLoc.x + "," + supergridLoc.y + "}";

            foreach (var p in subgridLocations)
            {
                loc += " {" + p.x + "," + p.y + "} ";
            }

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
        myPosition.subgridLocations = new List<Vector2Int>(){localSubgridLoc};

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
    
    public void Expand1xNWidthShard(ArbitrarySizeShardPositionData directionToExpandIn)
    {
        
        Vector2Int dir = GetRelativeDirection(directionToExpandIn.subgridLocations[0],this.myLocalSubgridLocation);
        
        

        if (dir == Vector2Int.up)
        {
            this.myShardSize += new Vector2Int(0, 1);


            this.myCompositeBlockParent.DeleteShardAt(directionToExpandIn.subgridLocations[0]);
            this.myCompositeBlockParent.PutShardAt(directionToExpandIn.subgridLocations[0], this);

            this.ScaleForMySize(this.myShardSize);
            this.PositionForMyDetails(this.myShardSize, this.myPosition.subgridLocations);


        }
        else if(dir == Vector2Int.down)
        {
            this.myShardSize += new Vector2Int(0, 1);

            this.ScaleForMySize(this.myShardSize);
        }
        else if(dir == Vector2Int.left)
        {
            this.myShardSize += new Vector2Int(1, 0);

            this.ScaleForMySize(this.myShardSize);
        }
        else if(dir == Vector2Int.right)
        {
            this.myShardSize += new Vector2Int(1, 0);

            this.ScaleForMySize(this.myShardSize);
        }
        
        
        
    }

    private void PositionForMyDetails(Vector2Int vector2Int, List<Vector2Int> myPositionSubgridLocations)
    {
        throw new System.NotImplementedException();
    }
}
