using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM93 : BaseController
{
    [SerializeField] private float restartTime;
    [SerializeField] private float holdInputTime;

    private float elapsed;

    private void Update()
    {
        elapsed += Time.deltaTime;
        BaseUpdate();

        if (elapsed > restartTime 
            ||
            (elapsed > holdInputTime && CommandsStartedThisFrame.ContainsKey(Command.Fire)))
        {
            Application.Quit();
        }
    }
}
