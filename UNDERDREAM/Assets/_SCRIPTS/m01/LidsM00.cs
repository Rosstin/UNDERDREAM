using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidsM00 : Lids
{
    new private void Start()
    {
        base.Start();

        Data.CurrentScene = 0; // current scene is first scene
    }
}
