using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesController : MonoBehaviour
{

    [Header("Configs")]
    public int numLines = 5;
    public float verticalSpacing = 1f;
    public float horizSpacing = 1f;
    public int lineLength = 8;

    [Header("Prefabs")]
    public HorizLine horizPrefab;

    public float yNoise = 0.2f; // positive or negative vertical noise

    private List<LineRenderer> lines = new List<LineRenderer>();

    public void Start()
    {
        for(int i= 0; i < numLines; i++)
        {
            DrawHoriz(i, lineLength);
        }

    }


    public LineRenderer DrawHoriz(int index, int lineLength)
    {
        LineRenderer l = CreateLine(lineLength);

        float yBaseline = index * verticalSpacing;

        Vector3 origin = new Vector3(0f, yBaseline, 0f);

        List<Vector3> linePositions = new List<Vector3>();

        l.SetPosition(0, origin);
        linePositions.Add(origin);

        for(int i = 1; i < lineLength; i++)
        {

            float randomYNoise = UnityEngine.Random.RandomRange(-yNoise, +yNoise);

            Vector3 point = new Vector3(i * horizSpacing, randomYNoise + yBaseline, 0f);



            l.SetPosition(i, point);

            Debug.Log("point " + point + " at pos " + i);

            linePositions.Add(point);
        }





        return l;

    }

    public LineRenderer CreateLine(int numVerts) {


        HorizLine newLine = Instantiate(horizPrefab);

        LineRenderer lineRenderer = newLine.gameObject.AddComponent<LineRenderer>();

        // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;

        // Set the number of vertices
        lineRenderer.positionCount = numVerts;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lines.Add(lineRenderer);

        return lineRenderer;
    }


/*    public void DrawLine(Vector3 start, Vector3 end)
    {




        // Set the positions of the vertices
        lineRenderer.SetPosition(start, end);

    }

*/
}
