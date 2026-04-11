using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockContainer : MonoBehaviour
{
    private List<List<CompositeBlock>> myLandedBlocks = null;

    private Vector2Int dimens;
    
    private GameObject topLeftAnchor;
    private ColorGrid2 colorGrid;
    private GameObject backPosRef;
    private TetrisGS gamestate;
    public void Init( TetrisGS gs, ColorGrid2 cg2, Vector2Int dimens, GameObject topLeftAnchor, GameObject backPosRef)
    {
        this.colorGrid = cg2;
        this.gamestate = gs;
        this.backPosRef = backPosRef;
        this.dimens = dimens;
        
        this.myLandedBlocks = new List<List<CompositeBlock>>();
        for (int x = 0; x < dimens.x; x++)
        {
            this.myLandedBlocks.Add(new List<CompositeBlock>());
            for (int y = 0; y < dimens.y; y++)
            {
                this.myLandedBlocks[x].Add(null);
            }
        }
        
        this.topLeftAnchor = topLeftAnchor;
    }

    public int GetWidth()
    {
        return this.dimens.x;
    }

    public int GetHeight()
    {
        return this.dimens.y;
    }

    public void GenerateLevelBlocks()
    {
        ClearBlocks();
        
        // todo generate a random grid of random blocks
        
        // for each column, generate a number of blocks between 0 and 4
        for (int x = 0; x < this.dimens.x; x++)
        {
            int blocksInC =Random.Range(0, 5);
            for (int y = 0; y < blocksInC; y++)
            {

                int yCoord = this.GetHeight() - 1 - y;
                
                CompositeBlock newBlock = this.GenerateBlock();

                this.myLandedBlocks[x][yCoord] = newBlock;

                // feed an appropriate coord for the block
                newBlock.transform.localPosition = this.colorGrid.GetPosForCoord(x, yCoord);


            }
        }

    }


    public void ClearBlocks()
    {
        for (int x = 0; x < myLandedBlocks.Count; x++)
        {
            List<CompositeBlock> col = myLandedBlocks[x];
            
            for (int y = 0; y < col.Count; y++)
            {
                CompositeBlock block = myLandedBlocks[x][y];                
                
                if (block != null)
                {
                    block.Clear();
                    GameObject.Destroy(block.gameObject);
                    block = null;
                }
            }
        }
    }

    public CompositeBlock GenerateBlock()
    {
        CompositeBlock cb = GameObject.Instantiate(this.gamestate.compPrefab).GetComponent<CompositeBlock>();
        cb.Init(this.gamestate, this.gamestate.blockContainer);

        cb.Randomize();


        return cb;
    }

    public float GetLowestYForColumn(int colX)
    {
        // given a column, get lowest y as a real position

        if (colX >= 0 && colX < myLandedBlocks.Count)
        {
            var column = myLandedBlocks[colX];
        
            // look for the first empty

            int yCand = column.Count;
            for (int y = 0; y < column.Count; y++)
            {
                CompositeBlock block = column[y];

                if (block != null)
                {
                    yCand = y;
                    break;
                }
            
            }

            return this.colorGrid.GetRealYForRow(yCand-1);
        }
        else
        {
            //Debug.LogError("attempting to get a Y for a column " + colX + " that is invalid");
            return -1;
        }
        


    }

    public Vector3 GetRestingPosForBlockFallFrom(Vector3 snappedPos)
    {
        var c=this.colorGrid.GetCoordForPos(snappedPos);
        float lowestY=GetLowestYForColumn(c.x);

        return new Vector3(snappedPos.x, lowestY, backPosRef.transform.position.z);

    }
}
