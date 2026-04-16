using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRow2 : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorBlock> ColorBlocks;
    public List<Material> Colors;

    [Header("Prefabs")] 
    public ColorBlock colorBlockPrefab;

    private int startingIndex;

    private int myBlocksInRow;

    public void ClearRow()
    {
        foreach (var c in ColorBlocks)
        {
            GameObject.Destroy(c.gameObject);
        }
        ColorBlocks.Clear();
    }
    
    public void Init(int startingIndex, int blocksInRow)
    {
        ClearRow();
        
        this.startingIndex = startingIndex;

        this.myBlocksInRow = blocksInRow;

        for(int i = 0; i < blocksInRow; i++)
        {
            ColorBlock c =GameObject.Instantiate(colorBlockPrefab).GetComponent<ColorBlock>();
            
            c.SetMyColor(Colors[GetMatIndex(this.startingIndex, i)]);
            c.Unhighlight();
            
            c.transform.SetParent(this.transform);
            c.transform.localPosition = new Vector3(1*i,0,0);
            
            c.SetLocationText(""+i+", " + startingIndex,true);
            
            ColorBlocks.Add(c);
        }
    }

    private int GetMatIndex(int startingIndex, int blockIndex)
    {
        int ind = startingIndex + blockIndex;
        while(ind >= Colors.Count)
        {
            ind -= Colors.Count;
        }


        return ind;
    }

}
