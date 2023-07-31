using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// A bar in the bar graph that can be moved by the player
/// </summary>
public class Bar : XRSimpleInteractable, IMoveable, IEquatable<Bar>, IComparable<Bar>
{
    [Header("Configurable Scale")]
    [SerializeField] [Range(0f, 2f)] private float widthScaleFactor; // 1 is 1 Unity meter wide
    [SerializeField] [Range(0f, 2f)] private float depthScaleFactor; // 1 is 1 Unity meter wide

    [Header("Color Material Outlets")]
    public Material Color0Minus;
    public Material Color1;
    public Material Color2;
    public Material Color3;
    public Material Color4;
    public Material Color5;
    public Material Color6Plus;
    public Material SelectedColor;

    [Header("Component References")]
    [SerializeField] private GameObject myBody;
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private TMPro.TextMeshPro myIndexText;
    [SerializeField] private TMPro.TextMeshPro myValueText;

    [Header("SFX")]
    [SerializeField] private AudioSource selectSfx;
    [SerializeField] private AudioSource unselectSfx;

    [Header("Movement")]
    [SerializeField] private float travelTime;
    [SerializeField] AnimationCurve moveCurve;

    private Vector3 startPos;
    private Vector3 destinationPos;

    // for move anim
    private float elapsedTime = 0;

    // private data to use for self operation
    private BarData myData;
    private IMoveable.MoveableState currentState;
    private Graph myParentGraph;
    private int myListIndex; // where the bar would be in its parent array

    public IMoveable.MoveableState CurrentState { get => currentState; set => currentState = value; }

    public void SetDestinationLocalPos(Vector3 destination)
    {
        destinationPos = destination;
    }

    public void SetCurrentPosInstantly(Vector3 newPos)
    {
        this.transform.localPosition = newPos;
    }

    /// <summary>
    /// The bar's position in the list of bars
    /// </summary>
    /// <returns></returns>
    public int GetListIndex()
    {
        return myListIndex;
    }

    /// <summary>
    /// Your position in your parent list changed. Update it
    /// </summary>
    /// <param name="newListIndex"></param>
    public void UpdateListIndex(int newListIndex)
    {
        this.myListIndex = newListIndex;
    }

    /// <summary>
    /// The value of the bar's positional "index" field that's used for movement - distinct from its orig pos
    /// This changes when you move the bar around
    /// </summary>
    /// <returns></returns>
    public int GetPositionalIndex()
    {
        return myData.PositionalIndex;
    }

    /// <summary>
    /// The bar was selected. This method is called via an in-editor script on the Bar parent object. (OnSelectInteractible for the VR controller, Button for the mouse)
    /// </summary>
    public void OnSelect()
    {
        EnterSelectState();
    }


    /// <summary>
    /// Update the positional index used for positioning the bar
    /// </summary>
    public void UpdatePositionalIndex(int newX)
    {
        myData.PositionalIndex = newX;
    }

    public void OnUnselect()
    {
        EnterUnselectState();
    }

    private void Update()
    {
        switch (currentState)
        {
            case IMoveable.MoveableState.Selected:
                UpdateSelectedState();
                break;
            case IMoveable.MoveableState.Unselected:
                UpdateUnselectedState();
                break;
        }
    }

    private void UpdateSelectedState(){}

    /// <summary>
    /// If not selected, approach destination pos
    /// </summary>
    private void UpdateUnselectedState(){
        elapsedTime += Time.deltaTime;

        float progress = moveCurve.Evaluate(elapsedTime / travelTime);

        this.transform.localPosition = Vector3.Lerp(startPos, destinationPos, progress);
    }

    /// <summary>
    /// Inflate the bar from its data
    /// </summary>
    /// <param name="myData"></param>
    /// <param name="primaryButtonWatcher"></param>
    /// <param name="myParentGraph"></param>
    /// <param name="myListIndex"></param>
    public void Init(BarData myData, Graph myParentGraph, int myListIndex)
    {
        currentState = IMoveable.MoveableState.Unselected;
        this.myData = myData;
        this.myParentGraph = myParentGraph;
        this.myListIndex = myListIndex;

        // your height is equal to your value. width/depth determined by configurable scale factor
        myBody.transform.localScale = new Vector3(widthScaleFactor, myData.Value, depthScaleFactor);


        // todo calculate your appropriate position by assuming that the span of the graph has certain values

        // your body should rest such that the bottom of the mesh is at this parent's origin
        myBody.transform.localPosition = new Vector3(0f, myData.Value / 2f, 0f);

        // start pos for smooth movement
        this.startPos = this.transform.localPosition;

        // your color is correlated to your value
        SetColor();

        myIndexText.text = myData.OriginalIndex+"";
        myValueText.text = myData.Value+"";
    }
    private void EnterSelectState()
    {
        myParentGraph.BarSelected(this);
        currentState = IMoveable.MoveableState.Selected;
        myRenderer.material = SelectedColor; // set color to selected color
        selectSfx.Play();
    }

    private void EnterUnselectState()
    {
        elapsedTime = 0f;
        myParentGraph.BarSelected(null);
        currentState = IMoveable.MoveableState.Unselected;
        SetColor(); // set color to appropriate color
        unselectSfx.Play();
    }

    /// <summary>
    /// Set color to the appropriate color for your value
    /// </summary>
    private void SetColor()
    {
        if (myData.Value < 1)
        {
            myRenderer.material = Color0Minus;
        }
        else if (myData.Value >= 1 && myData.Value < 2)
        {
            myRenderer.material = Color1;
        }
        else if (myData.Value >= 2 && myData.Value < 3)
        {
            myRenderer.material = Color2;
        }
        else if (myData.Value >= 3 && myData.Value < 4)
        {
            myRenderer.material = Color3;
        }
        else if (myData.Value >= 4 && myData.Value < 5)
        {
            myRenderer.material = Color4;
        }
        else if (myData.Value >= 5 && myData.Value < 6)
        {
            myRenderer.material = Color5;
        }
        else if (myData.Value >= 6)
        {
            myRenderer.material = Color6Plus;
        }
    }

    public bool Equals(Bar other)
    {
        if(other == null) return false;
        return (this.myData.PositionalIndex.Equals(other.myData.PositionalIndex));
    }

    public int CompareTo(Bar other)
    {
        if (other == null)
        {
            return 1;
        }
        else
        {
            return this.myData.PositionalIndex.CompareTo(other.myData.PositionalIndex);
        }
    }
}
