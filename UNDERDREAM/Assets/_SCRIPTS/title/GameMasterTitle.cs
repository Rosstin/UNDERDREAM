using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterTitle : BaseController
{
    private bool started = false;

    private void Update()
    {
        BaseUpdate();

        if (!started && CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {
            started = true;
            


        }
    }

}
