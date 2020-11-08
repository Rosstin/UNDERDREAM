using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public float TreeJitterTimeSeconds;

    public BoxCollider2D TreeCollider;
    [SerializeField] private Jitter treeJitter;

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

        yield return new WaitForSeconds(TreeJitterTimeSeconds);
        treeJitter.JitterEnabled = false;
    }


}
