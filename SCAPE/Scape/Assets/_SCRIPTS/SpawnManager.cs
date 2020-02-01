using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    protected SpawnManager() {}

    public void OnWorldStateChange(WorldState oldState, WorldState newState) {
        foreach (Spawner child in this.GetComponentsInChildren<Spawner>()) {
            child.OnWorldStateChange(oldState, newState);
        }
    }

    private void Start()
    {
        // Iterate through children and disable 'em to start
        foreach (Spawner child in this.GetComponentsInChildren<Spawner>()) {
            child.gameObject.SetActive(false);
        }
    }
}
