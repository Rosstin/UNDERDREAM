using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// A bar in the bar graph that can be moved by the player
/// </summary>
public class Bar : XRSimpleInteractable, IXRHoverInteractable, IMoveable
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

    // private data to use for self operation
    private BarData myData;
    private PrimaryButtonWatcher primaryButtonWatcherReference;
    private IMoveable.MoveableState currentState;
    private Graph myParentGraph;
    private int myListIndex; // where the bar would be in its parent array

    public IMoveable.MoveableState CurrentState { get => currentState; set => currentState = value; }

    public int GetListIndex()
    {
        return myListIndex;
    }

    /// <summary>
    /// The bar was selected 
    /// </summary>
    public void OnSelect()
    {
        EnterSelectState();
    }

    /// <summary>
    /// Update the displayed value of Index based on current X
    /// </summary>
    public void UpdateIndex(float newX)
    {
        int integerizedX = (int) newX;

        myData.Index = integerizedX;
        myIndexText.text = integerizedX+"";
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

    private void UpdateUnselectedState(){}



    public void Init(BarData myData, PrimaryButtonWatcher primaryButtonWatcher, Graph myParentGraph, int myListIndex)
    {
        currentState = IMoveable.MoveableState.Unselected;
        this.primaryButtonWatcherReference = primaryButtonWatcher;
        this.myData = myData;
        this.myParentGraph = myParentGraph;
        this.myListIndex = myListIndex;

        // your height is equal to your value. width/depth determined by configurable scale factor
        myBody.transform.localScale = new Vector3(widthScaleFactor, myData.Value, depthScaleFactor);

        // your body should rest such that the bottom of the mesh is at this parent's origin
        myBody.transform.localPosition = new Vector3(0f, myData.Value / 2f, 0f);

        // your color is correlated to your value
        SetColor();

        myIndexText.text = myData.Index+"";
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
}
