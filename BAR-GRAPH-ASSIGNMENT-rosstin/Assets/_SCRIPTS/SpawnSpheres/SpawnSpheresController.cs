using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpheresController : MonoBehaviour
{
    [Header("Configs")]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float distanceBetweenSpheres;

    [Header("Prefab Ref")]
    public SpawnedSphere spherePrefabRef;

    public List<SpawnedSphere> sphereList = new List<SpawnedSphere>();

    private void Start()
    {

        SpawnedSphere newSphere = Instantiate(spherePrefabRef);
        sphereList.Add(newSphere);
        newSphere.transform.position = startPoint;


        // add distanceBetweenSpheres to startpoint towards endpoint
        // if we haven't surpassed endpoint, draw a new sphere
        // otherwise, stop

        // how many line segments fit into a line

        float totalDist = 0;
        float maxDist = Vector3.Distance(startPoint, endPoint);

        Vector3 normalizedDir = (endPoint - startPoint).normalized;

        Vector3 unitToAdd = distanceBetweenSpheres * normalizedDir;

        Vector3 currentPointPos = startPoint;

        int numSpheres = (int) (maxDist / distanceBetweenSpheres); //not including start sphere

        for(int i = 0; i < numSpheres; i++)
        {
            currentPointPos = currentPointPos + unitToAdd;

            SpawnedSphere newSphere2 = Instantiate(spherePrefabRef); // object pooling
            sphereList.Add(newSphere2);
            newSphere2.transform.position = currentPointPos;
        }
    }


}
