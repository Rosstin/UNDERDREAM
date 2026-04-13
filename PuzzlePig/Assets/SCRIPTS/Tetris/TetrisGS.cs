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
    
    [Header("Materials")]
    public Material classicMat;
    public Material dietMat;
    public Material cherryMat;
    public Material bajaBlastMat;
    public Material lemonLime;
    public Material orangeMat;
    public Material grapeMat;
    public Material newCoke;
    public Material vanillaMat;

    [Header("Outlets")]
    public Camera mainCamera;
    public Collider CollisionPlane;
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
    
    [Header("Size Weights")] 
    public int X1_WEIGHT = 50;
    public int X3_WEIGHT = 50;

    [Header("Flavor Weights")] 
    public int CLASSIC_WEIGHT = 10;
    public int DIET_WEIGHT = 10;
    public int CHERRY_WEIGHT = 10;
    public int NEW_WEIGHT = 10;
    public int VANILLA_WEIGHT = 10;
    public int BAJABLAST_WEIGHT = 10;
    public int LEMONLIME_WEIGHT = 10;
    public int ORANGE_WEIGHT = 10;
    public int GRAPE_WEIGHT = 10;
    
    private WeightedList<ShardFlavors> weightedShardFlavors = null;
    private WeightedList<ShardSize> weightedShardSizes = null;
    
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
    
    public enum ShardSize
    {
        Unset,
        x1,
        x3,
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
        weightedShardSizes.Add(ShardSize.x1, X1_WEIGHT);
        weightedShardSizes.Add(ShardSize.x3, X3_WEIGHT);

    }
    
    

    public ShardFlavors GetWeightedRandomFlavor()
    {
        return weightedShardFlavors.Next();
    }

    public ShardSize GetWeightedRandomSize()
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
                
                //RaycastHit hit;

                Vector2Int xy = this.colorGrid.CheckCoordHit(ray);
                
                heldRet.UpdateRetPos(xy);

                /*
                if(CollisionPlane.Raycast(ray, out hit, 100f))
                {
                    heldRet.UpdateRetPosition(hit.point);

                }
            */
            }
                    
        }
    }

    
    public Vector3 SnapToGrid(Vector3 pos)
    {
        // rtodo: snap the position to a grid 1/3 the size 

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
    
    public void PlayFireSfx()
    {
        this.psh.Play();
        //this.rocket.Play();
    }

}
