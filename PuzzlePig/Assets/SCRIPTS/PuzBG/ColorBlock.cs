using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : MonoBehaviour
{
    [Header("Outlets")]
    public MeshRenderer MyRend;

    public void SetMat(Material mat)
    {
        MyRend.material = mat;
    }

}
