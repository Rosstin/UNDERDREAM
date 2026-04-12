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

    private Vector2Int myLocalSubgridLocation; // if this is -1,-1 it means you're a large shard. otherwise, 1,1 is the center, 0,0 is the top right corn, etc
    
    public void Init(TetrisGS gamestate, GameObject parent, TetrisGS.ShardFlavors flav, Vector3 pos, TetrisGS.ShardSize shardSize, Vector2Int localSubgridLoc)
    {
        this.mySize = shardSize;
        this.myGamestate = gamestate;
        this.SetFlavor(flav);
        this.transform.SetParent(parent.transform);
        this.myLocalSubgridLocation = localSubgridLoc;

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

    public TetrisGS.ShardFlavors GetFlavor()
    {
        return myFlavor;
    }
}
