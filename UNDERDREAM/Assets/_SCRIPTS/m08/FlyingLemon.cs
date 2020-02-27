using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLemon : MonoBehaviour
{
    [Header("Outlets")]
    public Rigidbody2D Body;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    // Start is called before the first frame update
    void Start()
    {
        Body.AddForce(StartingForce);
        Body.AddTorque(StartingTorque);
    }
}
