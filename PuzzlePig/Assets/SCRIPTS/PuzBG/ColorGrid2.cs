using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class for the Tetris game that generates the backing grid and makes the tetris blocks
/// </summary>
public class ColorGrid2 : MonoBehaviour
{
    [Header("Outlets")]
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

    public Vector3 SnapZToBack(Vector3 pos)
    {
        return new Vector3(pos.x, pos.y, backPosRef.transform.position.z);
    }

    public Vector3 SupergridToWorld(int x, int y)
    {
        return new Vector3(GetRealXForCol(x), GetRealYForRow(y), backPosRef.transform.localPosition.z);
    }

    public Vector3 SupergridToWorld(Vector2Int xy)
    {
        return this.SupergridToWorld(xy.x, xy.y);
    }

    public Vector2Int WorldToSupergrid(Vector3 pos)
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

    public CompositeBlock GetCompositeBlockForCoord(Vector2Int coord)
    {
        return this.myBlockContainer.GetCompBlockForCoord(coord);
    }
    
    public ColorBlock GetColorBlockForCoord(Vector2Int coord)
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
                Debug.Log("xc " + xc + " is out of range colorRows[yc].ColorBlocks.Count " +  colorRows[yc].ColorBlocks.Count);
                return null;
            }


            
        }
        else
        {
            Debug.Log("yc " + yc + " is out of range colorRows.Count " +  colorRows.Count);
            return null;
        }


    }

    public void GenerateLevelBlocks()
    {
        myBlockContainer.GenerateLevelBlocks();
        
    }

    public bool IsInRange(Vector2Int c)
    {
        if (c.x < 0 || c.x >= this.dimens.x || c.y < 0 || c.y >= this.dimens.y)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void HighlightBackBlockAtCoord(Vector2Int c)
    {
        UnhighlightAll();

        var bl=this.GetColorBlockForCoord(c);
        if (bl == null)
        {
            Debug.LogError("block wasnt found. Coord was " + c);
            return;
        }
        else
        {
            bl.Highlight();
        }


    }
    
    private void HighlightBackBlockAtPos(Vector3 pos)
    {
        // given a collision pos w the backboard, work back to the indexed block

        Vector3 snappedPos = SnapToGrid(pos);
        var c=WorldToSupergrid(snappedPos);

        HighlightBackBlockAtCoord(c);

    }

    private void UnhighlightAll()
    {
        foreach (var r in colorRows)
        {
            foreach (var b in r.ColorBlocks)
            {
                b.Unhighlight();
            }
        }

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

    //Vector2Int 
    
    public IEnumerator ScoreBlock(CompositeBlock scoringBlock)
    {
        // given the block and it's position, destroy adjacencies 

        var scoringBlockCoord=this.WorldToSupergrid(scoringBlock.transform.position);

        Debug.Log("score block at " +scoringBlockCoord);

        if (scoringBlock.GetShardSize() == TetrisGS.ShardSize.x1)
        {
            // check if there are any adjacencies 

            TetrisGS.ShardFlavors flavor =scoringBlock.GetFlavorForIndex();
            Debug.Log("score full block w flavor " + flavor);


        }
        else if (scoringBlock.GetShardSize() == TetrisGS.ShardSize.x3)
        {
            //Debug.Log("score 3x3 block");
            // check if there are any adjacencies 

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (x == 1 && y == 1)
                    {
                        continue;
                        //Debug.Log("center shard can't be scored. it's flavor: " + scoringBlock.GetFlavorForIndex(x,y));
                    }

                    // look at the shards that you're contiguous with and destroy them if they match
                    
                    var myFlav = scoringBlock.GetFlavorForIndex(x, y);

                    // top left corner
                    if (x == 0 && y == 0)
                    {
                        // check my left and check my above

                        var compToLeft = scoringBlock.GetSupergridLoc()+new Vector2Int(-1,0);
                        
                        
                        
                        
                        var shardsWithFlav = this.myBlockContainer.flavToListOfShards[flav];
                        foreach (var s in shardsWithFlav)
                        {
                            s.SetVisible(false);
                            Debug.Log("shard flav " + flav + " at shard positi: " + s.GetSuperAndSubgridLocs().supergridLoc + "");
                        }

                    }

                    // left side
                    if (x == 0 && y == 1)
                    {
                        //Debug.Log("left side shard at " + x + "," + y + " has flavor " + scoringBlock.GetFlavorForIndex(x,y));
                        // check block to my left
                        
                        // look at block to the left of me, specifically at the relevant index.
                        // my index is 0,1. The block to my left, the index would be 2,1

                        Vector2Int coordOfBlockToMyLeft = scoringBlockCoord +  new Vector2Int(-1,0);
                        
                        var blockToMyLeft = this.GetCompositeBlockForCoord(coordOfBlockToMyLeft);
                        if (blockToMyLeft == null)
                        {
                            Debug.Log("no block to my left");
                        }
                        else
                        {
                            var flavorToMyLeft = blockToMyLeft.GetFlavorForIndex(2, y);
                            Debug.Log("found block to my left. its flavor is  " + flavorToMyLeft);
                        }

                    }
                    
                    
                    
                    
                }
            }
            
            
        }
        else
        {
            Debug.LogError("shard size of the block isnt valid ");
        }
        
        yield return new WaitForSeconds(0.75f);

        this.gamestate.heldRet.FinishedScoring();

    }

    public Vector2Int CheckCoordHit(Ray ray)
    {
        for (int y = 0; y < colorRows.Count; y++)
        {
            var row = colorRows[y];
            for (int x = 0; x < row.ColorBlocks.Count; x++)
            {

                var block = row.ColorBlocks[x];

                RaycastHit hit;
                if (block.collider.Raycast(ray, out hit, 100f))
                {
                    //Debug.Log("hit at [" + x + ", " + y + "]");
                    return new Vector2Int(x, y);
                }

            }
            
        }

        return new Vector2Int(-1, -1);
    }
}
