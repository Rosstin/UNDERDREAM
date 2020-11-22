using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private MeshRenderer meshRenderer;

    public void MakeBlack()
    {
        meshRenderer.material = blackMaterial;
    }

    public void MakeGreen()
    {
        meshRenderer.material = greenMaterial;
    }

    public void MakeRed()
    {
        meshRenderer.material = redMaterial;
    }

    public void MakeYellow()
    {
        meshRenderer.material = yellowMaterial;
    }


}
