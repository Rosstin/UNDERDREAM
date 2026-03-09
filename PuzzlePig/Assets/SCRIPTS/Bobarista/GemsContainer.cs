using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsContainer : MonoBehaviour
{
    private const int LONG_WIDTH = 5; // width of the "long" side


    public int GetShortWidth()
    {
        return LONG_WIDTH - 1; // "short" side is long minus 1
    }

    public Vector3 GetPositionForIndex(int rowFromTop, int columnFromLeft)
    {
        // return the real worldspace position of a gem based on its location

        Vector3 gemPos = new Vector3();

        return gemPos;
    }
}
