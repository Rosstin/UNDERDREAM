using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;


/// <summary>
/// A bar in the bar graph that can be moved by the player
/// </summary>
public class Bar :  MonoBehaviour, IMoveable
{
    [Header("Color Prefabs")]
    public Material Color0Minus;
    public Material Color1;
    public Material Color2;
    public Material Color3;
    public Material Color4;
    public Material Color5;
    public Material Color6Plus;

    [Header("Component References")]
    [SerializeField] private GameObject myBody;
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private TMPro.TextMeshProUGUI myIndexText;
    [SerializeField] private TMPro.TextMeshProUGUI myValueText;

    private BarData myData;

    public void Init(BarData myData)
    {
        this.myData = myData;

        // your height is equal to your value
        myBody.transform.localScale = new Vector3(1f, myData.Value, 1f);

        // your body should rest such that the bottom of the mesh is at this parent's origin
        myBody.transform.localPosition = new Vector3(0f, myData.Value / 2f, 0f);

        // your color is correlated to your value
        SetColor();
    }

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
