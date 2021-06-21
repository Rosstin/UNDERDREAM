using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM92 : BaseController
{
    [SerializeField] private float restartTime;

    private float elapsed;

    private void Update()
    {
        elapsed += Time.deltaTime;
        BaseUpdate();

        if(elapsed > restartTime)
        {
            LoadNextScene();
        }
    }
}
