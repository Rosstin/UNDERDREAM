using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TetrisGS : BaseController
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

    [Header("Outlets")]
    public Camera mainCamera;
    public Collider CollisionPlane;
    public TetrisRet heldRet;
    public ColorGrid2 colorGrid;
    public GameObject gridTopLeftAnchor;
    public GameObject refSpot;
    
    [Header("Configs")]
    public Vector3 retStartPos;
    public Vector2Int gridDimens = new Vector2Int(5, 11);


    [Header("Configs")] 
    public AudioSource psh;
    public AudioSource tink;
    public AudioSource rocket;
    public AudioSource canCrunch;
    
    
    public enum ShardFlavors
    {
        Aqua,
        Crystal,
        Emerald,
        LemonLime,
        Black,
        Classic,
        Cherry,
        Sapphire,
        Orange,
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
            case ShardFlavors.Aqua:
                return aquaMat;
            case ShardFlavors.Crystal:
                return diaMat;
            case ShardFlavors.Emerald:
                return emMat;
            case ShardFlavors.LemonLime:
                return limMat;
            case ShardFlavors.Black:
                return obMat;
            case ShardFlavors.Classic:
                return raspMat;
            case ShardFlavors.Cherry:
                return rubMat;
            case ShardFlavors.Sapphire:
                return sapMat;
            case ShardFlavors.Orange:
                return tangMat;
            case ShardFlavors.Unset:
                Debug.LogError("Unset shard flavor");
                return null;
        }

        return null;
    }

    public void Start()
    {
            
        colorGrid.Init(this, this.blockContainer, gridDimens, gridTopLeftAnchor);
        CompositeBlock cb = colorGrid.GenerateBlock();
        
        heldRet.Init(this,cb);
    }

    private void Update()
    {
        BaseUpdate();

        UpdateTetrisControls();
    }

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
                UpdateRetPosition(retStartPos);

                heldRet.FireBlock();
            }
            //holding ing
            else
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit hit;
                
                if(CollisionPlane.Raycast(ray, out hit, 100f))
                {
                    UpdateRetPosition(hit.point);

                }
            }
                    
        }
    }

    private void UpdateRetPosition(Vector3 pos)
    {
        // check if position is in range - ignore if not 
        bool inrange=this.colorGrid.IsInRange(pos);


        if (inrange)
        {
            colorGrid.HighlightBackBlockAtPos(pos);
            refSpot.transform.position = pos;
            heldRet.transform.position = pos;
        }
        else
        {
            // not in range - ignore
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
