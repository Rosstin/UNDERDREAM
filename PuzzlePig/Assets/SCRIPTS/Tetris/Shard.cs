using UnityEngine;

public class Shard : MonoBehaviour
{
    [Header("Renderer")]
    public MeshRenderer meshRenderer;
    
    
    
    private TetrisGS.ShardFlavors myFlavor;
    private TetrisGS.ShardSize mySize;
    
    private Vector2 TopCornerPosition;

    private Vector2 Dimensions;

    private TetrisGS myGamestate = null;

    
    
    public void Init(TetrisGS gamestate, GameObject parent, TetrisGS.ShardFlavors flav, Vector3 pos, TetrisGS.ShardSize shardSize)
    {
        this.mySize = shardSize;
        this.myGamestate = gamestate;
        this.SetFlavor(flav);
        this.transform.SetParent(parent.transform);

        switch (shardSize)
        {
            case TetrisGS.ShardSize.x1:
                this.transform.localScale = new Vector3((1/1f), (1/1f), (1/1f));
                break;
            case TetrisGS.ShardSize.x3:
                this.transform.localScale = new Vector3((1/3f), (1/3f), (1/3f));
                break;
            case TetrisGS.ShardSize.Unset:
                Debug.LogError("shard size unset!");
                break;
            
        }
        
        this.transform.position = pos;
    }

    private void SetFlavor(TetrisGS.ShardFlavors flavor)
    {
        this.myFlavor = flavor;
        meshRenderer.material = this.myGamestate.GetMatForFlavor(flavor);

    }
}
