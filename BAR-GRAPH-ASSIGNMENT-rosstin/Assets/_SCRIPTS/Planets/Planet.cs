using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject planetMesh;
    public MeshRenderer planetMeshRenderer;

    private int index = -1;
    private float degreesPerSecond = -1f;

    public void Initialize(float scale, Material matColor, int index, float degreesPerSecond)
    {
        planetMesh.transform.localScale = new Vector3(scale, scale, scale);
        planetMesh.transform.localPosition = new Vector3(0f, 0f, 0f);
        planetMeshRenderer.material = matColor;

        this.degreesPerSecond = degreesPerSecond;

        this.index = index;
    }

    /// <summary>
    /// Given the positiondistance and knowing your index, set your position
    /// </summary>
    public void SetPosition(float positionDistance)
    {
        if(index == -1)
        {
            Debug.LogError("can't set position - not init");
            return;
        }

        this.transform.localPosition = new Vector3(positionDistance * (index + 1), 0f, 0f);

    }


    public int GetIndex()
    {
        return this.index;
    }

    public float GetDegreesPerSecond()
    {
        return this.degreesPerSecond;
    }

    // todo reset planet (to send back to pool)
}
