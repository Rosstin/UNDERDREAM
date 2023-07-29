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

    [Header("Prefabs")]
    [SerializeField] private Bar barPrefab;


    private List<Bar> bars;

    public void MakeBars()
    {
        bars.Add(GameObject.Instantiate(barPrefab));

        //for(int i = 0; i < )
        
    }

    [System.Serializable]
    public class BarData
    {
        public int Index;
        public int Value;
    }


}

