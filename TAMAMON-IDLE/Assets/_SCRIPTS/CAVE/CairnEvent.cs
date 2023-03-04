using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CairnEvent : TamaEvent
{
    public override bool EventCanFire()
    {
        return true;
    }

    public override void Play()
    {
        Debug.LogWarning("Play cairn event");
    }
}
