using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM92 : BaseController
{
    [SerializeField] private float restartTime;
    [SerializeField] private float holdInputTime;

    private float elapsed;

    new void Start()
    {
        Steamworks.SteamUserStats.SetAchievement("EAT_THE_LEMON");
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        BaseUpdate();

        if (elapsed > restartTime 
            ||
            (elapsed > holdInputTime && CommandsStartedThisFrame.ContainsKey(Command.Fire)))
        {
            LoadNextScene();
        }
    }
}
