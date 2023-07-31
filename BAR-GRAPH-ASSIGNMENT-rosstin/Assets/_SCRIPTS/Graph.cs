using System;
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
    public List<BarData> BarsData; // data for bar init conditions. copied on start so it can be modified and reset to initial conditions
    [SerializeField] private Transform XMax; // an object transform representing the bar's max X (probably 60f)
    [SerializeField] private Transform XMin; // an object transform representing min X (0f)
    [SerializeField] [Range(0f, 100f)] private float MaxIndexValue; // this value represents the right side of the graph's X extent. Should be 60f.

    [Header("Prefabs")]
    [SerializeField] private Bar barPrefab;

    [Header("Test Output Text")]
    [SerializeField] private TMPro.TextMeshPro outputText;

    [Header("Reset Button")]
    [SerializeField] private UnityEngine.UI.Button resetButton;

    [Header("Hand References For Raycasting")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject xrParent;

    public Vector3 hitPos { get; set; } // records the current hit pos 

    private List<Bar> bars = new List<Bar>();

    private List<BarData> ModifiedBarsData;

    // private vars for interaction
    private bool validHitThisFrame = false;
    private bool rightGripDown = false;
    private Bar selectedBar = null; // null means no bar selected

    /// <summary>
    /// Set the selected bar. This can be null to represent no bar selected.
    /// </summary>
    /// <param name="newSelectedBar"></param>
    public void SetSelectedBar(Bar selectedBar)
    {
        Debug.Log("SetSelectedBar new selected bar is " + selectedBar);
        this.selectedBar = selectedBar;
    }

    /// <summary>
    /// Using the OriginalIndex as ID, check if passed-in bar is the selected bar
    /// </summary>
    /// <param name="bar"></param>
    public bool IsThisSelectedBar(Bar bar)
    {
        // if no bar is selected, always false
        if (selectedBar == null) return false;
        // otherwise, check the original index val. if they match, the passed in bar is the selected bar
        return bar.GetOriginalIndexValue() == selectedBar.GetOriginalIndexValue();
    }

    private void Awake()
    {
        resetButton.onClick.AddListener(Reset);
    }

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        Debug.Log("Reset");

        // wipe the modified data and copy the original data back in
        ModifiedBarsData = new List<BarData>();

        // copy the bars data so that you only modify a copy - this way you can reset more easily
        for (int i = 0; i < BarsData.Count; i++)
        {
            ModifiedBarsData.Add(new BarData(BarsData[i]));
        }

        MakeBars();
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
        for (int i = 0; i < ModifiedBarsData.Count; i++)
        {
            // set positional data for barsdata. initially, the position is based on the data from the inspector
            ModifiedBarsData[i].PositionalIndex = ModifiedBarsData[i].OriginalIndex;

            // create the bar 
            Bar newBar = Instantiate(barPrefab);

            // parent it
            newBar.transform.parent = this.transform;
            newBar.transform.localScale = new Vector3(1f, 1f, 1f);

            // and position it correctly in Z (at the z origin)
            newBar.transform.localPosition = new Vector3(0f, 0f, 0f);

            // add to list
            bars.Add(newBar);

            // initialize it
            bars[i].Init(ModifiedBarsData[i], this, i);

        }

        PlaceBars();
    }

    private void Update()
    {
        RecordGripButtonState();

        RaycastForAppropriatePlatform();

        UpdateSelectedBar();
    }

    /// <summary>
    /// Get the appropriate input device and update a bar that's been selected
    /// </summary>
    private void UpdateSelectedBar()
    {
        bool isEditor = false;
        // are we in the editor instead of in VR? if so, rely on mouse instead of controller
        if (Application.isEditor)
        {
            isEditor = true;
        }

        bool relevantButtonDown = false;
        if (isEditor)
        {
            if (Input.GetMouseButton(0))
            {
                relevantButtonDown = true;
            }
        }
        else
        {
            relevantButtonDown = rightGripDown;
        }

        if(selectedBar != null)
        {
            // 1. If the bar is getting dropped, drop it
            if (!relevantButtonDown)
            {
                DropBar(selectedBar);
                return;
            }


            // 2. CALCULATE WHERE BAR SHOULD BE
            // must convert hitpos to be local
            var localHitPos = this.transform.InverseTransformPoint(hitPos);
            // now get the new index calc
            int newIndex = GetIndexValueFromXLocalPos(localHitPos.x);

            if (validHitThisFrame)
            {
                // move bar only in X
                var xLocalPos = GetXLocalPosFromIndexValue(newIndex);
                selectedBar.SetCurrentPosInstantly(
                    new Vector3(xLocalPos, selectedBar.transform.localPosition.y, selectedBar.transform.localPosition.z));
            }

            // update your positional index
            // this may trigger the other bars to reorder
            selectedBar.UpdatePositionalIndexAndReorderBarsIfNecessary(newIndex);
        }
    }

    private void DropBar(Bar bar)
    {
        // drop it
        bar.OnUnselect();

        // update barsdata object with current data
        ModifiedBarsData[bar.GetListIndex()].PositionalIndex = bar.GetPositionalIndex();

        TriggerBarReorder();
    }

    public void TriggerBarReorder()
    {
        // re-sort the bars based on their new index values, then place the bars in the correct order
        bars.Sort();
        ModifiedBarsData.Sort();

        // update what you know about your own position to be accurate again
        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].UpdateListIndex(i);
        }

        // reposition
        PlaceBars();
    }

    private void PlaceBars()
    {
        // place based on data
        for (int i = 0; i < ModifiedBarsData.Count; i++)
        {
            PlaceBar(bars, i);
        }
    }

    private void PlaceBar(List<Bar> bars, int barListIndex)
    {
        // sort the bars based on their list index position, not actual stated Index Value
        // a fake index val based only on list order, 
        int virtualIndexVal = (barListIndex + 1) * 10; // todo parameterize magic numbers

        // update the bar's positional data with this index val
        ModifiedBarsData[barListIndex].PositionalIndex = virtualIndexVal;

        float localXPos = GetXLocalPosFromIndexValue(virtualIndexVal);

        // really place the bar
        bars[barListIndex].SetDestinationLocalPos(new Vector3(localXPos, 0f, 0f));

        foreach (Bar bar in bars)
        {
            // if this isnt the currently selected bar trigger unselect to start moving
            if (selectedBar == null || bar.GetOriginalIndexValue() != selectedBar.GetOriginalIndexValue())
            {
                bar.OnUnselect();
            }
            else
            {
                Debug.Log("selectedbar is: " + selectedBar + " and shouldnt be moved");
            }
        }
    }

    private void RaycastForAppropriatePlatform()
    {
        validHitThisFrame = false;

        // build the ray
        RaycastHit hit = new RaycastHit(); // this object will collect data about the collision each frame

        Ray ray;

        // mask layer 7, where the backing object is
        int layer = 7;
        int layerMask = 1 << layer;

        // use mouse since we're in editor
        if (Application.isEditor)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        // assume RH controller otherwise
        else
        {
            ray = new Ray(rightHand.transform.position, rightHand.transform.forward);
        }

        // we will shoot a ray out, masking so that we can only hit the "backing" object
        if (Physics.Raycast(ray: ray, out hit, maxDistance: 100f, layerMask: layerMask)) // do the raycast, based on the camera's position and orientation, and store the hit for our reference
        {
            hitMarker.transform.position = hit.point;
            hitMarker.gameObject.SetActive(true);
            hitPos = hit.point;
            validHitThisFrame = true;
        }
        else // there wasn't any collision
        {
            hitMarker.gameObject.SetActive(false);
            hitPos = Vector3.one;
            validHitThisFrame = false;
        }
    }

    private void RecordGripButtonState()
    {
        // RECORD STATE OF GRIP BUTTON
        var rightHandedDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, rightHandedDevices);
        string output = "";

        if(rightHandedDevices.Count > 1)
        {
            Debug.LogError("Only one right handed device is currently supported - strange behavior may result");
        }

        if(rightHandedDevices.Count > 0)
        {
            if (rightHandedDevices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out rightGripDown))
            {
                output += "" + rightHandedDevices[0].serialNumber + " grip " + rightGripDown + ". ";
            }
            outputText.text = output;
        }
        else
        {
            //Debug.LogError("No right-handed device detected..");
        }

    }



    /// <summary>
    /// Given the index value, calculate relative X pos.
    /// Must be the logical inverse of GetIndexValueFromXLocalPos or strange behavior will result [todo: unit test this]
    /// NOTE: GetIndexValueFromXLocalPos converts to an int, so it is not a strict inverse.
    /// </summary>
    /// <param name="pos"></param>
    private float GetXLocalPosFromIndexValue(int indexVal)
    {
        float relPos = indexVal / MaxIndexValue;

        return Mathf.Lerp(a: XMin.localPosition.x, b: XMax.localPosition.x, t: relPos);
    }

    /// <summary>
    /// Given the relative X pos, calculate index val
    /// Must be logical inverse of GetXLocalPosFromIndexValue or strange behavior will result [todo: unit test this]
    /// NOTE: this method converts to an int, so it is not a strict inverse.
    /// </summary>
    /// <param name="localPositionX"></param>
    /// <returns></returns>
    private int GetIndexValueFromXLocalPos(float localPositionX)
    {
        float relPos = Mathf.InverseLerp(XMin.localPosition.x, XMax.localPosition.x, localPositionX);

        // calculate it and floor it to int value
        int indexVal = (int) (relPos * MaxIndexValue);

        return indexVal;
    }



}

[System.Serializable]
public class BarData : IEquatable<BarData>, IComparable<BarData>
{
    [HideInInspector] public int PositionalIndex; // where it currently is // used for positioning, shouldnt be configured
    public int OriginalIndex; // sets initial position, can be configured in inspector
    public int Value; // height, can be configured in inspector

    public BarData(BarData sourceBarData)
    {
        this.PositionalIndex= sourceBarData.PositionalIndex;
        this.OriginalIndex = sourceBarData.OriginalIndex;
        this.Value = sourceBarData.Value;
    }

    public bool Equals(BarData other)
    {
        if (other == null) return false;
        return (this.PositionalIndex.Equals(other.PositionalIndex));
    }

    public int CompareTo(BarData other)
    {
        if (other == null)
        {
            return 1;
        }
        else
        {
            return this.PositionalIndex.CompareTo(other.PositionalIndex);
        }
    }

}

