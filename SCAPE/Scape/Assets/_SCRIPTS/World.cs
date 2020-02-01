using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldState {
    INITIAL = 0,
    PLAY_WITH_CAT,
    ENDGAME,
};

public class World : Singleton<World>
{
    public WorldState state{
    get{
        return _state;
    }
    set{
        WorldState oldState = _state;
        _state = value;
        SpawnManager.Instance.OnWorldStateChange(oldState, _state);
    }
    }
    private WorldState _state;

    protected World() {}

    private void Start()
    {
        // Stage 1: wait for cat to spawn
        // Stage 2: wait for other things to spawn
    }
}
