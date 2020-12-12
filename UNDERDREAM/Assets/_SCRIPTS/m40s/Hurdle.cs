using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    public string CorrectCommand;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;
    [SerializeField] private Color readyColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private HurdleState myState;

    public enum HurdleState
    {
        Init,
        Ready,
        Correct,
        Wrong
    }

    public void SetVisible(bool visible)
    {
        this.gameObject.SetActive(visible);
    }

    private void Start()
    {
        MakeNormal();
    }

    public HurdleState GetState()
    {
        return myState;
    }

    public void MakeNormal()
    {
        myState = HurdleState.Init;
        spriteRenderer.color= normalColor;
    }

    public void MakeCorrect()
    {
        myState = HurdleState.Correct;
        spriteRenderer.color = correctColor;
    }

    public void MakeWrong()
    {
        myState = HurdleState.Wrong;
        spriteRenderer.color = wrongColor;
    }

    public void MakeReady()
    {
        myState = HurdleState.Ready;
        spriteRenderer.color = readyColor;
    }


}
