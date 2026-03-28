using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsContainer : MonoBehaviour
{
    private const int LONG_WIDTH = 5; // width of the "long" side

    public float gemDiameter; // should be 1

    public float zDistance;
    

    public int GetShortWidth()
    {
        return LONG_WIDTH - 1; // "short" side is long minus 1
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
        
        
        Vector3 position = new Vector3(columnFromLeft * gemDiameter, +oddOffset -rowFromTop * gemDiameter, zDistance);
        
        // snap them to nearest pos
        
        
        
        Vector3 gemPos = new Vector3();

        return gemPos;
    }
}
