﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotatePerSecond;

    public bool RandomStart;

    private void Start()
    {
        if (RandomStart)
        {
            var randomRot = Random.Range(0f, 360f);
            this.gameObject.transform.Rotate(0,0,randomRot,Space.Self);
        }
    }

    private void Update()
    {
        var rotThisFrame = RotatePerSecond * Time.deltaTime;
        this.gameObject.transform.Rotate(0,0, rotThisFrame, Space.Self);
    }
}
