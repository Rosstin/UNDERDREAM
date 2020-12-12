using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{


    [Header("Timing")]
    public float TreeJitterTimeSeconds;
    public float NutDropCooldownSeconds;

    [Header("Public Outlets")]
    public BoxCollider2D TreeCollider;

    [Header("Private Outlets")]
    [SerializeField] private Jitter treeJitter;
    [SerializeField] private List<Coconut> coconuts;

    private int droppedNutIndex = 0;
    private bool readyForNextDrop = true;

    public bool IsReady()
    {
        return readyForNextDrop;
    }

    public int ShakeTree()
    {
        int nutIndex = droppedNutIndex;

        if (nutIndex >= coconuts.Count)
        {
            nutIndex = 99;
        }

        StartCoroutine(Shake());
        return nutIndex;
    }

    private IEnumerator Shake()
    {
        readyForNextDrop = false;

        // play a sound

        // jitter the tree
        treeJitter.JitterEnabled = true;

        // drop a nut
        if(droppedNutIndex < coconuts.Count)
        {
            coconuts[droppedNutIndex].Drop();
            droppedNutIndex++;
        }

        yield return new WaitForSeconds(TreeJitterTimeSeconds);
        treeJitter.JitterEnabled = false;
        yield return new WaitForSeconds(NutDropCooldownSeconds);
        readyForNextDrop = true;
    }


}
