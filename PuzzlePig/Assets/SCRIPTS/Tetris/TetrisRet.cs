using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisRet : MonoBehaviour
{
    private CompositeBlock myBlock=null;

    public void Init(CompositeBlock block)
    {
        this.myBlock = block;
    }
    
    public void SetBlockPosition(Vector3 pos)
    {
        if (myBlock != null)
        {
            myBlock.transform.position = pos;
        }
    }
    
}
