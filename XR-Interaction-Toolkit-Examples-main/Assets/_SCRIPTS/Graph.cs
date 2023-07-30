using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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

    [Header("Hand References For Raycasting")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHandMarker;
    [SerializeField] private GameObject rightHandMarker;

    [Header("Primary Button Watcher")]
    [SerializeField] private PrimaryButtonWatcher primaryButtonWatcher;

    private List<Bar> bars = new List<Bar>();

    public Vector3 RightHandHitPosition { get; set; } // records the current hit pos. NegativeInfinity means no hit

    private Bar selectedBar = null; // null means no bar selected

    public void BarSelected(Bar selectedBar)
    {
        this.selectedBar = selectedBar;
    }

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
        RecordGripButtonState();

        RaycastFromRightHand();

        UpdateSelectedBar();
    }

    private void UpdateSelectedBar()
    {
        if(selectedBar != null)
        {
            selectedBar.transform.position = RightHandHitPosition;
        }
    }

    private void RaycastFromRightHand()
    {
        // RAYCAST FROM RIGHT HAND

        // build the ray
        RaycastHit hit = new RaycastHit(); // this object will collect data about the collision each frame

        // mask layer 7, where the backing object is
        int layer = 7;
        int layerMask = 1 << layer;

        // we will shoot a ray out based on the righthand, masking so that we can only hit the "backing" object
        if (Physics.Raycast(ray: new Ray(rightHand.transform.position, rightHand.transform.forward), out hit, maxDistance: 100f, layerMask: layerMask)) // do the raycast, based on the camera's position and orientation, and store the hit for our reference
        {
            rightHandMarker.transform.position = hit.point;
            rightHandMarker.gameObject.SetActive(true);
            RightHandHitPosition = hit.point;
        }
        else // there wasn't any collision
        {
            rightHandMarker.gameObject.SetActive(false);
            RightHandHitPosition = Vector3.negativeInfinity;
        }

    }

    private void RecordGripButtonState()
    {
        // RECORD STATE OF GRIP BUTTON
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        string output = "";
        foreach (var device in inputDevices)
        {
            bool gripValue;

            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripValue))
            {
                //output += "" + device.serialNumber + " grip " + gripValue + ". ";
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
        selectedBar = null;

        // wipe existing bars
        foreach (var bar in bars)
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
            bars[i].transform.localScale = new Vector3(1f, 1f, 1f);

            // initialize it
            bars[i].Init(BarsData[i], primaryButtonWatcher, this);

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

