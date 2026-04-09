using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisGS : MonoBehaviour
{
    [Header("Prefabs")] 
    public CompositeBlock compPrefab;
    
    [Header("Containers")]
    public BlockContainer blockContainer;
    
    [Header("Materials")]
    public Material aquaMat;
    public Material diaMat;
    public Material emMat;
    public Material limMat;
    public Material obMat;
    public Material raspMat;
    public Material rubMat;
    public Material sapMat;
    public Material tangMat;

    public enum ShardFlavors
    {
        Aqua,
        Diamond,
        Emerald,
        Limon,
        Obsidian,
        Raspberry,
        Ruby,
        Sapphire,
        Tangerine,
        Unset,
    }
    
    private List<CompositeBlock> blocks = new List<CompositeBlock>();

    public Material GetMatForFlavor(ShardFlavors flavor)
    {
        switch (flavor)
        {
            case ShardFlavors.Aqua:
                return aquaMat;
            case ShardFlavors.Diamond:
                return diaMat;
            case ShardFlavors.Emerald:
                return emMat;
            case ShardFlavors.Limon:
                return limMat;
            case ShardFlavors.Obsidian:
                return obMat;
            case ShardFlavors.Raspberry:
                return raspMat;
            case ShardFlavors.Ruby:
                return rubMat;
            case ShardFlavors.Sapphire:
                return sapMat;
            case ShardFlavors.Tangerine:
                return tangMat;
            case ShardFlavors.Unset:
                Debug.LogError("Unset shard flavor");
                return null;
        }

        return null;
    }

    public void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ClearBlocks();
            
            CompositeBlock cb = GameObject.Instantiate(compPrefab).GetComponent<CompositeBlock>();
            cb.Init(this, blockContainer);
            
            cb.Randomize();
            
            blocks.Add(cb);
            
        }

    }

    private void ClearBlocks()
    {
        foreach (var b in blocks)
        {
            b.Clear();
            GameObject.Destroy(b.gameObject);
        }
        blocks.Clear();
    }
}
