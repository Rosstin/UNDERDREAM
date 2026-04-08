using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeBlock : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] public Shard shardPrefab;

    private List<Shard> myShards = new List<Shard>(); 
    
    public void GenerateRandomBlock()
    {
        // for starters, let's generate a 3x3 grid of colors. Then let's simplify the shards
        
        // generating a 3x3
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                TetrisGS.ShardFlavors randomFlavor = GenerateRandomFlavor();
            }
        }
        
        
        
        
        
        
    }

    /// <summary>
    /// Generate one of 9 random flavors. Todo: more detailed params
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private TetrisGS.ShardFlavors GenerateRandomFlavor()
    {
        throw new System.NotImplementedException();
    }
}
