using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockContainer : MonoBehaviour
{
    public List<List<CompositeBlock>> blocksByLocation = null; // keep track of blocks by their supergrid/subgrid location
    public Dictionary<TetrisGS.ShardFlavors, List<Shard>> flavToListOfShards; // keep track of blocks of a given flavor

    public Dictionary<Shard.ArbitrarySizeShardPositionData, Shard> currentlyScoringShards;

    
    private Vector2Int dimens;
    
    private GameObject topLeftAnchor;
    private ColorGrid2 colorGrid;
    private GameObject backPosRef;
    private TetrisGS gamestate;
    public void Init( TetrisGS gs, ColorGrid2 cg2, Vector2Int dimens, GameObject topLeftAnchor, GameObject backPosRef)
    {
        this.colorGrid = cg2;
        this.gamestate = gs;
        this.backPosRef = backPosRef;
        this.dimens = dimens;
        
        this.blocksByLocation = new List<List<CompositeBlock>>();
        for (int x = 0; x < dimens.x; x++)
        {
            this.blocksByLocation.Add(new List<CompositeBlock>());
            for (int y = 0; y < dimens.y; y++)
            {
                this.blocksByLocation[x].Add(null);
            }
        }
        
        
        flavToListOfShards =  new Dictionary<TetrisGS.ShardFlavors, List<Shard>>();
        for (int i = 0; i < (int)TetrisGS.ShardFlavors.Unset; i++)
        {
            flavToListOfShards.Add((TetrisGS.ShardFlavors)i, new List<Shard>());
        }

        
        this.topLeftAnchor = topLeftAnchor;

    }

    public int GetWidth()
    {
        return this.dimens.x;
    }

    public int GetHeight()
    {
        return this.dimens.y;
    }

    
    public void GenerateLevelBlocks()
    {
        ClearBlocks();

        

        List<int> heights = new List<int> {  0,1,2,3,4,5 };
        var shuffledHeights = heights.OrderBy( x => Random.value ).ToList( );



        // for each column, generate a number of blocks between 0 and 4
        for (int x = 0; x < this.dimens.x; x++)
        {
            int blocksInC = shuffledHeights[x];
            for (int y = 0; y < blocksInC; y++)
            {

                int yCoord = this.GetHeight() - 1 - y;
                
                CompositeBlock newBlock = this.GenerateBlock();

                this.blocksByLocation[x][yCoord] = newBlock;
                this.flavToListOfShards = newBlock.PopulateFlavorList(flavToListOfShards);

                
                //Debug.Log("setting supergrid loc " + x +" , " + y);
                newBlock.SetSuperGridLoc(new Vector2Int(x,yCoord));

                // feed an appropriate coord for the block
                newBlock.transform.localPosition = this.colorGrid.SupergridToWorld(x, yCoord);

                

            }
        }

        Debug.Log("flav list " + flavToListOfShards);


    }


    /// <summary>
    /// Consolidate same-color shards that lie within a block. IE if 2 red shards are next to eachother, merge them into one 2x1 shard
    /// </summary>
    public void ConsolidateLandedBlocks()
    {
        for (int x = 0; x < blocksByLocation.Count; x++)
        {
            List<CompositeBlock> col = blocksByLocation[x];
            
            for (int y = 0; y < col.Count; y++)
            {
                CompositeBlock block = blocksByLocation[x][y];                
                
                if (block != null)
                {
                    block.Consolidate();
                    block.Consolidate();
                }
            }
        }
    }
    
    public void ClearBlocks()
    {
        for (int x = 0; x < blocksByLocation.Count; x++)
        {
            List<CompositeBlock> col = blocksByLocation[x];
            
            for (int y = 0; y < col.Count; y++)
            {
                CompositeBlock block = blocksByLocation[x][y];                
                
                if (block != null)
                {
                    block.Clear();
                    GameObject.Destroy(block.gameObject);
                    block = null;
                }
            }
        }
    }

    public CompositeBlock GenerateBlock()
    {
        CompositeBlock cb = GameObject.Instantiate(this.gamestate.compPrefab).GetComponent<CompositeBlock>();
        cb.Init(this.gamestate, this.gamestate.blockContainer, new Vector2Int(-1,-1));

        cb.Randomize();

        cb.Consolidate();
        cb.Consolidate();

        return cb;
    }

    public int GetLowestYForColumn(int colX)
    {
        // given a column, get lowest y

        if (colX >= 0 && colX < blocksByLocation.Count)
        {
            var column = blocksByLocation[colX];
        
            // look for the first empty

            int yCand = column.Count;
            for (int y = 0; y < column.Count; y++)
            {
                CompositeBlock block = column[y];

                if (block != null)
                {
                    yCand = y;
                    break;
                }
            
            }

            return yCand-1;
        }
        else
        {
            return -1;
        }


    }

    public Shard GetShardAtAbsoluteLocation(Shard.AbsoluteGridPositionData loc)
    {
        var block = GetCompBlockForCoord(loc.supergridLoc);

        if (block != null)
        {
            return block.GetShardForIndex(loc.subgridLoc);
        }
        else
        {
            return null;
        }
        
    }

    
    
    public Vector2Int GetRestingCoordForBlockFallFrom(Vector2Int c)
    {
        int lowestY=GetLowestYForColumn(c.x);

        Vector2Int restingCoord = new Vector2Int(c.x, lowestY);

        return restingCoord;
    }

    
    
    public CompositeBlock GetCompBlockForCoord(Vector2Int coord)
    {
        if (
            coord.x >= 0 
            && coord.x < blocksByLocation.Count 
            && coord.y >= 0 
            && coord.y < blocksByLocation[coord.x].Count)
        {
            var block = blocksByLocation[coord.x][coord.y];
            return block;
        }
        else
        {
            return null;
        }
    }

    public void RestoreScoringShardsToNormal()
    {
        foreach (Shard s in currentlyScoringShards.Values)
        {
            s.SetShardState(Shard.ShardState.Normal);
        }
    }


    public void ClearCurrentlyScoringShards()
    {
        this.currentlyScoringShards = new Dictionary<Shard.ArbitrarySizeShardPositionData, Shard>();
    }

    public void ExpandUnscoredBlocks()
    {
        // rtodo destroy scored blocks
        // rtodo if all shards in a block are destroyed, destroy the block

        // expand unscored shards
        for (int x = 0; x < blocksByLocation.Count; x++)
        {
            List<CompositeBlock> col = blocksByLocation[x];
            
            for (int y = 0; y < col.Count; y++)
            {
                CompositeBlock block = blocksByLocation[x][y];                
                
                if (block != null)
                {
                    block.ExpandUnscored();
                    block.ExpandUnscored();
                    //block.ExpandUnscored();
                    block.Consolidate();
                }
            }
        }
        
        
        
        
    }

    public void ClearFullyScoredBlocks()
    {
        for (int x = 0; x < blocksByLocation.Count; x++)
        {
            List<CompositeBlock> col = blocksByLocation[x];
            
            for (int y = 0; y < col.Count; y++)
            {
                CompositeBlock block = blocksByLocation[x][y];                
                
                
                if ( block != null && block.IsFullyScored())
                {
                    DestroyBlock(block);
                }
            }
        }
    }

    public void DestroyBlock(CompositeBlock compositeBlock)
    {
        //rtodo remove from flav list too



        blocksByLocation[compositeBlock.GetSupergridLoc().x][compositeBlock.GetSupergridLoc().y] = null;
        
        GameObject.Destroy(compositeBlock.gameObject);
    }
}
