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
    public TetrisRet heldRet = null;
    
    [Header("Configs")]
    public Vector3 startPos;

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
        BaseUpdate();

        UpdateTetrisControls();
    }

    private void UpdateTetrisControls()
    {
                
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("random block ");
            
            ClearBlocks();
            
            CompositeBlock cb = GameObject.Instantiate(compPrefab).GetComponent<CompositeBlock>();
            cb.Init(this, blockContainer);
            
            cb.Randomize();
            
            blocks.Add(cb);
            
        }

        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {

            
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
            
                TetrisRet bobaRet = objectHit.gameObject.GetComponent<TetrisRet>();

                if (bobaRet != null)
                {
                    heldRet = bobaRet;
                }
            }
                    
        }
        
        if (heldRet != null)
        {
            // dropping ing
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire))
            {
                // calculate the direction between the current position and the startpos
                Vector3 shootDirection = startPos - heldRet.transform.position;
                
                shootDirection.Normalize();

                //DummyBall.transform.position = startPos + shootDirection * fireDist;
                
                /*
                DummyBall.transform.position = startPos;
                
                DummyBall.Shoot(shootDirection, speed);
                */
                
                
                
                
                // we want to actually fire the ball, have it bounce from walls, stop when it hits another ball
                
                
                heldRet.transform.position = startPos;
                heldRet = null;
            }
            //holding ing
            else
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit hit;
                
                if(CollisionPlane.Raycast(ray, out hit, 100f))
                {

                    heldRet.transform.position = hit.point;
                }
            }
                    
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
