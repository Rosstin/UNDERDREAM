using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The graph, which contains the bars
/// </summary>
public class Graph : MonoBehaviour
{
    [Header("Configurable Graph Data")]
    public List<BarData> BarsData;
    public float XScaleFactor; // convert between unity meters and the graph's units - 0.1 would mean that 1 unity meter is 10 graph units in the X

    [Header("Prefabs")]
    [SerializeField] private Bar barPrefab;


    private List<Bar> bars = new List<Bar>();

    private void Start()
    {
        MakeBars();
    }

    private void MakeBars()
    {
        // wipe existing bars
        foreach(var bar in bars)
        {
            Destroy(bar.gameObject);
        }

        // initialize the list of bars
        bars = new List<Bar>();

        // inflate bars from data
        for(int i = 0; i < BarsData.Count; i++)
        {
            // create the bar
            bars.Add(Instantiate(barPrefab));

            bars[i].transform.parent = this.transform;

            // initialize it
            bars[i].Init(BarsData[i]);

            // place it
            bars[i].transform.localPosition = new Vector3(BarsData[i].Index * XScaleFactor, 0f, 0f);
        }
    }


}

[System.Serializable]
public class BarData
{
    public int Index;
    public int Value;
}

