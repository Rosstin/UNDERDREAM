using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GemsContainer : MonoBehaviour
{
    [Header("Numerical Configs")] 
    public int LONG_WIDTH; // should be 5 - width of the "long" side
    public float gemDiameter; // should be 1
    public float zDistance;
    
    [Header("Gem Container Parent")]
    public GameObject parent;

    [Header("Gem Prefab")]
    public List<GameObject> gemPrefabs;

    [Header("Positional Reference")] 
    public Transform upperLeftPosition;
    
    public void Start()
    {
        GenerateRandomGemPattern(6);
    }

    private void ClearContainer()
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public int GetShortWidth()
    {
        return LONG_WIDTH - 1; // "short" side is long minus 1
    }

    public int GetColumnWidth(int columnIndex)
    {
        // 0 or even is 'long', odd is 'short'

        if (columnIndex % 2 == 0)
        {
            return LONG_WIDTH;
        }
        else
        {
            return GetShortWidth();
        }
    }

    public void GenerateRandomGemPattern(int numRows)
    {
        ClearContainer();
        // starting from the top, cycle through the 4 gems
        for (int y = 0; y < numRows; y++)
        {
            int colWidth = GetColumnWidth(y);
            for (int x = 0; x < colWidth; x++)
            {

                // get a random or arbitrary color                
                int randomInt = UnityEngine.Random.Range(0, gemPrefabs.Count+1);

                if (randomInt == gemPrefabs.Count)
                {
                    // skip it [empty]
                }
                else
                {
                    var chosenPref = gemPrefabs[randomInt];
                    
                    var gem = GameObject.Instantiate(chosenPref, parent.transform);
                    
                    var gemPos = GetPositionForIndex(y, x);

                    gem.transform.position = gemPos;
                    
                    
                    
                }

                
                

            }
            
            
            
        }
        
        
    }

    public Vector3 GetPositionForIndex(int rowFromTop, int columnFromLeft)
    {
        // return the real worldspace position of a gem based on its location
        // given the radius of the gem, position them
        // odd rows, even rows
        // you can pack them in tighter if you want
        
        bool isOdd = rowFromTop % 2 == 1;

        float oddOffset = 0;
        if (isOdd)
        {
            oddOffset = gemDiameter / 2f;
        }
        
        
        Vector3 position = new Vector3(+oddOffset + columnFromLeft * gemDiameter,  -rowFromTop * gemDiameter, zDistance);
        
        // if position is 0,0,0, it should be upper left position
        
        

        return (position+upperLeftPosition.position);
    }
}
