using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompositeBlock : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] public Shard shardPrefab;

    [Header("My Bounds")] [SerializeField] 
    public GameObject topLeftRef;
    public GameObject botRightRef;
    
    private List<Shard> myShards = new List<Shard>();

    private TetrisGS gamestate = null;

    System.Random random = new System.Random();
    
    public void Init(TetrisGS gs, BlockContainer blockCont)
    {
        this.gamestate = gs;
        this.transform.SetParent(blockCont.transform);
        this.transform.localPosition = Vector3.zero;
    }

    public void Clear()
    {
        foreach (Shard shard in myShards)
        {
            GameObject.Destroy(shard.gameObject);
        }
        myShards.Clear();
    }

    private float GetBlockWidth()
    {
        return botRightRef.transform.position.x- topLeftRef.transform.position.x;
    }

    public float GetBlockHeight()
    {
        return topLeftRef.transform.position.y - botRightRef.transform.position.y;
    }
    
    private Vector3 GetPositionForIndex(int x, int y)
    {
        Vector3 pos = topLeftRef.transform.position + new Vector3(GetBlockWidth() * (1f/6f), -GetBlockHeight() * (1f/6f), 0);

        pos += new Vector3(x*(GetBlockWidth()/3f), -y*(GetBlockHeight()/3f), 0);

        return pos;
    }
    
    public void Randomize()
    {
        Clear();
        // for starters, let's generate a 3x3 grid of colors. Then let's simplify the shards
        
        
        // calculate the center pos and whatnot
        
        
        // generating a 3x3
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                TetrisGS.ShardFlavors randomFlavor = GenerateRandomFlavor();
                Shard shard = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
                shard.Init(gamestate, this, randomFlavor, GetPositionForIndex(x,y));

                myShards.Add(shard);

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
}
