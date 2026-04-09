using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGrid2 : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorRow> Rows;
    public Transform topLeftAnchor;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomWall;
    
    [Header("Configs")]
    public Vector2 dimens;
    
    void Start()
    {
        
        
        
        /*
        for(int i = 0; i < Rows.Count; i++)
        {
            Rows[i].Init(i);
        }
        */
        
    }

    public void GenerateGrid(Vector2 dimens, Transform topLeftPos)
    {
        this.dimens = dimens;
        this.topLeftAnchor = topLeftPos;

        
        
        
        
    }
}
