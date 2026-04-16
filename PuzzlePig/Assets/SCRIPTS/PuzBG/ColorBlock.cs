using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : MonoBehaviour
{
    [Header("Outlets")]
    public MeshRenderer MyRend;
    public Material highlightColor;
    public BoxCollider collider;
    public TextMesh locationText;
    
    private Material myColor;
    
    public enum BlockState
    {
        Unset,
        Normal,
        Highlight
    }

    public void SetLocationText(string text, bool enabled)
    {
        this.locationText.gameObject.SetActive(enabled);
        this.locationText.text = text;
    }
    
    public void SetMyColor(Material mat)
    {
        this.myColor = mat;
    }

    public void SetMat(Material mat)
    {
        MyRend.material = mat;
    }
    
    public void Highlight()
    {
        SetMat(this.highlightColor);
    }

    public void Unhighlight()
    {
        this.SetMat(this.myColor);
    }

}
