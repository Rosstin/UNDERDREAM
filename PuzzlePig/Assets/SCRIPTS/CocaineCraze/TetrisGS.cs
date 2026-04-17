using System;
using System.Collections;
using System.Collections.Generic;
using KaimiraGames;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisGS : BaseController
{
    [Header("Prefabs")] 
    public CompositeBlock compPrefab;
    
    [Header("Containers")]
    public BlockContainer blockContainer;
    
    [Header("9 Flavor Materials")]
    public Material classicMat;
    public Material dietMat;
    public Material cherryMat;
    public Material bajaBlastMat;
    public Material lemonLime;
    public Material orangeMat;
    public Material grapeMat;
    public Material newCoke;
    public Material vanillaMat;

    [Header("Scoring Materials")]
    public Material scoreMat;

    [Header("Outlets")]
    public Camera mainCamera;
    public TetrisRet heldRet;
    public ColorGrid2 colorGrid;
    public GameObject gridTopLeftAnchor;
    
    [Header("Configs")]
    public Vector2Int gridDimens = new Vector2Int(5, 11);


    [Header("Configs")] 
    public AudioSource psh;
    public AudioSource tink;
    public AudioSource rocket;
    public AudioSource canCrunch;
    public AudioSource pop;
    
    [Header("Size Weights")] 
    public int SOLID_BLOCK_WEIGHT;
    public int SHARDED_BLOCK_WEIGHT;

    [Header("Flavor Weights")] 
    public int CLASSIC_WEIGHT;
    public int DIET_WEIGHT;
    public int CHERRY_WEIGHT;
    public int NEW_WEIGHT;
    public int VANILLA_WEIGHT;
    public int BAJABLAST_WEIGHT;
    public int LEMONLIME_WEIGHT;
    public int ORANGE_WEIGHT;
    public int GRAPE_WEIGHT;
    
    private WeightedList<ShardFlavors> weightedShardFlavors = null;
    private WeightedList<Vector2Int> weightedShardSizes = null;
    
    public enum ShardFlavors
    {
        Classic,
        Diet,
        Cherry,
        New,
        Vanilla,
        BajaBlast, 
        LemonLime,
        Orange,
        Grape,
        Unset,
    }

    
    public Material GetMatForFlavor(ShardFlavors flavor)
    {
        switch (flavor)
        {
            case ShardFlavors.BajaBlast:
                return bajaBlastMat;
            case ShardFlavors.New:
                return newCoke;
            case ShardFlavors.Vanilla:
                return vanillaMat;
            case ShardFlavors.LemonLime:
                return lemonLime;
            case ShardFlavors.Diet:
                return dietMat;
            case ShardFlavors.Classic:
                return classicMat;
            case ShardFlavors.Cherry:
                return cherryMat;
            case ShardFlavors.Grape:
                return grapeMat;
            case ShardFlavors.Orange:
                return orangeMat;
            case ShardFlavors.Unset:
                Debug.LogError("Unset shard flavor");
                return null;
        }

        return null;
    }

    public void Start()
    {

        GenWeights();
        
        
        colorGrid.Init(this, this.blockContainer, gridDimens, gridTopLeftAnchor);

        CompositeBlock cb = colorGrid.GenerateBlock();
        cb.SetSuperGridLoc(new Vector2Int(0,0)); // set a location so consolidation works
        cb.Consolidate();
        cb.Consolidate();
        
        heldRet.Init(this,cb);
    }

    private void GenWeights()
    {
        weightedShardFlavors = new();
        weightedShardFlavors.Add(ShardFlavors.Classic,CLASSIC_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.Diet,DIET_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.Cherry,CHERRY_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.New,NEW_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.Vanilla,VANILLA_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.BajaBlast,BAJABLAST_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.LemonLime,LEMONLIME_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.Orange,ORANGE_WEIGHT);
        weightedShardFlavors.Add(ShardFlavors.Grape,GRAPE_WEIGHT);

        weightedShardSizes = new();
        weightedShardSizes.Add(new Vector2Int(3,3), SOLID_BLOCK_WEIGHT);
        weightedShardSizes.Add(new Vector2Int(1,1), SHARDED_BLOCK_WEIGHT);

    }
    
    

    public ShardFlavors GetWeightedRandomFlavor()
    {
        return weightedShardFlavors.Next();
    }

    public Vector2Int GetWeightedRandomSize()
    {
        return weightedShardSizes.Next();
    }


    private void Update()
    {
        BaseUpdate();

        UpdateTetrisControls();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateTetrisControls()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            colorGrid.GenerateLevelBlocks();
        }

        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {
            if (heldRet.CanGrab())
            {
                heldRet.GrabBlock();
            }
        }
        
        if (heldRet.IsRetHeld())
        {
            // dropping ing
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire))
            {
                heldRet.FireBlock();
            }
            //holding ing
            else
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                Vector2Int xy = this.colorGrid.CheckCoordHit(ray);

                if (xy.x == -1 || xy.y == -1)
                {
                    // invalid
                }
                else
                {
                    heldRet.UpdateRetPos(xy);
                }
                

            }
                    
        }
    }

    
    public Vector3 SnapToGrid(Vector3 pos)
    {
        return this.colorGrid.SnapToGrid(pos);
    }

    public void PlayTinkSfx()
    {
        this.tink.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
        this.tink.Play();
    }
    public void PlayCanCrunchSfx()
    {
        this.canCrunch.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
        this.canCrunch.Play();
    }

    public void PlayGrabSfx()
    {
        this.pop.Play();
    }
    
    public void PlayFireSfx()
    {
        this.psh.Play();
    }

}
