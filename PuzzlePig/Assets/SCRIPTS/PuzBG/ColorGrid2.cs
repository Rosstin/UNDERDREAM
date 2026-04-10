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

    private List<ColorRow2> colorRows = new List<ColorRow2>();
    
    public void Init(TetrisGS gs, BlockContainer bc, Vector2Int gridDimens, GameObject topLeftAnch)
    {
        this.gamestate = gs;
        this.myBlockContainer = bc;
        this.myBlockContainer.Init(gs,this, gridDimens, topLeftAnch, this.backPosRef );
        
        this.GenerateGrid(gridDimens, topLeftAnch);
        
        this.GenerateLevelBlocks();


        
    }
    
    private void ClearBlocks()
    {
        myBlockContainer.ClearBlocks();
    }

    public float GetRealYForRow(int y)
    {
        return topLeftAnchor.transform.localPosition.y - y * 1f;
    }

    public int GetRowForRealY(float posy)
    {
        float yFloat = (int)(-posy + topLeftAnchor.transform.localPosition.y) / 1f;

        int yInt = (int)(yFloat);
        return yInt;
    }

    public float GetRealXForCol(int x)
    {
        return topLeftAnchor.transform.localPosition.x + x * 1f;
    }

    public int GetColForRealX(float posx)
    {
        float xFloat = (int)(posx - topLeftAnchor.transform.localPosition.x) / 1f;
        int xInt = (int)(xFloat);
        return xInt;
    }

    public Vector3 GetPosForCoord(int x, int y)
    {
        return new Vector3(GetRealXForCol(x), GetRealYForRow(y), backPosRef.transform.localPosition.z);
    }


    public Vector2Int GetCoordForPos(Vector3 pos)
    {

        return new Vector2Int(GetColForRealX(pos.x), GetRowForRealY(pos.y));

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
            colorRows.Add(cRow);
        }
    }

    public ColorBlock GetBlockForCoord(Vector2Int coord)
    {
        int xc = coord.x;
        int yc = coord.y;

        if (yc >= 0 && yc < colorRows.Count)
        {
            if (xc >= 0 && xc < colorRows[yc].ColorBlocks.Count)
            {
                return colorRows[yc].ColorBlocks[xc];
            }
            else
            {
                Debug.LogError("xc " + xc + " is out of range colorRows[yc].ColorBlocks.Count " +  colorRows[yc].ColorBlocks.Count);
                return null;
            }


            
        }
        else
        {
            Debug.LogError("yc " + yc + " is out of range colorRows.Count " +  colorRows.Count);
            return null;
        }


    }

    public void GenerateLevelBlocks()
    {
        myBlockContainer.GenerateLevelBlocks();
        
    }

    public void HighlightBackBlockAtPos(Vector3 pos)
    {
        // given a collision pos w the backboard, work back to the indexed block

        Vector3 snappedPos = SnapToGrid(pos);
        var c=GetCoordForPos(snappedPos);

        var bl=this.GetBlockForCoord(c);
        if (bl == null)
        {
            Debug.LogError("block for pos " + pos + " wasnt found. Coord was " + c);
        }

        foreach (var r in colorRows)
        {
            foreach (var b in r.ColorBlocks)
            {
                b.Unhighlight();
            }
        }
        
        bl.Highlight();

    }

    public Vector3 SnapToGrid(Vector3 pos)
    {
        // snap the pos to our grid
        
        // we're basically using ints - if you intify it, it should work perfectly

        // if you're touching a back panel, you should snap to that pos
        
        
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
