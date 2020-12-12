using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    public string CorrectCommand;

    [SerializeField] protected Color normalColor;
    [SerializeField] protected Color correctColor;
    [SerializeField] protected Color wrongColor;
    [SerializeField] protected Color readyColor;
    [SerializeField] protected SpriteRenderer spriteRenderer;

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
