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
    
    [Header("Configs")]
    public Vector3 retStartPos;
    public Vector2Int gridDimens = new Vector2Int(5, 11);
    
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
            
        colorGrid.Init(this);
        colorGrid.GenerateGrid(gridDimens, gridTopLeftAnchor);
        
        colorGrid.GenerateLevelBlocks();

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
        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {
            heldRet.GrabBlock();
        }
        
        if (heldRet.IsRetHeld())
        {
            // dropping ing
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire))
            {
                // calculate the direction between the current position and the startpos
                Vector3 shootDirection = retStartPos - heldRet.transform.position;
                
                shootDirection.Normalize();

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
        heldRet.transform.position = pos;
    }
    
    public Vector3 SnapToGrid(Vector3 pos)
    {
        // rtodo: snap the position to a grid 1/3 the size 

        return this.colorGrid.SnapToGrid(pos);
    }

}
