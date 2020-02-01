using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchpadBController : MonoBehaviour
{
    private DuctTapeController stuckTape = null;

    public void StickToTape(DuctTapeController tape)
    {
        this.stuckTape = tape;
    }

    void Update()
    {
        
    }
}
