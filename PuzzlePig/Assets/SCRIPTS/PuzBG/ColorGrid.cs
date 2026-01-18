using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGrid : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorRow> Rows;

    void Start()
    {
        for(int i = 0; i < Rows.Count; i++)
        {
            Rows[i].Init(i);
        }
        
    }
}
