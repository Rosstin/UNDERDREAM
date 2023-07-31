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
    public List<BarData> BarsData;
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

    public Vector3 hitPos { get; set; } // records the current hit pos 

    private List<Bar> bars = new List<Bar>();

    // private vars for interaction
    private bool validHitThisFrame = false;
    private bool rightGripDown = false;
    private Bar selectedBar = null; // null means no bar selected

    public void BarSelected(Bar selectedBar)
    {
        this.selectedBar = selectedBar;
    }

    public void ResetBarPos(int barListIndex)
    {
        InitAndPlaceBar(bars, barListIndex);
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
            if (!relevantButtonDown)
            {
                DropBar(selectedBar);
            }
            else if (validHitThisFrame)
            {
                // must convert hitpos to be local
                var localHitPos = this.transform.InverseTransformPoint(hitPos);

                // now get the new index calc
                int newIndex = GetIndexValueFromXLocalPos(localHitPos.x);
                selectedBar.UpdateIndex(newIndex);

                // move bar only in X
                // reconvert back to grainify 
                selectedBar.transform.localPosition = new Vector3( GetXLocalPosFromIndexValue(newIndex), selectedBar.transform.localPosition.y, selectedBar.transform.localPosition.z);
            }
        }
    }

    private void DropBar(Bar bar)
    {
        // drop it
        bar.OnUnselect();

        Debug.Log("dropping bar w/ index value " + bar.GetIndexValue() 
            + " .. BarsData[bar.GetListIndex()].Index: " + BarsData[bar.GetListIndex()].Index
            + " .. ");

        // update barsdata object with current data
        BarsData[bar.GetListIndex()].Index = bar.GetIndexValue();

        // re-sort the bars based on their new index values, then place the bars in the correct order
        bars.Sort();
        BarsData.Sort();

        Debug.Log("BarsData[bar.GetListIndex()].Index after update: " + BarsData[bar.GetListIndex()].Index
    + " .. ");

        // update what you know about your own position to be accurate again
        for (int i = 0; i < bars.Count; i++)
        {
            bars[i].UpdateListIndex(i);
        }

        // reposition
        PlaceBars();
    }

    private void SortBarsOnIndexData()
    {
        
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
        }

        PlaceBars();
    }

    private void PlaceBars()
    {
        // inflate bars from data
        for (int i = 0; i < BarsData.Count; i++) {
            InitAndPlaceBar(bars, i);
        }
    }

    private void InitAndPlaceBar(List<Bar> bars, int barListIndex)
    {
        // initialize it
        bars[barListIndex].Init(BarsData[barListIndex], this, barListIndex);

        // sort the bars based on their list index position, not actual stated Index Value

        // a fake index val based only on list order, 
        int virtualIndexVal = (barListIndex+1) * 10; // todo parameterize magic numbers

        float localXPos = GetXLocalPosFromIndexValue(virtualIndexVal);

        // really place the bar
        bars[barListIndex].transform.localPosition = new Vector3(localXPos, 0f, 0f);

        // convert back to validate

        var validationIndexVal = GetIndexValueFromXLocalPos(localXPos); // todo should be unit test instead

        //Debug.Log("Orig index val: " + virtualIndexVal + ".. localxpos: " + localXPos + " .. reconverted index (should match first val): " + validationIndexVal);
        if(validationIndexVal != virtualIndexVal)
        {
            //Debug.LogError("indexVal: " + virtualIndexVal + " does not match " + validationIndexVal + "! A logical error - these methods should be inverses");
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
    public int Index;
    public int Value;

    public bool Equals(BarData other)
    {
        if (other == null) return false;
        return (this.Index.Equals(other.Index));
    }

    public int CompareTo(BarData other)
    {
        if (other == null)
        {
            return 1;
        }
        else
        {
            return this.Index.CompareTo(other.Index);
        }
    }

}

