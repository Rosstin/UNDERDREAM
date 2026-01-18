using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikolaiController : MonoBehaviour
{

    [Header("Configs")]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float distanceBetweenSpheres;

    [Header("Prefab Ref")]
    public SpawnedSphere spherePrefabRef;

    public List<SpawnedSphere> sphereList = new List<SpawnedSphere>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnedSphere newSphere = Instantiate(spherePrefabRef);
        sphereList.Add(newSphere);
        newSphere.transform.position = startPoint;



    }

}
