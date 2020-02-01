using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchyTape : MonoBehaviour
{
    [Header("Outlets")]
    public StretchyTapeEnd EndA;
    public StretchyTapeEnd EndB;
    public StretchyTapeBody Body;

    public Transform DebugEndAPos;
    public Transform DebugEndBPos;

    private float initialLength;
    private Vector3 initialLocalScale;

    void Start()
    {
        initialLength = Body.transform.localScale.x;
        initialLocalScale = Body.transform.localScale;

        DrawTapeBetween(DebugEndAPos.position, DebugEndBPos.position);

    }

    public void DrawTapeBetween(Vector3 a, Vector3 b)
    {
        // deform it
        float length = Mathf.Abs(Vector3.Distance(a, b));
        Body.transform.localScale = new Vector3(initialLocalScale.x, initialLocalScale.y, length);

        // place at midpoint
        Body.transform.position = (a + b) / 2f;

        // rotate it // its forward transform should point to one endpoint
        Body.transform.LookAt(a);
    }



}
