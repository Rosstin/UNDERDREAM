using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    protected SpawnManager() {}

    public void OnWorldStateChange(WorldState oldState, WorldState newState) {
        Debug.Log(this.GetComponentsInChildren<Spawner>());
        foreach (Spawner child in Resources.FindObjectsOfTypeAll<Spawner>()) {
            child.OnWorldStateChange(oldState, newState);
        }
    }

    private void Start()
    {
        // Iterate through children and disable 'em to start
        WorldState initialState = World.Instance.state;
        foreach (Spawner child in this.GetComponentsInChildren<Spawner>()) {
            child.gameObject.SetActive(
                child.EnableOnPhase == initialState
            );
        }
    }
}
