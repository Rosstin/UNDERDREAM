using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{

    [SerializeField] private List<Coconut> coconuts;

    public float TreeJitterTimeSeconds;

    public BoxCollider2D TreeCollider;
    [SerializeField] private Jitter treeJitter;

    private int droppedNutIndex = 0;

    public bool IsJittering()
    {
        return treeJitter.JitterEnabled;
    }

    public void ShakeTree()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
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
    }


}
