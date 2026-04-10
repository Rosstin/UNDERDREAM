using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 GetPosForCoord(int x, int y)
    {
        return new Vector3(topLeftAnchor.transform.localPosition.x + x *1f, topLeftAnchor.transform.localPosition.y - y*1f, backPosRef.transform.localPosition.z);
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
}
