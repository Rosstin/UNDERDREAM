using System.Collections.Generic;
using UnityEngine;

public static class CocaineCrazeConstants
{
    public static Vector2Int UP = new Vector2Int(0, 1);
    public static Vector2Int DOWN = new Vector2Int(0, -1);
    public static Vector2Int LEFT = new Vector2Int(-1, 0);
    public static Vector2Int RIGHT = new Vector2Int(1, 0);
    
    public static List<Vector2Int> UP_DOWN_LEFT_RIGHT = new List<Vector2Int>()
        { UP, DOWN, LEFT, RIGHT };

}