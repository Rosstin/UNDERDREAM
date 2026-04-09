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
    
    public void Randomize()
    {
        Clear();
        // for starters, let's generate a 3x3 grid of colors. Then let's simplify the shards
        
        // generating a 3x3
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                TetrisGS.ShardFlavors randomFlavor = GenerateRandomFlavor();
                Shard shard = GameObject.Instantiate(shardPrefab).GetComponent<Shard>();
                shard.Init(gamestate, this, randomFlavor, x, y);

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
