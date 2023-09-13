using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleEventTrigger : TamaEvent
{
    [Header("Event Outlets")]
    [SerializeField] private GameObject previewObject;
    [SerializeField] private GameObject previewObjectStartPos;
    [SerializeField] private GameObject previewObjectEndPos;

    public override GameObject PreviewObject => previewObject;
    public override GameObject PreviewObjectStartPos => previewObjectStartPos;
    public override GameObject PreviewObjectEndPos => previewObjectEndPos;


    public override bool EventCanFire()
    {
        return true;
    }

}
