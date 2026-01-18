using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public GameObject sunMesh;

    public void Initialize(float scale)
    {
        sunMesh.transform.localScale = new Vector3(scale, scale, scale);
        sunMesh.transform.localPosition = new Vector3(0f,0f,0f);
    }

}


