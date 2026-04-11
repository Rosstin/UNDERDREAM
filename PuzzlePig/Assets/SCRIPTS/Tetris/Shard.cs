using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    [Header("Renderer")]
    public MeshRenderer meshRenderer;
    
    public TetrisGS.ShardFlavors myFlavor;

    public Vector2 TopCornerPosition;

    public Vector2 Dimensions;

    private TetrisGS myGamestate = null;

    //private const float oneThird = 1.0f / 3.0f;
    
    public void Init(TetrisGS gamestate, GameObject parent, TetrisGS.ShardFlavors flav, Vector3 pos)
    {
        this.myGamestate = gamestate;
        this.SetFlavor(flav);
        this.transform.SetParent(parent.transform);
        this.transform.localScale = new Vector3((1/3f), (1/3f), (1/3f));
        this.transform.position = pos;
    }

    private void SetFlavor(TetrisGS.ShardFlavors flavor)
    {
        this.myFlavor = flavor;
        meshRenderer.material = this.myGamestate.GetMatForFlavor(flavor);

    }
}
