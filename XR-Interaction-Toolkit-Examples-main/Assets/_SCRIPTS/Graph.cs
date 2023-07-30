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

    [Header("Test Output Text")]
    [SerializeField] private TMPro.TextMeshPro outputText;

    [Header("Reset Button")]
    [SerializeField] private UnityEngine.UI.Button resetButton;

    private List<Bar> bars = new List<Bar>();

    private void Awake()
    {
        resetButton.onClick.AddListener(Reset);
    }

    private void Reset()
    {
        Debug.Log("Reset");
        MakeBars();
    }

    private void Update()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        string output = "";
        foreach (var device in inputDevices)
        {
            bool gripValue;


            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripValue))
            {
                output += "" + device.serialNumber + " grip " + gripValue + ". ";
            }
        }

        outputText.text = output;
    }

    private void Start()
    {
        Reset();
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

