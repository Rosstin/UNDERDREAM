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

    private void Start()
    {
        MakeNormal();
    }

    public void MakeNormal()
    {
        spriteRenderer.color= normalColor;
    }

    public void MakeCorrect()
    {
        spriteRenderer.color = correctColor;
    }

    public void MakeWrong()
    {
        spriteRenderer.color = wrongColor;
    }

    public void MakeReady()
    {
        spriteRenderer.color = readyColor;
    }


}
