using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColorGrid2 : MonoBehaviour
{
    [Header("Outlets")]
    public List<ColorRow> Rows;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomWall;

    [Header("Prefabs")] 
    public ColorRow2 cRowPref;

    
    private Vector2Int dimens;
    private GameObject topLeftAnchor;

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
}
