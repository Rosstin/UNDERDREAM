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

    private GameObject backPosRef;
    private TetrisGS gamestate;
    public void Init( TetrisGS gs, Vector2Int dimens, GameObject topLeftAnchor, GameObject backPosRef)
    {
        this.gamestate = gs;
        this.myLandedBlocks = new List<List<CompositeBlock>>();
        this.backPosRef = backPosRef;
        this.dimens = dimens;
        
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
                newBlock.transform.localPosition = this.GetPosForCoord(x, yCoord);


            }
        }

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

        var column = myLandedBlocks[colX];
        
        // look for the first empty

        int yCand = -1;
        for (int y = 0; y < column.Count; y++)
        {
            CompositeBlock block = column[y];

            if (block != null)
            {
                yCand = y;
                break;
            }
            
        }

        return GetRealYForRow(yCand-1);


    }

    public Vector3 GetRestingPosForBlockFallFrom(Vector3 snappedPos)
    {
        var c=GetCoordForPos(snappedPos);
        float lowestY=GetLowestYForColumn(c.x);

        return new Vector3(snappedPos.x, lowestY, backPosRef.transform.position.z);

    }
}
