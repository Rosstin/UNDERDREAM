using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public WorldState EnableOnPhase;

    public void OnWorldStateChange(WorldState oldState, WorldState newState) {
        Debug.Log("state changed to " + newState);
        if (this.EnableOnPhase == newState) {
            this.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
    }
}
