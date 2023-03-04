using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CairnEvent : TamaEvent
{
    [SerializeField] private GameObject previewObject;

    public override GameObject PreviewObject => previewObject;

    public override bool EventCanFire()
    {
        return true;
    }

    public override void Play()
    {
        Debug.LogWarning("Play cairn event");
    }
}
