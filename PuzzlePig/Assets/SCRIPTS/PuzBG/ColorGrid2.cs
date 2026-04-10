using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class for the Tetris game that generates the backing grid and makes the tetris blocks
/// </summary>
public class ColorGrid2 : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorRow> Rows;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomWall;
    public GameObject backPosRef;

    [Header("Prefabs")] 
    public ColorRow2 cRowPref;


    private BlockContainer myBlockContainer;
    private TetrisGS gamestate;
    private Vector2Int dimens;
    private GameObject topLeftAnchor;

    public void Init(TetrisGS gs, BlockContainer bc, Vector2Int gridDimens, GameObject topLeftAnch)
    {
        this.gamestate = gs;
        this.myBlockContainer = bc;
        this.myBlockContainer.Init(gs, gridDimens, topLeftAnch, this.backPosRef );
        
        this.GenerateGrid(gridDimens, topLeftAnch);
        
        this.GenerateLevelBlocks();


        
    }
    
    private void ClearBlocks()
    {
        myBlockContainer.ClearBlocks();
    }

    
    public void GenerateGrid(Vector2Int dimens, GameObject topLeftPos)
    {
        
        this.dimens = dimens;
        this.topLeftAnchor = topLeftPos;

        for(int y = 0; y < this.dimens.y; y++)
        {
            var cRow = GameObject.Instantiate(cRowPref).GetComponent<ColorRow2>();
            
            cRow.transform.SetParent(this.transform);

            cRow.transform.localPosition = new Vector3(topLeftAnchor.transform.localPosition.x, topLeftAnchor.transform.localPosition.y - y*1f, topLeftAnchor.transform.localPosition.z);
            
            cRow.Init(y,this.dimens.x);

        }
        
        
        
        
        
    }

    public void GenerateLevelBlocks()
    {
        myBlockContainer.GenerateLevelBlocks();
        
    }

    
    public Vector3 SnapToGrid(Vector3 pos)
    {
        // snap the pos to our grid
        
        // we're basically using ints - if you intify it, it should work perfectly

        Vector3 intOffset = new Vector3(
            -0.5f,
            -0.5f,
            0f);

        Vector3 gridOffset = new Vector3(
            +0.5f,
            +1.0f,
            0f);
        
        Vector3Int snappedPos = new Vector3Int(
            (int)(pos.x+intOffset.x), 
            (int)(pos.y+intOffset.y), 
            (int)(pos.z+intOffset.z));

        

        Vector3 snappedPosNormed = snappedPos+gridOffset;
        
        Vector3 snappedToBack = new Vector3(snappedPosNormed.x, snappedPosNormed.y, backPosRef.transform.position.z);
        
        
        
        
        return snappedToBack;
    }

    public CompositeBlock GenerateBlock()
    {
        return myBlockContainer.GenerateBlock();

    }
}
