using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompositeBlock : MonoBehaviour
{
    [Header("Prefabs")]
    public Shard shardPrefab;

    [Header("Outlets")] 
    public GameObject contentParent;
    
    [Header("My Bounds")] 
    public GameObject topLeftRef;
    public GameObject botRightRef;
    
    private Vector2Int mySupergridLocation; // -1,-1 means you're not landed on the grid [being held, moving, etc]
    
    private List<List<Shard>> shards3x3 = null;
    
    //private Vector2Int sizeOfMyShards;
    
    private TetrisGS gamestate = null;
    private List<CompositeBlock> blocks = new List<CompositeBlock>();

    System.Random random = new System.Random();

    public void SetJitter(Vector3 jitter)
    {
        this.contentParent.transform.localPosition = jitter;
    }
    
    public void Init(TetrisGS gs, BlockContainer blockCont, Vector2Int superGridLoc)
    {
        this.mySupergridLocation = superGridLoc;
        this.gamestate = gs;
        this.transform.SetParent(blockCont.transform);
        this.transform.localPosition = Vector3.zero;
        
        Populate3x3WithEmpties();

    }

    /*
    public Vector2Int GetShardSize()
    {
        return sizeOfMyShards;
    }
    */
    
    public void Clear()
    {
        for (int x = 0; x < shards3x3.Count; x++)
        {
            List<Shard> col = shards3x3[x];

            for (int y = 0; y < col.Count; y++)
            {
                if (col[y] != null)
                {
                    GameObject.Destroy(col[y].gameObject);
                    col[y] = null;
                }
            }
        }
    }

    public static float GetBlockWidth()
    {
        return 1f;
    }

    public static float GetBlockHeight()
    {
        return 1f;
    }

    public Vector3 GetPositionForIndex(int x, int y, Vector2Int size)
    {
        
        // first the overall position of the topleft transform
        Vector3 pos = topLeftRef.transform.position + new Vector3(GetBlockWidth() * (size.x/6f), -GetBlockHeight() * (size.y/6f), 0);

        // modify that by your local position inside
        pos += new Vector3(x*(GetBlockWidth()/3f), -y*(GetBlockHeight()/3f), 0);
        
        return pos;
    }
    
    public void Randomize()
    {
        Clear();

        GenShardedBlock();
    }


    private void Populate3x3WithEmpties()
    {
        this.shards3x3 = new List<List<Shard>>();
        for (int x = 0; x < 3; x++)
        {
            this.shards3x3.Add(new List<Shard>());
            for (int y = 0; y < 3; y++)
            {
                this.shards3x3[x].Add(null);
            }
        }
    }
    public void GenFullSizeBlock()
    {
        var shardSize = new Vector2Int(3,3);
        TetrisGS.ShardFlavors flavForThisBlock = GenerateRandomFlavor();

        Shard s = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
        s.Init(gamestate, contentParent, this, flavForThisBlock, GetPositionForIndex(1,1,shardSize), shardSize, new Vector2Int(-1,-1));
        
        List<Shard> shardsOfFlav = this.gamestate.blockContainer.flavToListOfShards[flavForThisBlock];
        shardsOfFlav.Add(s);
        this.gamestate.blockContainer.flavToListOfShards[flavForThisBlock] = shardsOfFlav;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                this.shards3x3[x][y] = s;
            }
        }
    }

    public void GenShardedBlock()
    {        
        var shardSize = new Vector2Int(1,1);

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                TetrisGS.ShardFlavors randomFlavor = GenerateRandomFlavor();
                Shard shard = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
                shard.Init(gamestate, contentParent,this, randomFlavor, GetPositionForIndex(x,y,shardSize), shardSize, new Vector2Int(x,y));
                
                shards3x3[x][y] = shard;
            }
        }
    }
    
    /// <summary>
    /// Generate one of 9 random flavors. Todo: more detailed params
    /// </summary>
    /// <returns></returns>
    private TetrisGS.ShardFlavors GenerateRandomFlavor()
    {
        var flav = this.gamestate.GetWeightedRandomFlavor();

        return flav;
    }

    public void SetPos(Vector3 pos, Vector3 jitter)
    {
        SetJitter(jitter);
        this.transform.position = pos;
    }

    public Shard GetShardForIndex(Vector2Int xy)
    {
        return GetShardForIndex(xy.x, xy.y);
    }
    
    public Shard GetShardForIndex(int x=-1, int y=-1)
    {
        if (x >= 3 || x <= -1 || y >= 3 || y <= -1)
        {
            Debug.Log("trying to get shard for INVALID index " + x + ", " + y);
            return null;
        }
        else
        {
            return shards3x3[x][y];
        }
    }

    public void SetSuperGridLoc(Vector2Int sGridLoc)
    {
        // rtodo merge assignment here with setting things?

        foreach (var l in shards3x3)
        {
            if (l != null)
            {
                foreach (var s in l)
                {
                    if (s != null)
                    {
                        s.SetSuperGridLoc(sGridLoc);
                    }
                }
            }
        }
        
        this.mySupergridLocation=sGridLoc;
    }

    public Dictionary<TetrisGS.ShardFlavors, List<Shard>> PopulateFlavorList(Dictionary<TetrisGS.ShardFlavors, List<Shard>> flavToListOfShards)
    {
        foreach (var lis in shards3x3)
        {
            foreach (var sh in lis)
            {
                flavToListOfShards = this.AddShardToList(sh,flavToListOfShards);
            }
        }

            
        return flavToListOfShards;
    }

    public Dictionary<TetrisGS.ShardFlavors, List<Shard>> AddShardToList(Shard s, Dictionary<TetrisGS.ShardFlavors, List<Shard>> flavToListOfShards)
    {
        var flav = s.GetFlavor();

        var list = flavToListOfShards[flav];
        
        list.Add(s);

        flavToListOfShards[flav] = list;

        return flavToListOfShards;
    }

    public Vector2Int GetSupergridLoc()
    {
        return mySupergridLocation;
    }

    public override string ToString()
    {
        return "block at " + this.mySupergridLocation;
    }

    public void ExpandUnscored()
    {
        Debug.Log("expand unscored " + this);
     
        for (int x = 0; x < shards3x3.Count; x++)
        {
            List<Shard> col = shards3x3[x];

            for (int y = 0; y < col.Count; y++)
            {

                if (col[y] != null)
                {
                    // grab the shard and check neighbors within myself
                    Shard shardToExpand = col[y];
                    
                    if(!shardToExpand.isScoring())
                    {
                        ExpandShard(shardToExpand);

                        /*
                        foreach(var dir in CocaineCrazeConstants.UP_DOWN_LEFT_RIGHT)
                        {
                            
                            
                            Shard adjShard =
                                this.gamestate.colorGrid.GetShardInDirectionLocalOnly(this, new Vector2Int(x, y), dir);
                            
                            if (adjShard != null && adjShard != shardToExpand && adjShard.isScoring())
                            {
                                Debug.Log("found adjacent scoring block " + adjShard);
                                
                                //MergeTwoShards(shardToExpand,adjShard);


                            }
                        }
                        */

                    }
                    
                }
            }
        }
    }

    private void ExpandShard(Shard shardToExpand)
    {
        List<Shard> scoringShardsToDestroy = new List<Shard>();
        // choose a direction - if all the new spaces I want to expand into are unfilled, expand
        foreach (var dir in CocaineCrazeConstants.UP_DOWN_LEFT_RIGHT)
        {
            if (dir == Vector2Int.up || dir == Vector2Int.down)
            {
                int xWidth = shardToExpand.GetMySize().x;
                
                // check in your direction
                bool blocked = false;
                
                for (int x = shardToExpand.GetTopLeftCorner().x;
                     x < shardToExpand.GetTopLeftCorner().x + xWidth;
                     x++)
                {
                    Vector2Int relevantCorner = Vector2Int.zero;
                    if (dir == Vector2Int.down)
                    {
                        relevantCorner = shardToExpand.GetBotLeftCorner();
                    }else if (dir == Vector2Int.up)
                    {
                        relevantCorner = shardToExpand.GetTopLeftCorner();
                    }
                    Vector2Int expansionCoord = new Vector2Int(x, relevantCorner.y - dir.y);

                    Debug.Log("expand updown from " + shardToExpand.GetTopLeftCorner() + " to expansion coord " + expansionCoord + " by moving in direction " + dir);
                    
                    if (IsSubgridOob(expansionCoord))
                    {
                        Debug.Log("blocked UD by oob");
                        blocked = true;
                    }
                    else
                    {
                        var shardAt = shards3x3[expansionCoord.x][expansionCoord.y];
                        if (shardAt != null)
                        {
                            if (shardAt.isScoring())
                            {
                                scoringShardsToDestroy.Add(shardAt);
                            }
                            else
                            {
                                Debug.Log("blocked UD by another shard");
                                blocked = true;
                            }
                        }
                    }
                }

                if (blocked == false)
                {
                    // expand in that dir
                    
                    Debug.Log("expanding shard UD " + shardToExpand + " of size " + shardToExpand.GetMySize() + " in direction " + dir );
                    
                    ExpandShardInDirFill(shardToExpand, dir);
                }
            }
            else if (dir == Vector2Int.left || dir == Vector2Int.right)
            {
                int yHeight = shardToExpand.GetMySize().y;

                
                
                // check in your direction
                bool blocked = false;

                for (int y = shardToExpand.GetTopLeftCorner().y;
                     y < shardToExpand.GetTopLeftCorner().y + yHeight;
                     y++)
                {
                    Vector2Int relevantCorner = Vector2Int.zero;
                    if (dir == Vector2Int.left)
                    {
                        relevantCorner = shardToExpand.GetTopLeftCorner();
                    }else if (dir == Vector2Int.right)
                    {
                        relevantCorner = shardToExpand.GetTopRightCorner();
                    }

                    Vector2Int expansionCoord = new Vector2Int(  relevantCorner.x + dir.x, y);

                    Debug.Log("expand shard LR " + shardToExpand +" rl from " + shardToExpand.GetTopLeftCorner() + " to expansion coord " + expansionCoord + " by moving in direction " + dir);
                    
                    if (IsSubgridOob(expansionCoord))
                    {
                        Debug.Log("blocked LR by oob");
                        blocked = true;
                    }
                    else
                    {
                        var shardAt = shards3x3[expansionCoord.x][expansionCoord.y];
                        if (shardAt != null)
                        {
                            if (shardAt.isScoring())
                            {
                                scoringShardsToDestroy.Add(shardAt);
                            }
                            else
                            {
                                Debug.Log("blocked LR by another shard");
                                blocked = true;
                            }
                        }
                    }

                }
                
                if (blocked == false)
                {
                    // expand in that dir
                    Debug.Log("expanding shard lr of size " + shardToExpand.GetMySize() + " in direction " + dir );

                    foreach (var s in scoringShardsToDestroy)
                    {
                        DestroyShard(s);
                    }
                    
                    ExpandShardInDirFill(shardToExpand, dir);
                }

            }

        }

    }

    private void ExpandShardInDirFill(Shard shardToExpand, Vector2Int dir)
    {
        shardToExpand.ExpandShardInDirFill(dir);
    }

    private void MergeTwoShards(Shard shardToConsolidate, Shard adjShard)
    {
                            
        // if we're merging 2 1x1s, it's fine
        if (shardToConsolidate.GetMySize() == new Vector2Int(1, 1) &&
            adjShard.GetMySize() == new Vector2Int(1, 1))
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        // if we're merging a 2,1 and a 1,1, they need to be at the same Y
        else if (shardToConsolidate.GetMySize() == new Vector2Int(2, 1) && adjShard.GetMySize() == new Vector2Int(1, 1) && 
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }else if (shardToConsolidate.GetMySize() == new Vector2Int(1, 2) && adjShard.GetMySize() == new Vector2Int(1, 1) && 
                  shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(1, 1) && adjShard.GetMySize() == new Vector2Int(1, 2) && 
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }else if (shardToConsolidate.GetMySize() == new Vector2Int(1, 1) && adjShard.GetMySize() == new Vector2Int(1, 2) && 
                  shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }

        else if (shardToConsolidate.GetMySize() == new Vector2Int(1, 2) &&
                 adjShard.GetMySize() == new Vector2Int(1, 2) &&
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(2, 1) &&
                 adjShard.GetMySize() == new Vector2Int(2, 1) &&
                 shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(2, 2) &&
                 adjShard.GetMySize() == new Vector2Int(1, 2) &&
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(2, 2) &&
                 adjShard.GetMySize() == new Vector2Int(2, 1) &&
                 shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(1, 3) &&
                 adjShard.GetMySize() == new Vector2Int(1, 3) &&
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(3, 1) &&
                 adjShard.GetMySize() == new Vector2Int(3, 1) &&
                 shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(2, 3) &&
                 adjShard.GetMySize() == new Vector2Int(1, 3) &&
                 shardToConsolidate.GetTopLeftCorner().y == adjShard.GetTopLeftCorner().y
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }
        else if (shardToConsolidate.GetMySize() == new Vector2Int(3, 2) &&
                 adjShard.GetMySize() == new Vector2Int(3, 1) &&
                 shardToConsolidate.GetTopLeftCorner().x == adjShard.GetTopLeftCorner().x
                )
        {
            shardToConsolidate.ExpandShard(adjShard.GetSuperAndSubgridLocs());
            SetSubgridLocsForShard(shardToConsolidate);
        }

    }
    
    /// <summary>
    /// For a given block, take all the components and merge them
    /// rtodo - needs to be agnostic of absolute pos / supergrid pos
    /// </summary>
    public void Consolidate()
    { 
        for (int x = 0; x < shards3x3.Count; x++)
        {
            List<Shard> col = shards3x3[x];

            for (int y = 0; y < col.Count; y++)
            {

                if (col[y] != null)
                {
                    // grab the shard and check neighbors within myself
                    Shard shardToConsolidate = col[y];
                    
                    foreach(var dir in CocaineCrazeConstants.UP_DOWN_LEFT_RIGHT)
                    {
                        
                        
                        Shard adjShard =
                            this.gamestate.colorGrid.GetShardInDirectionLocalOnly(this, new Vector2Int(x, y), dir);

                        
                        if (adjShard != null && adjShard != shardToConsolidate)
                        {

                            //  flavors have to be the same and dimens need to line up
                            if (adjShard.GetFlavor() == shardToConsolidate.GetFlavor())
                            {
                                MergeTwoShards(shardToConsolidate,adjShard);
                            }
                        }
                    }
                }
            }
        }
    }

    public bool IsSubgridOob(int xOrY)
    {
        if (xOrY <= -1 || xOrY >= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsSubgridOob(Vector2Int xy)
    {
        if (IsSubgridOob(xy.x))
        {
            return true;
        }

        if (IsSubgridOob(xy.y))
        {
            return true;
        }

        return false;
    }

    private void SetSubgridLocsForShard(Shard s)
    {
        var subgridlocs=s.GetAllSubgridLocations();
        foreach (var loc in subgridlocs)
        {
            if(IsSubgridOob(loc))
            {
                Debug.LogError("trying to set invalid subgrid location " + loc + " for a shard. list of locs:" );
                foreach(var l in subgridlocs)
                {
                    Debug.Log("" + l);
                }
            }
            else
            {
                shards3x3[loc.x][loc.y] = s;
            }
        }
        
    }
    
    public void DeleteShardAt(Vector2Int subgridPos)
    {
        var shard = shards3x3[subgridPos.x][subgridPos.y];
        GameObject.Destroy(shard.gameObject);
        shards3x3[subgridPos.x][subgridPos.y] = null;
    }

    public void DestroyShard(Shard s)
    {
        GameObject.Destroy(s.gameObject);
    }

}
