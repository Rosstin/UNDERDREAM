using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRow : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorBlock> ColorBlocks;
    public List<Material> Colors;

    private int startingIndex;

    public void Init(int startingIndex)
    {
        this.startingIndex = startingIndex;


        for(int i = 0; i < ColorBlocks.Count; i++)
        {

            ColorBlocks[i].SetMat(Colors[GetMatIndex(this.startingIndex, i)]);
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
