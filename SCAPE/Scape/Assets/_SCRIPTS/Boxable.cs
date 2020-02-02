using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxable : MonoBehaviour
{
    private bool inBox = true;

    public void UnBox()
    {
        Debug.LogWarning("UNBOX!!");
        inBox = false;
    }

    public bool InBox()
    {
        return inBox;
    }
}
