using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Configs")] 
    public int X1_WEIGHT = 50;
    public int X3_WEIGHT = 50;

    private Vector2Int mySupergridLocation; // -1,-1 means you're not landed on the grid [being held, moving, etc]
    
    private List<List<Shard>> shards3x3 = null;
    private Shard singleShard=null;
    
    private TetrisGS.ShardSize sizeOfMyShards = TetrisGS.ShardSize.Unset;
    
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

    public TetrisGS.ShardSize GetShardSize()
    {
        return sizeOfMyShards;
    }
    
    public void Clear()
    {
        if (singleShard != null)
        {
            GameObject.Destroy(singleShard);
        }

        for (int x = 0; x < shards3x3.Count; x++)
        {
            List<Shard> col = shards3x3[x];

            for (int y = 0; y < col.Count; y++)
            {
                Shard s = col[y];

                if (s != null)
                {
                    GameObject.Destroy(s.gameObject);
                    s = null;
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
    
    public Vector3 GetPositionForIndex(int x, int y)
    {
        Vector3 pos = topLeftRef.transform.position + new Vector3(GetBlockWidth() * (1f/6f), -GetBlockHeight() * (1f/6f), 0);

        pos += new Vector3(x*(GetBlockWidth()/3f), -y*(GetBlockHeight()/3f), 0);

        return pos;
    }
    
    public void Randomize()
    {
        Clear();
        
        int totWeight = X1_WEIGHT + X3_WEIGHT;
        int roll = Random.Range(0, totWeight);
        if (roll < X1_WEIGHT)
        {
            GenX1Block();
        }
        else
        {
            GenX3Block();
        }
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
    public void GenX1Block()
    {
        this.sizeOfMyShards = TetrisGS.ShardSize.x1;
        TetrisGS.ShardFlavors flavForThisBlock = GenerateRandomFlavor();

        Shard s = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
        s.Init(gamestate, contentParent, flavForThisBlock, GetPositionForIndex(1,1), TetrisGS.ShardSize.x1, new Vector2Int(-1,-1));
        
        List<Shard> shardsOfFlav = this.gamestate.blockContainer.flavToListOfShards[flavForThisBlock];
        shardsOfFlav.Add(s);
        this.gamestate.blockContainer.flavToListOfShards[flavForThisBlock] = shardsOfFlav;

        this.singleShard = s;
    }

    public void GenX3Block()
    {        
        this.sizeOfMyShards = TetrisGS.ShardSize.x3;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                TetrisGS.ShardFlavors randomFlavor = GenerateRandomFlavor();
                Shard shard = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
                shard.Init(gamestate, contentParent, randomFlavor, GetPositionForIndex(x,y), TetrisGS.ShardSize.x3, new Vector2Int(x,y));
                
                shards3x3[x][y] = shard;
            }
        }
    }
    
    /// <summary>
    /// Generate one of 9 random flavors. Todo: more detailed params
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private TetrisGS.ShardFlavors GenerateRandomFlavor()
    {
        Array values = Enum.GetValues(typeof(TetrisGS.ShardFlavors));
        TetrisGS.ShardFlavors randomFlavor = (TetrisGS.ShardFlavors)values.GetValue(random.Next(values.Length-1));

        return randomFlavor;
    }

    public void SetPos(Vector3 pos, Vector3 jitter)
    {
        SetJitter(jitter);
        this.transform.position = pos;
    }

    public TetrisGS.ShardFlavors GetFlavorForIndex(int x=-1, int y=-1)
    {
        if (this.sizeOfMyShards == TetrisGS.ShardSize.x1)
        {
            return singleShard.GetFlavor();
        }
        else if (this.sizeOfMyShards == TetrisGS.ShardSize.x3)
        {
            return shards3x3[x][y].GetFlavor();
        }
        else
        {
            Debug.LogError("this comp block is invalid");
            return TetrisGS.ShardFlavors.Unset;
        }
        
        
    }

    public void SetSuperGridLoc(Vector2Int sGridLoc)
    {
        // rtodo merge assignment here with setting things?
        this.mySupergridLocation=sGridLoc;
    }
}
